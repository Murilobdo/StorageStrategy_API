using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using StorageStrategy.API.Configuration;
using StorageStrategy.API.Controllers;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Domain.Services.Printing;
using StorageStrategy.Models;
using StorageStrategy.Tests.FakeRepository;
using Xunit;

namespace StorageStrategy.Tests.Security
{
    /// <summary>
    /// Feature 006 (research R5): o agente de impressão autentica com a chave da estação, nunca com JWT de
    /// funcionário; a empresa vem da estação cadastrada, nunca do cliente.
    /// </summary>
    public class PrintStationAuthenticationTest
    {
        private readonly FakePrintStationRepository _stations = new();

        private (string Key, PrintStationEntity Station) CriarEstacao(int companyId, bool ativa = true)
        {
            var station = new PrintStationEntity { CompanyId = companyId, Name = "Caixa", IsActive = ativa };
            _stations.AddAsync(station).Wait();
            var key = PrintStationKey.Generate(station.PrintStationId, out var hash);
            station.KeyHash = hash;
            station.KeyLastChars = PrintStationKey.LastChars(key);
            return (key, station);
        }

        private async Task<AuthenticateResult> Autenticar(string? authorization)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddSingleton<IPrintStationRepository>(_stations);
            services.AddAuthentication().AddPrintStationAuthentication();
            var provider = services.BuildServiceProvider();

            var context = new DefaultHttpContext { RequestServices = provider };
            if (authorization != null)
                context.Request.Headers.Authorization = authorization;

            var auth = provider.GetRequiredService<IAuthenticationService>();
            return await auth.AuthenticateAsync(context, PrintStationAuthenticationHandler.SchemeName);
        }

        [Fact(DisplayName = "Chave válida autentica com empresa, estação e role da estação")]
        public async Task Chave_valida_autentica()
        {
            var (key, station) = CriarEstacao(companyId: 7);

            var result = await Autenticar($"PrintStation {key}");

            Assert.True(result.Succeeded);
            var user = result.Principal!;
            Assert.Equal("7", user.FindFirstValue("CompanyId"));
            Assert.Equal(station.PrintStationId.ToString(), user.FindFirstValue(PrintStationAuthenticationHandler.StationIdClaim));
            Assert.True(user.IsInRole(PrintStationAuthenticationHandler.RoleName));
            Assert.False(user.IsInRole("Employee"));
            Assert.Null(user.FindFirst("EmployeeId"));
        }

        [Fact(DisplayName = "Chave com segredo errado não autentica")]
        public async Task Segredo_errado_falha()
        {
            var (key, station) = CriarEstacao(companyId: 7);
            var falsa = $"ps_{station.PrintStationId}_{new string('A', 43)}";

            var result = await Autenticar($"PrintStation {falsa}");

            Assert.False(result.Succeeded);
            Assert.NotEqual(key, falsa);
        }

        [Fact(DisplayName = "Chave de uma estação usada com o id de outra estação não autentica")]
        public async Task Chave_de_outra_estacao_falha()
        {
            var (keyA, stationA) = CriarEstacao(companyId: 1);
            var (_, stationB) = CriarEstacao(companyId: 2);
            var segredoA = keyA[($"ps_{stationA.PrintStationId}_").Length..];

            var result = await Autenticar($"PrintStation ps_{stationB.PrintStationId}_{segredoA}");

            Assert.False(result.Succeeded);
        }

        [Theory(DisplayName = "Chave malformada não autentica")]
        [InlineData("PrintStation ")]
        [InlineData("PrintStation abc")]
        [InlineData("PrintStation ps_")]
        [InlineData("PrintStation ps_x_segredo")]
        [InlineData("PrintStation ps_0_segredo")]
        [InlineData("PrintStation ps_1_")]
        public async Task Chave_malformada_falha(string header)
        {
            CriarEstacao(companyId: 1);

            var result = await Autenticar(header);

            Assert.False(result.Succeeded);
        }

        [Fact(DisplayName = "Estação inativa não autentica")]
        public async Task Estacao_inativa_falha()
        {
            var (key, _) = CriarEstacao(companyId: 7, ativa: false);

            var result = await Autenticar($"PrintStation {key}");

            Assert.False(result.Succeeded);
        }

        [Theory(DisplayName = "Sem header ou com JWT de funcionário o esquema da estação não autentica")]
        [InlineData(null)]
        [InlineData("Bearer eyJhbGciOiJIUzI1NiJ9.eyJDb21wYW55SWQiOiI3In0.assinatura")]
        public async Task Sem_chave_da_estacao_nao_autentica(string? header)
        {
            CriarEstacao(companyId: 7);

            var result = await Autenticar(header);

            Assert.False(result.Succeeded);
        }

        [Fact(DisplayName = "Endpoints do agente exigem o esquema e a role da estação (JWT de funcionário não entra)")]
        public void Endpoints_do_agente_exigem_esquema_da_estacao()
        {
            var authorize = typeof(PrintAgentController).GetCustomAttributes<AuthorizeAttribute>(inherit: true).ToList();

            Assert.Contains(authorize, a => a.AuthenticationSchemes == PrintStationAuthenticationHandler.SchemeName
                                            && a.Roles == PrintStationAuthenticationHandler.RoleName);
            Assert.DoesNotContain(typeof(PrintAgentController).GetMethods()
                    .Where(m => m.GetCustomAttributes<HttpMethodAttribute>().Any()),
                m => m.GetCustomAttributes<AllowAnonymousAttribute>().Any());
        }

        [Fact(DisplayName = "Endpoints de funcionário não aceitam a role da estação")]
        public void Endpoints_de_funcionario_nao_aceitam_estacao()
        {
            var roles = typeof(PrintController).GetCustomAttributes<AuthorizeAttribute>(inherit: true)
                .Select(a => a.Roles ?? string.Empty);

            Assert.All(roles, r => Assert.DoesNotContain(PrintStationAuthenticationHandler.RoleName, r));
            Assert.Contains(roles, r => r.Contains("Employee"));
        }
    }
}
