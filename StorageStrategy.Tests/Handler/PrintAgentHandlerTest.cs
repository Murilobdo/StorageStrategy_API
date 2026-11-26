using Microsoft.Extensions.Logging.Abstractions;
using StorageStrategy.Domain.Commands.Print;
using StorageStrategy.Domain.Handlers.Print;
using StorageStrategy.Models;
using StorageStrategy.Tests.FakeRepository;
using Xunit;
using static StorageStrategy.Tests.Handler.PrintFixtures;

namespace StorageStrategy.Tests.Handler
{
    /// <summary>US1: agente reserva o job e informa o resultado (sem papel duplicado, sem vazar entre empresas).</summary>
    public class PrintAgentHandlerTest
    {
        private readonly FakePrintJobRepository _jobs = new();
        private readonly FakePrintStationRepository _stations = new();
        private readonly FakeTimeProvider _time = new();

        private ClaimPrintJobHandler Claim() => new(_jobs, _stations, _time, NullLogger<ClaimPrintJobHandler>.Instance);
        private ReportPrintResultHandler Report() => new(_jobs, _time, NullLogger<ReportPrintResultHandler>.Instance);

        private PrintJobEntity Job(int companyId, int minutesAgo = 1, PrintJobStatusEnum status = PrintJobStatusEnum.Aguardando)
        {
            var job = new PrintJobEntity
            {
                CompanyId = companyId,
                CommandId = 5,
                Payload = new byte[] { 0x1B, 0x40, 0x41 },
                Status = status,
                CreatedAt = _time.Now.AddMinutes(-minutesAgo)
            };
            _jobs.AddAsync(job).Wait();
            return job;
        }

        [Fact(DisplayName = "Claim entrega o job mais antigo da própria empresa e marca Imprimindo")]
        public async Task Claim_entrega_job_mais_antigo()
        {
            var estacao = Estacao(_stations, EmpresaA);
            Job(EmpresaB, minutesAgo: 10);
            var antigo = Job(EmpresaA, minutesAgo: 5);
            Job(EmpresaA, minutesAgo: 1);

            var result = await Claim().Handle(new ClaimPrintJobCommand(EmpresaA, estacao.PrintStationId), CancellationToken.None);

            var claimed = Assert.IsType<ClaimedPrintJobResponse>(result.Response);
            Assert.Equal(antigo.PrintJobId, claimed.PrintJobId);
            Assert.Equal(Convert.ToBase64String(antigo.Payload), claimed.PayloadBase64);
            Assert.Equal(PrintJobStatusEnum.Imprimindo, antigo.Status);
            Assert.Equal(claimed.ClaimId, antigo.ClaimId);
            Assert.Equal(estacao.PrintStationId, antigo.PrintStationId);
        }

        [Fact(DisplayName = "Claim nunca entrega job de outra empresa")]
        public async Task Claim_nao_vaza_entre_empresas()
        {
            var estacao = Estacao(_stations, EmpresaA);
            var deB = Job(EmpresaB);

            var result = await Claim().Handle(new ClaimPrintJobCommand(EmpresaA, estacao.PrintStationId), CancellationToken.None);

            Assert.True(result.Success);
            Assert.Null(result.Response);
            Assert.Equal(PrintJobStatusEnum.Aguardando, deB.Status);
        }

        [Fact(DisplayName = "Ping devolve estação e empresa e atualiza o último contato")]
        public async Task Ping_devolve_empresa()
        {
            var estacao = Estacao(_stations, EmpresaA);
            var empresas = new FakeCompanyRepository();
            await empresas.AddAsync(new CompanyEntity { CompanyId = EmpresaA, Name = "Bar do Zé" });

            var result = await new PingPrintStationHandler(_stations, empresas, _time)
                .Handle(new PingPrintStationQuery(EmpresaA, estacao.PrintStationId), default);

            var ping = Assert.IsType<PrintStationPingResponse>(result.Response);
            Assert.Equal("Bar do Zé", ping.CompanyName);
            Assert.Equal(_time.Now, estacao.LastSeenAt);
            Assert.Empty(_jobs.jobs);
        }

        [Fact(DisplayName = "Ping com estação de outra empresa é recusado")]
        public async Task Ping_outra_empresa()
        {
            var estacaoB = Estacao(_stations, EmpresaB);

            var result = await new PingPrintStationHandler(_stations, new FakeCompanyRepository(), _time)
                .Handle(new PingPrintStationQuery(EmpresaA, estacaoB.PrintStationId), default);

            Assert.False(result.Success);
        }

        [Fact(DisplayName = "Claim com estação de outra empresa é recusado")]
        public async Task Claim_estacao_de_outra_empresa()
        {
            var estacaoB = Estacao(_stations, EmpresaB);
            Job(EmpresaA);

            var result = await Claim().Handle(new ClaimPrintJobCommand(EmpresaA, estacaoB.PrintStationId), CancellationToken.None);

            Assert.False(result.Success);
        }

