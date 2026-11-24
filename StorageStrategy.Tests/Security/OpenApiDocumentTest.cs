using System.Net;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using StorageStrategy.API.Configuration;
using StorageStrategy.API.Controllers;
using Xunit;

namespace StorageStrategy.Tests.Security
{
    /// <summary>
    /// O documento OpenAPI (base do Scalar) precisa ser gerado sem erro para todos os controllers.
    /// Sobe só o pipeline de controllers + OpenAPI, sem banco, seed ou migrations.
    /// </summary>
    public class OpenApiDocumentTest
    {
        [Fact(DisplayName = "GET /openapi/v1.json gera o documento sem erro")]
        public async Task Documento_openapi_e_gerado()
        {
            var builder = WebApplication.CreateBuilder();
            builder.WebHost.UseUrls("http://127.0.0.1:0");
            builder.Services.AddControllers()
                .AddApplicationPart(typeof(AccountController).Assembly)
                .AddJsonOptions(o => o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            builder.Services.AddOpenApi(o => o.AddDocumentTransformer<BearerSecuritySchemeTransformer>());

            await using var app = builder.Build();
            app.MapControllers();
            app.MapOpenApi();
            await app.StartAsync();

            var address = app.Services.GetRequiredService<IServer>()
                .Features.Get<IServerAddressesFeature>()!.Addresses.First();
            using var client = new HttpClient { BaseAddress = new Uri(address) };

            var response = await client.GetAsync("/openapi/v1.json");
            var body = await response.Content.ReadAsStringAsync();

            Assert.True(response.StatusCode == HttpStatusCode.OK, body.Length > 2000 ? body[..2000] : body);
            Assert.Contains("\"Bearer\"", body);

            await app.StopAsync();
        }
    }
}
