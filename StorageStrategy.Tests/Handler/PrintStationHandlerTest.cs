using Microsoft.Extensions.Logging.Abstractions;
using StorageStrategy.Domain.Commands.Print;
using StorageStrategy.Domain.Handlers.Print;
using StorageStrategy.Domain.Services.Printing;
using StorageStrategy.Models;
using StorageStrategy.Tests.FakeRepository;
using Xunit;
using static StorageStrategy.Tests.Handler.PrintFixtures;

namespace StorageStrategy.Tests.Handler
{
    /// <summary>US4: gerente configura a impressora (estação) da empresa.</summary>
    public class PrintStationHandlerTest
    {
        private readonly FakePrintStationRepository _stations = new();
        private readonly FakeTimeProvider _time = new();

        private SavePrintStationHandler Save() => new(_stations, _time, NullLogger<SavePrintStationHandler>.Instance);
        private RegeneratePrintStationKeyHandler Regenerate() => new(_stations, _time, NullLogger<RegeneratePrintStationKeyHandler>.Instance);
        private GetPrintStationHandler Get() => new(_stations, _time);

        private static SavePrintStationCommand Cadastro(int companyId = EmpresaA, string nome = "Caixa principal",
            int papel = 58, PrintEncodingEnum encoding = PrintEncodingEnum.Cp860) =>
            new() { CompanyId = companyId, EmployeeId = 1, Name = nome, PaperWidthMm = papel, Encoding = encoding };

        [Fact(DisplayName = "Primeiro cadastro devolve a chave uma vez e grava só o hash")]
        public async Task Cadastro_devolve_chave()
        {
            var result = await Save().Handle(Cadastro(), default);

            var response = Assert.IsType<PrintStationResponse>(result.Response);
            Assert.True(result.Success);
            Assert.StartsWith($"ps_{response.PrintStationId}_", response.StationKey);

            var salvo = Assert.Single(_stations.stations);
            Assert.True(salvo.IsActive);
            Assert.Equal(32, salvo.KeyHash.Length);
            Assert.True(PrintStationKey.TryParse(response.StationKey, out _, out var segredo));
            Assert.True(PrintStationKey.Verify(segredo, salvo.KeyHash));
            Assert.Equal(response.StationKey![^4..], salvo.KeyLastChars);
        }

        [Fact(DisplayName = "Atualizar não muda a chave nem a devolve")]
        public async Task Atualizar_mantem_chave()
        {
            await Save().Handle(Cadastro(), default);
            var hashAntes = _stations.stations[0].KeyHash.ToArray();

            var result = await Save().Handle(Cadastro(nome: "Balcão", encoding: PrintEncodingEnum.Cp850), default);

            var response = (PrintStationResponse)result.Response;
            Assert.Null(response.StationKey);
            Assert.Equal("Balcão", response.Name);
            Assert.Equal(PrintEncodingEnum.Cp850, response.Encoding);
            Assert.Single(_stations.stations);
            Assert.Equal(hashAntes, _stations.stations[0].KeyHash);
        }

        [Theory(DisplayName = "Validação do cadastro")]
        [InlineData("A", 58, PrintEncodingEnum.Cp860, "O nome da impressora deve ter entre 2 e 60 caracteres")]
        [InlineData("   ", 58, PrintEncodingEnum.Cp860, "O nome da impressora deve ter entre 2 e 60 caracteres")]
        [InlineData("Caixa", 80, PrintEncodingEnum.Cp860, "Só o papel de 58mm é suportado")]
        [InlineData("Caixa", 58, (PrintEncodingEnum)99, "Codificação inválida")]
        public async Task Validacao(string nome, int papel, PrintEncodingEnum encoding, string erro)
        {
            var result = await Save().Handle(Cadastro(nome: nome, papel: papel, encoding: encoding), default);

            Assert.False(result.Success);
            Assert.Contains(result.Errors, e => e.ErrorMessage == erro);
            Assert.Empty(_stations.stations);
        }

        [Fact(DisplayName = "Nome com 61 caracteres é recusado")]
        public async Task Nome_longo()
        {
            var result = await Save().Handle(Cadastro(nome: new string('a', 61)), default);

            Assert.False(result.Success);
        }

        [Fact(DisplayName = "Regenerar invalida a chave anterior")]
        public async Task Regenerar_invalida_anterior()
        {
            var criada = (PrintStationResponse)(await Save().Handle(Cadastro(), default)).Response;
            PrintStationKey.TryParse(criada.StationKey, out _, out var segredoAntigo);

            var result = await Regenerate().Handle(new RegeneratePrintStationKeyCommand(EmpresaA, 1), default);

            var nova = (PrintStationResponse)result.Response;
            PrintStationKey.TryParse(nova.StationKey, out _, out var segredoNovo);
            var station = _stations.stations[0];
            Assert.False(PrintStationKey.Verify(segredoAntigo, station.KeyHash));
            Assert.True(PrintStationKey.Verify(segredoNovo, station.KeyHash));
        }

        [Fact(DisplayName = "Regenerar sem estação da empresa falha")]
        public async Task Regenerar_sem_estacao()
        {
            Estacao(_stations, EmpresaB);

            var result = await Regenerate().Handle(new RegeneratePrintStationKeyCommand(EmpresaA, 1), default);

            Assert.False(result.Success);
            Assert.Equal("Nenhuma impressora configurada", result.ErrorMessage);
        }

        [Fact(DisplayName = "Consulta nunca devolve a chave nem o hash e mostra se está conectada")]
        public async Task Consulta_sem_segredos()
        {
            await Save().Handle(Cadastro(), default);
            _stations.stations[0].LastSeenAt = _time.Now.AddSeconds(-20);

            var response = (PrintStationResponse)(await Get().Handle(new GetPrintStationQuery(EmpresaA), default)).Response;

            Assert.True(response.IsConfigured);
            Assert.Null(response.StationKey);
            Assert.True(response.IsOnline);
            Assert.Equal(20, response.SecondsSinceLastSeen);
            Assert.DoesNotContain(typeof(PrintStationResponse).GetProperties(), p => p.Name.Contains("Hash"));
        }

        [Fact(DisplayName = "Consulta de empresa sem estação devolve não configurada (não vê a de outra empresa)")]
        public async Task Consulta_outra_empresa()
        {
            Estacao(_stations, EmpresaB);

            var response = (PrintStationResponse)(await Get().Handle(new GetPrintStationQuery(EmpresaA), default)).Response;

            Assert.False(response.IsConfigured);
        }
    }
}
