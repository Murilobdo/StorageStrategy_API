using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Domain.Services.Printing;

namespace StorageStrategy.API.Configuration
{
    /// <summary>
    /// Autenticação do agente de impressão (feature 006, research R5):
    /// header <c>Authorization: PrintStation ps_{id}_{segredo}</c>. A empresa vem da estação cadastrada.
    /// Usado só pelos endpoints do <c>PrintAgentController</c>; o JWT de funcionário não é aceito lá,
    /// e a chave da estação não é aceita nos endpoints de funcionário.
    /// </summary>
    public class PrintStationAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public const string SchemeName = "PrintStation";
        public const string RoleName = "PrintStation";
        public const string StationIdClaim = "PrintStationId";

        public PrintStationAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder) : base(options, logger, encoder)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string? header = Request.Headers.Authorization;
            if (string.IsNullOrEmpty(header) || !header.StartsWith(SchemeName + " ", StringComparison.OrdinalIgnoreCase))
                return AuthenticateResult.NoResult();

            var key = header[(SchemeName.Length + 1)..].Trim();
            if (!PrintStationKey.TryParse(key, out var stationId, out var secret))
                return AuthenticateResult.Fail("Chave da estação inválida");

            var repository = Context.RequestServices.GetRequiredService<IPrintStationRepository>();
            var station = await repository.GetActiveByIdAsync(stationId);
            if (station is null || !PrintStationKey.Verify(secret, station.KeyHash))
            {
                Logger.LogWarning("Falha de autenticação da estação de impressão {PrintStationId}", stationId);
                return AuthenticateResult.Fail("Chave da estação inválida");
            }

            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, station.Name),
                new Claim(ClaimTypes.Role, RoleName),
                new Claim("CompanyId", station.CompanyId.ToString()),
                new Claim(StationIdClaim, station.PrintStationId.ToString()),
            }, SchemeName);

            return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(identity), SchemeName));
        }
    }

    public static class PrintStationAuthenticationExtensions
    {
        public static AuthenticationBuilder AddPrintStationAuthentication(this AuthenticationBuilder builder) =>
            builder.AddScheme<AuthenticationSchemeOptions, PrintStationAuthenticationHandler>(
                PrintStationAuthenticationHandler.SchemeName, _ => { });
    }
}
