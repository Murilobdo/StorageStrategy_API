using Microsoft.AspNetCore.Authorization;
using StorageStrategy.Utils.Services;

namespace StorageStrategy.API.Configuration
{
    /// <summary>
    /// Regras de autorização da API (P-1 / feature 003):
    /// - FallbackPolicy: todo endpoint sem metadado de autorização exige usuário autenticado
    ///   (exceções só com [AllowAnonymous] explícito, vigiadas pelo AnonymousEndpointsTest);
    /// - PlatformAdmin: cargo Admin E pertencer à empresa da plataforma (AppSettings:PlatformAdmin:CompanyId).
    /// </summary>
    public static class AuthorizationSetup
    {
        public const string PlatformAdminPolicy = "PlatformAdmin";
        public const string PlatformCompanyIdKey = "AppSettings:PlatformAdmin:CompanyId";

        /// <summary>
        /// Lê o ID da empresa da plataforma. Ausente: usa o padrão de <see cref="PlatformAdminSettings"/> (1),
        /// porque o appsettings.json da VPS é sobrescrito e pode não ter a chave. Valor explícito <= 0:
        /// política nega a todos.
        /// </summary>
        public static int GetPlatformCompanyId(IConfiguration configuration) =>
            configuration.GetValue<int?>(PlatformCompanyIdKey) ?? new PlatformAdminSettings().CompanyId;

        public static IServiceCollection AddApiAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            int platformCompanyId = GetPlatformCompanyId(configuration);

            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                options.AddPolicy(PlatformAdminPolicy, policy =>
                {
                    policy.RequireAuthenticatedUser();

                    if (platformCompanyId <= 0)
                    {
                        // Fail closed: sem a empresa da plataforma configurada, ninguém administra empresas.
                        policy.RequireAssertion(_ => false);
                        return;
                    }

                    policy.RequireRole("Admin");
                    policy.RequireClaim("CompanyId", platformCompanyId.ToString());
                });
            });

            return services;
        }
    }
}
