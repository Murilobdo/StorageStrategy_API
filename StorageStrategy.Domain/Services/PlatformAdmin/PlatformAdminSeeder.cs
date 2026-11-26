using Isopoh.Cryptography.Argon2;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;
using StorageStrategy.Utils.Services;

namespace StorageStrategy.Domain.Services.PlatformAdmin
{
    /// <summary>
    /// Garante a empresa e o admin da plataforma na inicialização (C-9 / feature 003).
    /// - Nenhuma senha no código: o admin só é criado se AppSettings:PlatformAdmin:InitialPassword
    ///   estiver configurada fora do código (variável de ambiente / configuração da VPS).
    /// - Admin já existente nunca é alterado.
    /// - A senha nunca é logada.
    /// </summary>
    public class PlatformAdminSeeder
    {
        public const string PlatformCompanyName = "Admin Company";
        public const string AdminEmail = "murilobdo@admin.com";
        public const string AdminName = "Murilo Bernardes (Admin)";

        private readonly ICompanyRepository _companies;
        private readonly IEmployeeRepository _employees;
        private readonly PlatformAdminSettings _settings;
        private readonly ILogger<PlatformAdminSeeder> _logger;

        public PlatformAdminSeeder(
            ICompanyRepository companies,
            IEmployeeRepository employees,
            IOptions<AppSettings> appSettings,
            ILogger<PlatformAdminSeeder> logger)
        {
            _companies = companies;
            _employees = employees;
            _settings = appSettings.Value.PlatformAdmin ?? new PlatformAdminSettings();
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            var company = (await _companies.GetAllAsync(c => c.Name == PlatformCompanyName)).FirstOrDefault();

            if (company == null)
            {
                company = new CompanyEntity().InitAdmin(
                    companyId: _settings.CompanyId,
                    name: PlatformCompanyName,
                    description: "Admin",
                    createAt: DateTime.Now,
                    validate: DateTime.Now.AddYears(10));

                await _companies.AddAsync(company);
                await _companies.SaveAsync();
                _logger.LogInformation("Empresa da plataforma criada (CompanyId {CompanyId}).", company.CompanyId);
            }

            if (await _employees.FindByEmail(AdminEmail) != null)
                return; // Admin já existe: a senha nunca é alterada pela inicialização.

            if (string.IsNullOrWhiteSpace(_settings.InitialPassword))
            {
                _logger.LogWarning(
                    "Admin da plataforma ausente e AppSettings:PlatformAdmin:InitialPassword não configurada; usuário não criado.");
                return;
            }

            await _employees.AddAsync(new EmployeeEntity
            {
                Name = AdminName,
                Email = AdminEmail,
                PasswordHash = Argon2.Hash(_settings.InitialPassword),
                JobRole = EmployeeRole.Admin,
                CompanyId = company.CompanyId,
                Comission = 0,
                IsActive = true
            });
            await _employees.SaveAsync();

            _logger.LogInformation("Admin da plataforma criado na empresa {CompanyId} com a senha inicial configurada.", company.CompanyId);
        }
    }
}
