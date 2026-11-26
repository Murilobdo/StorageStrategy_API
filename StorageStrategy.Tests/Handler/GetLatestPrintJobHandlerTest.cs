using StorageStrategy.Domain.Commands.Print;
using StorageStrategy.Domain.Handlers.Print;
using StorageStrategy.Models;
using StorageStrategy.Tests.FakeRepository;
using Xunit;
using static StorageStrategy.Tests.Handler.PrintFixtures;

namespace StorageStrategy.Tests.Handler
{
    public class GetLatestPrintJobHandlerTest
    {
        private readonly FakePrintJobRepository _jobs = new();
        private readonly FakePrintStationRepository _stations = new();
        private readonly FakeTimeProvider _time = new();

        private GetLatestPrintJobHandler Handler() => new(_jobs, _stations, _time);

        [Fact(DisplayName = "Devolve o job mais recente da comanda com o status da estação")]
        public async Task Devolve_mais_recente()
        {
            Estacao(_stations, EmpresaA, lastSeenAt: _time.Now);
            await _jobs.AddAsync(new PrintJobEntity { CompanyId = EmpresaA, CommandId = 5, Status = PrintJobStatusEnum.Impresso, CreatedAt = _time.Now.AddMinutes(-5) });
            await _jobs.AddAsync(new PrintJobEntity { CompanyId = EmpresaA, CommandId = 5, Status = PrintJobStatusEnum.Aguardando, CreatedAt = _time.Now });

            var result = await Handler().Handle(new GetLatestPrintJobQuery(5, EmpresaA), default);

            var response = Assert.IsType<PrintJobResponse>(result.Response);
            Assert.Equal(PrintJobStatusEnum.Aguardando, response.Status);
            Assert.True(response.StationOnline);
        }

        [Fact(DisplayName = "Comanda nunca impressa devolve vazio")]
        public async Task Nunca_impressa()
        {
            var result = await Handler().Handle(new GetLatestPrintJobQuery(5, EmpresaA), default);

            Assert.True(result.Success);
            Assert.Null(result.Response);
        }

        [Fact(DisplayName = "Comanda de outra empresa devolve vazio")]
        public async Task Outra_empresa()
        {
            await _jobs.AddAsync(new PrintJobEntity { CompanyId = EmpresaB, CommandId = 5, Status = PrintJobStatusEnum.Impresso, CreatedAt = _time.Now });

            var result = await Handler().Handle(new GetLatestPrintJobQuery(5, EmpresaA), default);

            Assert.Null(result.Response);
        }
    }
}
