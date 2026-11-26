using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace StorageStrategy.API.Configuration
{
    /// <summary>
    /// Declara o esquema "Bearer" (JWT) no documento OpenAPI, para a interface Scalar permitir informar
    /// o token e chamar endpoints protegidos (FR-016b). A documentação só é mapeada em Development.
    /// </summary>
    public sealed class BearerSecuritySchemeTransformer : IOpenApiDocumentTransformer
    {
        public const string SchemeName = "Bearer";

        public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
        {
            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
            document.Components.SecuritySchemes[SchemeName] = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Token JWT retornado por POST api/Account/login."
            };

            document.Security ??= new List<OpenApiSecurityRequirement>();
            document.Security.Add(new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference(SchemeName, document)] = new List<string>()
            });

            return Task.CompletedTask;
        }
    }
}
