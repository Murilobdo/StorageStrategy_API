using Isopoh.Cryptography.Argon2;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StorageStrategy.Domain.Services.PlatformAdmin;
using StorageStrategy.Models;
using StorageStrategy.Tests.FakeRepository;
using StorageStrategy.Utils.Services;
using Xunit;

namespace StorageStrategy.Tests.Services
{
    /// <summary>US4 (C-9): nenhuma senha fixa; admin só é criado com senha configurada fora do código.</summary>
    public class PlatformAdminSeederTest
    {
        // Valor fictício, só de teste.
        private const string SenhaInicialDeTeste = "SenhaInicial#2026";

        private readonly FakeCompanyRepository _companies = new();
        private readonly FakeEmployeeRepository _employees = new();
        private readonly ListLogger<PlatformAdminSeeder> _logger = new();

        public PlatformAdminSeederTest()
        {
            _employees.employees.Clear();
        }

        private PlatformAdminSeeder Seeder(string initialPassword, int companyId = 1) =>
            new(_companies, _employees,
                Options.Create(new AppSettings
                {
                    PlatformAdmin = new PlatformAdminSettings { CompanyId = companyId, InitialPassword = initialPassword }
                }),
                _logger);

        private EmployeeEntity? Admin() =>
            _employees.employees.FirstOrDefault(e => e.Email == PlatformAdminSeeder.AdminEmail);

        [Fact(DisplayName = "Sem senha inicial: cria a empresa da plataforma, não cria o admin e avisa")]
        public async Task Sem_senha_inicial_nao_cria_admin()
        {
            await Seeder(initialPassword: string.Empty).SeedAsync();

            var company = Assert.Single(_companies.companies);
            Assert.Equal(PlatformAdminSeeder.PlatformCompanyName, company.Name);
            Assert.Equal(1, company.CompanyId);
            Assert.Null(Admin());
            Assert.Contains(_logger.Entries, e => e.Level == LogLevel.Warning);
        }

        [Fact(DisplayName = "Com senha inicial: cria o admin da plataforma com essa senha")]
        public async Task Com_senha_inicial_cria_admin()
        {
            await Seeder(SenhaInicialDeTeste, companyId: 7).SeedAsync();

            var admin = Admin();
            Assert.NotNull(admin);
            Assert.Equal(EmployeeRole.Admin, admin!.JobRole);
            Assert.Equal(7, admin.CompanyId);
            Assert.True(admin.IsActive);
            Assert.True(Argon2.Verify(admin.PasswordHash, SenhaInicialDeTeste));
        }

        [Fact(DisplayName = "Admin existente: a senha não é alterada mesmo com senha inicial configurada")]
        public async Task Admin_existente_nao_e_alterado()
        {
            _companies.companies.Add(new CompanyEntity().InitAdmin(1, PlatformAdminSeeder.PlatformCompanyName, "Admin", DateTime.Now, DateTime.Now.AddYears(1)));
            string hashExistente = Argon2.Hash("SenhaQueJaExiste1");
            _employees.employees.Add(new EmployeeEntity
            {
                EmployeeId = 1, CompanyId = 1, Email = PlatformAdminSeeder.AdminEmail, Name = "Admin",
                JobRole = EmployeeRole.Admin, IsActive = true, PasswordHash = hashExistente
            });

            await Seeder(SenhaInicialDeTeste).SeedAsync();

            Assert.Single(_companies.companies);
            Assert.Single(_employees.employees);
            Assert.Equal(hashExistente, Admin()!.PasswordHash);
        }

        [Fact(DisplayName = "Nenhum log contém a senha inicial")]
        public async Task Logs_nao_contem_senha()
        {
            await Seeder(SenhaInicialDeTeste).SeedAsync();
            await Seeder(SenhaInicialDeTeste).SeedAsync();
            await Seeder(string.Empty).SeedAsync();

            Assert.DoesNotContain(_logger.Entries, e => e.Message.Contains(SenhaInicialDeTeste));
        }

        private sealed class ListLogger<T> : ILogger<T>
        {
            public List<(LogLevel Level, string Message)> Entries { get; } = new();

            public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
            public bool IsEnabled(LogLevel logLevel) => true;

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
            {
                Entries.Add((logLevel, formatter(state, exception)));
            }
        }
    }
}
