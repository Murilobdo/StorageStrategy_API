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
    /// <summary>US3: cancelar, expirar, reentregar e limpar a fila.</summary>
    public class PrintMaintenanceTest
    {
        private readonly FakePrintJobRepository _jobs = new();
        private readonly FakeTimeProvider _time = new();

        private PrintJobEntity Job(int companyId, PrintJobStatusEnum status, TimeSpan idade, TimeSpan? reservadoHa = null)
        {
            var job = new PrintJobEntity
            {
                CompanyId = companyId,
                CommandId = 5,
                Status = status,
                CreatedAt = _time.Now - idade,
                ClaimedAt = reservadoHa is null ? null : _time.Now - reservadoHa,
                ClaimId = reservadoHa is null ? null : Guid.NewGuid(),
                PrintStationId = reservadoHa is null ? null : 1
            };
            _jobs.AddAsync(job).Wait();
            return job;
        }

        private CancelPrintJobHandler Cancel() => new(_jobs, _time, NullLogger<CancelPrintJobHandler>.Instance);
        private PrintJobMaintenance Maintenance() => new(_jobs, _time, NullLogger<PrintJobMaintenance>.Instance);

        [Fact(DisplayName = "Cancela job Aguardando")]
        public async Task Cancela_aguardando()
        {
            var job = Job(EmpresaA, PrintJobStatusEnum.Aguardando, TimeSpan.FromMinutes(1));

            var result = await Cancel().Handle(new CancelPrintJobCommand(job.PrintJobId, EmpresaA), default);

            Assert.True(result.Success);
            Assert.Equal(PrintJobStatusEnum.Cancelado, job.Status);
            Assert.NotNull(job.CompletedAt);
        }

        [Theory(DisplayName = "Só é possível cancelar pedidos aguardando")]
        [InlineData(PrintJobStatusEnum.Imprimindo)]
        [InlineData(PrintJobStatusEnum.Impresso)]
        [InlineData(PrintJobStatusEnum.Falhou)]
        public async Task Nao_cancela_outros_status(PrintJobStatusEnum status)
        {
            var job = Job(EmpresaA, status, TimeSpan.FromMinutes(1));

            var result = await Cancel().Handle(new CancelPrintJobCommand(job.PrintJobId, EmpresaA), default);

            Assert.False(result.Success);
            Assert.Equal("Só é possível cancelar pedidos aguardando", result.ErrorMessage);
            Assert.Equal(status, job.Status);
        }

        [Fact(DisplayName = "Não cancela job de outra empresa")]
        public async Task Nao_cancela_outra_empresa()
        {
            var job = Job(EmpresaB, PrintJobStatusEnum.Aguardando, TimeSpan.FromMinutes(1));

            var result = await Cancel().Handle(new CancelPrintJobCommand(job.PrintJobId, EmpresaA), default);

            Assert.False(result.Success);
            Assert.Equal("Impressão não encontrada", result.ErrorMessage);
            Assert.Equal(PrintJobStatusEnum.Aguardando, job.Status);
        }

        [Fact(DisplayName = "Aguardando há mais de 30 min expira; mais novo continua")]
        public async Task Expira_apos_30_min()
        {
            var velho = Job(EmpresaA, PrintJobStatusEnum.Aguardando, TimeSpan.FromMinutes(31));
            var novo = Job(EmpresaA, PrintJobStatusEnum.Aguardando, TimeSpan.FromMinutes(29));

            await Maintenance().RunAsync(purgeOld: false);

            Assert.Equal(PrintJobStatusEnum.Expirado, velho.Status);
            Assert.NotNull(velho.CompletedAt);
            Assert.Equal(PrintJobStatusEnum.Aguardando, novo.Status);
        }

        [Fact(DisplayName = "Imprimindo sem resultado há mais de 2 min volta para a fila sem reserva")]
        public async Task Reserva_travada_volta_para_fila()
        {
            var travado = Job(EmpresaA, PrintJobStatusEnum.Imprimindo, TimeSpan.FromMinutes(5), reservadoHa: TimeSpan.FromMinutes(3));
            var recente = Job(EmpresaA, PrintJobStatusEnum.Imprimindo, TimeSpan.FromMinutes(5), reservadoHa: TimeSpan.FromMinutes(1));

            await Maintenance().RunAsync(purgeOld: false);

            Assert.Equal(PrintJobStatusEnum.Aguardando, travado.Status);
            Assert.Null(travado.ClaimId);
            Assert.Null(travado.ClaimedAt);
            Assert.Null(travado.PrintStationId);
            Assert.Equal(PrintJobStatusEnum.Imprimindo, recente.Status);
        }

        [Fact(DisplayName = "Limpeza apaga jobs com mais de 30 dias")]
        public async Task Limpa_historico_antigo()
        {
            Job(EmpresaA, PrintJobStatusEnum.Impresso, TimeSpan.FromDays(31));
            var recente = Job(EmpresaA, PrintJobStatusEnum.Impresso, TimeSpan.FromDays(29));

            await Maintenance().RunAsync(purgeOld: true);

            Assert.Equal(recente, Assert.Single(_jobs.jobs));
        }
    }
}
