using StorageStrategy.Models;
using StorageStrategy.Tests.FakeRepository;

namespace StorageStrategy.Tests.Handler
{
    /// <summary>Relógio controlável para os testes de impressão (fuso local = UTC para resultados estáveis).</summary>
    public class FakeTimeProvider : TimeProvider
    {
        public DateTimeOffset UtcNow { get; set; } = new(2026, 11, 26, 23, 0, 0, TimeSpan.Zero);

        public override DateTimeOffset GetUtcNow() => UtcNow;
        public override TimeZoneInfo LocalTimeZone => TimeZoneInfo.Utc;

        public DateTime Now => UtcNow.UtcDateTime;
        public void Advance(TimeSpan delta) => UtcNow = UtcNow.Add(delta);
    }

    public static class PrintFixtures
    {
        public const int EmpresaA = 1;
        public const int EmpresaB = 2;

        public static CommandEntity Comanda(int commandId, int companyId, bool comItens = true) => new()
        {
            CommandId = commandId,
            CompanyId = companyId,
            Code = $"C{commandId}",
            Name = "Mesa 4",
            EmployeeId = 10,
            Employee = new EmployeeEntity { EmployeeId = 10, Name = "Maria", CompanyId = companyId },
            Company = new CompanyEntity { CompanyId = companyId, Name = "Bar do Zé" },
            Discount = 0,
            Increase = 0,
            Items = comItens
                ? new List<CommandItemEntity>
                {
                    new(1, commandId, 1, "Porção de Calabresa", 20m, 49.90m, 1, 0m),
                    new(2, commandId, 2, "Cerveja Long Neck", 5m, 12m, 3, 0m),
                }
                : new List<CommandItemEntity>()
        };

        public static PrintStationEntity Estacao(FakePrintStationRepository repo, int companyId, DateTime? lastSeenAt = null)
        {
            var station = new PrintStationEntity
            {
                CompanyId = companyId,
                Name = "Caixa",
                IsActive = true,
                KeyHash = new byte[32],
                LastSeenAt = lastSeenAt
            };
            repo.AddAsync(station).Wait();
            return station;
        }
    }
}
