using Microsoft.Extensions.Logging.Abstractions;
using StorageStrategy.Domain.Commands.Print;
using StorageStrategy.Domain.Handlers.Print;
using StorageStrategy.Models;
using StorageStrategy.Printer;
using StorageStrategy.Tests.FakeRepository;
using Xunit;
using static StorageStrategy.Tests.Handler.PrintFixtures;

namespace StorageStrategy.Tests.Handler
{
    /// <summary>US1: funcionário solicita a impressão da comanda.</summary>
    public class CreatePrintJobHandlerTest : HandlerBaseTest
    {
        private readonly FakePrintJobRepository _jobs = new();
        private readonly FakePrintStationRepository _stations = new();
        private readonly FakeTimeProvider _time = new();

        private CreatePrintJobHandler Handler() =>
            new(_jobs, _stations, new ReceiptRenderer(), _time, NullLogger<CreatePrintJobHandler>.Instance);

        private static CreatePrintJobCommand Pedido(int commandId = 5, int companyId = EmpresaA, int employeeId = 10) =>
            new() { CommandId = commandId, CompanyId = companyId, EmployeeId = employeeId };

        [Fact(DisplayName = "Cria job Aguardando com o papel pronto e 1ª via")]
        public async Task Sucesso_cria_job()
        {
            _jobs.commands.Add(Comanda(5, EmpresaA));
            Estacao(_stations, EmpresaA, lastSeenAt: _time.Now.AddSeconds(-10));

            var result = await Handler().Handle(Pedido(), CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal("Enviado para impressão", result.Message);
            var response = Assert.IsType<PrintJobResponse>(result.Response);
            Assert.Equal(PrintJobStatusEnum.Aguardando, response.Status);
            Assert.Equal(1, response.Copy);
            Assert.True(response.StationOnline);

            var job = Assert.Single(_jobs.jobs);
            Assert.Equal(EmpresaA, job.CompanyId);
            Assert.Equal(10, job.RequestedByEmployeeId);
            Assert.Equal(new byte[] { 0x1B, 0x40 }, job.Payload.Take(2).ToArray());
        }

        [Fact(DisplayName = "Estação offline: job é criado mesmo assim e a resposta avisa")]
        public async Task Estacao_offline_cria_job_e_avisa()
        {
            _jobs.commands.Add(Comanda(5, EmpresaA));
            Estacao(_stations, EmpresaA, lastSeenAt: _time.Now.AddMinutes(-5));

            var result = await Handler().Handle(Pedido(), CancellationToken.None);

            Assert.True(result.Success);
            Assert.False(((PrintJobResponse)result.Response).StationOnline);
            Assert.Single(_jobs.jobs);
        }

        [Fact(DisplayName = "Comanda de outra empresa responde como não encontrada e não cria job")]
        public async Task Comanda_de_outra_empresa()
        {
            _jobs.commands.Add(Comanda(5, EmpresaB));
            Estacao(_stations, EmpresaA);

            var result = await Handler().Handle(Pedido(commandId: 5, companyId: EmpresaA), CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("Comanda não encontrada", result.ErrorMessage);
            Assert.Empty(_jobs.jobs);
        }

        [Fact(DisplayName = "Comanda sem itens não imprime")]
        public async Task Comanda_sem_itens()
        {
            _jobs.commands.Add(Comanda(5, EmpresaA, comItens: false));
            Estacao(_stations, EmpresaA);

            var result = await Handler().Handle(Pedido(), CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("Adicione itens para imprimir", result.ErrorMessage);
            Assert.Empty(_jobs.jobs);
        }

        [Fact(DisplayName = "Empresa sem impressora configurada não imprime")]
        public async Task Sem_estacao()
        {
            _jobs.commands.Add(Comanda(5, EmpresaA));
            Estacao(_stations, EmpresaB);

            var result = await Handler().Handle(Pedido(), CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("Nenhuma impressora configurada", result.ErrorMessage);
        }

        [Fact(DisplayName = "Comanda inválida (id zero) falha na validação")]
        public async Task Validacao_comanda_obrigatoria()
        {
            var result = await Handler().Handle(Pedido(commandId: 0), CancellationToken.None);

            Assert.False(result.Success);
            Assert.Contains(result.Errors, e => e.ErrorMessage == "Informe a comanda");
        }

        [Fact(DisplayName = "Duplo toque em menos de 3 s devolve o mesmo job")]
        public async Task Duplo_toque()
        {
            _jobs.commands.Add(Comanda(5, EmpresaA));
            Estacao(_stations, EmpresaA);
            var handler = Handler();

            var primeiro = await handler.Handle(Pedido(), CancellationToken.None);
            _time.Advance(TimeSpan.FromSeconds(2));
            var segundo = await handler.Handle(Pedido(), CancellationToken.None);

            Assert.Single(_jobs.jobs);
            Assert.Equal(((PrintJobResponse)primeiro.Response).PrintJobId, ((PrintJobResponse)segundo.Response).PrintJobId);
        }

        [Fact(DisplayName = "Depois de 3 s, ou por outro funcionário, cria job novo")]
        public async Task Depois_da_janela_cria_novo()
        {
            _jobs.commands.Add(Comanda(5, EmpresaA));
            Estacao(_stations, EmpresaA);
            var handler = Handler();

            await handler.Handle(Pedido(), CancellationToken.None);
            await handler.Handle(Pedido(employeeId: 11), CancellationToken.None);
            _time.Advance(TimeSpan.FromSeconds(4));
            await handler.Handle(Pedido(), CancellationToken.None);

            Assert.Equal(3, _jobs.jobs.Count);
        }

        [Fact(DisplayName = "Comanda já impressa gera 2ª via")]
        public async Task Segunda_via()
        {
            _jobs.commands.Add(Comanda(5, EmpresaA));
            Estacao(_stations, EmpresaA);
            await _jobs.AddAsync(new PrintJobEntity
            {
                CompanyId = EmpresaA, CommandId = 5, Status = PrintJobStatusEnum.Impresso,
                CreatedAt = _time.Now.AddMinutes(-10)
            });

            var result = await Handler().Handle(Pedido(), CancellationToken.None);

            Assert.Equal(2, ((PrintJobResponse)result.Response).Copy);
        }

        [Theory(DisplayName = "No papel sai só o nome do produto, sem a categoria")]
        [InlineData("SALGADOS | COXINHA C CATUPIRY", "COXINHA C CATUPIRY")]
        [InlineData("BEBIDAS|Cerveja Long Neck", "Cerveja Long Neck")]
        [InlineData("Refrigerante Lata", "Refrigerante Lata")]
        [InlineData("SEM NOME |  ", "SEM NOME |")]
        public void Nome_do_produto_sem_categoria(string itemName, string esperado)
        {
            Assert.Equal(esperado, Domain.Services.Printing.ReceiptDocumentFactory.ProductName(itemName));
        }

        [Fact(DisplayName = "Cabeçalho usa a data de abertura da comanda")]
        public void Cabecalho_usa_data_de_abertura()
        {
            var comanda = Comanda(5, EmpresaA);
            comanda.InitialDate = new DateTime(2026, 7, 16, 22, 22, 0);
            comanda.Items[0].Name = "SALGADOS | Porção de Calabresa";

            var texto = new ReceiptRenderer().RenderText(
                Domain.Services.Printing.ReceiptDocumentFactory.Create(comanda, 1), new ReceiptOptions());

            Assert.Contains("Aberta em: 16/07/2026 22:22", texto);
            Assert.DoesNotContain("SALGADOS", texto);
            Assert.Contains("1x Porção de Calabresa", texto);
        }

        [Fact(DisplayName = "Via impressa de outra empresa não conta para a 2ª via")]
        public async Task Via_de_outra_empresa_nao_conta()
        {
            _jobs.commands.Add(Comanda(5, EmpresaA));
            Estacao(_stations, EmpresaA);
            await _jobs.AddAsync(new PrintJobEntity { CompanyId = EmpresaB, CommandId = 5, Status = PrintJobStatusEnum.Impresso });

            var result = await Handler().Handle(Pedido(), CancellationToken.None);

            Assert.Equal(1, ((PrintJobResponse)result.Response).Copy);
        }
    }
}