        [Fact(DisplayName = "Dois claims seguidos nunca recebem o mesmo job")]
        public async Task Dois_claims_jobs_diferentes()
        {
            var estacao = Estacao(_stations, EmpresaA);
            Job(EmpresaA, 2);
            Job(EmpresaA, 1);

            var r1 = (ClaimedPrintJobResponse)(await Claim().Handle(new ClaimPrintJobCommand(EmpresaA, estacao.PrintStationId), default)).Response;
            var r2 = (ClaimedPrintJobResponse)(await Claim().Handle(new ClaimPrintJobCommand(EmpresaA, estacao.PrintStationId), default)).Response;
            var r3 = (await Claim().Handle(new ClaimPrintJobCommand(EmpresaA, estacao.PrintStationId), default)).Response;

            Assert.NotEqual(r1.PrintJobId, r2.PrintJobId);
            Assert.Null(r3);
        }

        [Fact(DisplayName = "Claim atualiza o último contato no máximo a cada 15 s")]
        public async Task Heartbeat_throttle()
        {
            var estacao = Estacao(_stations, EmpresaA);
            var cmd = new ClaimPrintJobCommand(EmpresaA, estacao.PrintStationId);

            await Claim().Handle(cmd, default);
            var primeiro = estacao.LastSeenAt;
            var salvamentos = _stations.SaveCount;

            _time.Advance(TimeSpan.FromSeconds(5));
            await Claim().Handle(cmd, default);
            Assert.Equal(primeiro, estacao.LastSeenAt);
            Assert.Equal(salvamentos, _stations.SaveCount);

            _time.Advance(TimeSpan.FromSeconds(11));
            await Claim().Handle(cmd, default);
            Assert.Equal(_time.Now, estacao.LastSeenAt);
        }

        private async Task<(PrintJobEntity Job, Guid ClaimId)> Reservado()
        {
            var estacao = Estacao(_stations, EmpresaA);
            var job = Job(EmpresaA);
            var claimed = (ClaimedPrintJobResponse)(await Claim().Handle(new ClaimPrintJobCommand(EmpresaA, estacao.PrintStationId), default)).Response;
            return (job, claimed.ClaimId);
        }

        private static ReportPrintResultCommand Resultado(PrintJobEntity job, Guid claimId, bool sucesso, string? motivo = null, int companyId = EmpresaA) =>
            new() { PrintJobId = job.PrintJobId, CompanyId = companyId, ClaimId = claimId, Success = sucesso, FailureReason = motivo };

        [Fact(DisplayName = "Resultado ok marca Impresso")]
        public async Task Resultado_ok()
        {
            var (job, claimId) = await Reservado();

            var result = await Report().Handle(Resultado(job, claimId, true), default);

            Assert.Equal(PrintResultOutcome.Accepted, result.Response);
            Assert.Equal(PrintJobStatusEnum.Impresso, job.Status);
            Assert.NotNull(job.CompletedAt);
        }

        [Fact(DisplayName = "Resultado com erro marca Falhou com motivo truncado em 200")]
        public async Task Resultado_falha()
        {
            var (job, claimId) = await Reservado();

            await Report().Handle(Resultado(job, claimId, false, new string('x', 300)), default);

            Assert.Equal(PrintJobStatusEnum.Falhou, job.Status);
            Assert.Equal(200, job.FailureReason!.Length);
        }

        [Fact(DisplayName = "ClaimId diferente é conflito e não muda o job")]
        public async Task ClaimId_errado()
        {
            var (job, _) = await Reservado();

            var result = await Report().Handle(Resultado(job, Guid.NewGuid(), true), default);

            Assert.Equal(PrintResultOutcome.Conflict, result.Response);
            Assert.Equal(PrintJobStatusEnum.Imprimindo, job.Status);
        }

        [Fact(DisplayName = "Mesmo resultado reenviado é idempotente")]
        public async Task Resultado_repetido()
        {
            var (job, claimId) = await Reservado();
            await Report().Handle(Resultado(job, claimId, true), default);
            var concluido = job.CompletedAt;
            _time.Advance(TimeSpan.FromSeconds(30));

            var result = await Report().Handle(Resultado(job, claimId, false, "sem papel"), default);

            Assert.Equal(PrintResultOutcome.Accepted, result.Response);
            Assert.Equal(PrintJobStatusEnum.Impresso, job.Status);
            Assert.Equal(concluido, job.CompletedAt);
        }

        [Fact(DisplayName = "Resultado para job de outra empresa responde não encontrado")]
        public async Task Resultado_outra_empresa()
        {
            var (job, claimId) = await Reservado();

            var result = await Report().Handle(Resultado(job, claimId, true, companyId: EmpresaB), default);

            Assert.Equal(PrintResultOutcome.NotFound, result.Response);
            Assert.Equal(PrintJobStatusEnum.Imprimindo, job.Status);
        }
    }
}
