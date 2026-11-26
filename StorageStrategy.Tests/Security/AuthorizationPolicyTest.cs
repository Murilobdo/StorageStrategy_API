using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StorageStrategy.API.Configuration;
using Xunit;

namespace StorageStrategy.Tests.Security
{
    public class AuthorizationPolicyTest
    {
        private static IServiceProvider BuildProvider(string? platformCompanyId = "1")
        {
            var values = new Dictionary<string, string?>();
            if (platformCompanyId != null)
                values["AppSettings:PlatformAdmin:CompanyId"] = platformCompanyId;

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(values)
                .Build();

            var services = new ServiceCollection();
            services.AddLogging();
            services.AddApiAuthorization(configuration);
            return services.BuildServiceProvider();
        }

        private static ClaimsPrincipal User(string role, int companyId) =>
            new(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Role, role),
                new Claim("CompanyId", companyId.ToString()),
                new Claim("EmployeeId", "10")
            }, authenticationType: "Bearer"));

        private static readonly ClaimsPrincipal Anonymous = new(new ClaimsIdentity());

        private static async Task<bool> PlatformAdminAllows(IServiceProvider provider, ClaimsPrincipal user)
        {
            var authorization = provider.GetRequiredService<IAuthorizationService>();
            var result = await authorization.AuthorizeAsync(user, null, AuthorizationSetup.PlatformAdminPolicy);
            return result.Succeeded;
        }

        [Fact(DisplayName = "FallbackPolicy exige usuário autenticado")]
        public async Task Fallback_policy_exige_usuario_autenticado()
        {
            var provider = BuildProvider();
            var policyProvider = provider.GetRequiredService<IAuthorizationPolicyProvider>();
            var authorization = provider.GetRequiredService<IAuthorizationService>();

            var fallback = await policyProvider.GetFallbackPolicyAsync();

            Assert.NotNull(fallback);
            Assert.False((await authorization.AuthorizeAsync(Anonymous, null, fallback!)).Succeeded);
            Assert.True((await authorization.AuthorizeAsync(User("Employee", 3), null, fallback!)).Succeeded);
        }

        [Fact(DisplayName = "PlatformAdmin permite Admin da empresa da plataforma")]
        public async Task Platform_admin_permite_admin_da_plataforma()
        {
            Assert.True(await PlatformAdminAllows(BuildProvider(), User("Admin", 1)));
        }

        [Fact(DisplayName = "PlatformAdmin nega Admin de empresa cliente")]
        public async Task Platform_admin_nega_admin_de_empresa_cliente()
        {
            Assert.False(await PlatformAdminAllows(BuildProvider(), User("Admin", 5)));
        }

        [Fact(DisplayName = "PlatformAdmin nega Manager da empresa da plataforma")]
        public async Task Platform_admin_nega_manager_da_plataforma()
        {
            Assert.False(await PlatformAdminAllows(BuildProvider(), User("Manager", 1)));
        }

        [Fact(DisplayName = "PlatformAdmin nega usuário anônimo")]
        public async Task Platform_admin_nega_anonimo()
        {
            Assert.False(await PlatformAdminAllows(BuildProvider(), Anonymous));
        }

        [Fact(DisplayName = "PlatformAdmin sem a chave na configuração usa a empresa padrão 1")]
        public async Task Platform_admin_sem_chave_usa_padrao()
        {
            var provider = BuildProvider(platformCompanyId: null);

            Assert.True(await PlatformAdminAllows(provider, User("Admin", 1)));
            Assert.False(await PlatformAdminAllows(provider, User("Admin", 2)));
        }

        [Fact(DisplayName = "PlatformAdmin com CompanyId <= 0 nega a todos (fail closed)")]
        public async Task Platform_admin_sem_configuracao_nega_todos()
        {
            var provider = BuildProvider(platformCompanyId: "0");

            Assert.False(await PlatformAdminAllows(provider, User("Admin", 0)));
            Assert.False(await PlatformAdminAllows(provider, User("Admin", 1)));
        }
    }
}
