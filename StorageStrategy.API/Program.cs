using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using StorageStrategy.Data.Context;
using StorageStrategy.Data.Repository;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Domain.Middleware;
using StorageStrategy.Utils.Services;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using StorageStrategy.Domain.Services.MinioStorage;
using StorageStrategy.Domain.Services.MercadoPago;
using StorageStrategy.Domain.Services.Refit;
using Refit;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using StorageStrategy.API.Configuration;
using StorageStrategy.Domain.Services.PlatformAdmin;
using Scalar.AspNetCore;
using StorageStrategy.Printer;
using StorageStrategy.Domain.Services.Printing;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Descrição OpenAPI nativa (.NET 10); interface Scalar só em Development (ver pipeline abaixo).
builder.Services.AddOpenApi(options => options.AddDocumentTransformer<BearerSecuritySchemeTransformer>());

ConfigureCors();
builder.Services.AddDbContext<StorageDbContext>();

ConfigureDependencyInjection();
ConfigureJwt();
builder.Services.AddApiAuthorization(builder.Configuration);
ConfigureDataProtection();

builder.Services.AddRefitClient<IMercadoPagoService>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.mercadopago.com"));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());


builder.Host.UseDefaultServiceProvider(cfg =>
{
    cfg.ValidateOnBuild = true;
});

var app = builder.Build();

if (AuthorizationSetup.GetPlatformCompanyId(app.Configuration) <= 0)
    app.Logger.LogWarning(
        "{Key} não configurado: nenhuma operação de administração de empresas será permitida.",
        AuthorizationSetup.PlatformCompanyIdKey);

// Configure the HTTP request pipeline.

// Documentação (descrição + interface) só em Development; em qualquer outro ambiente não é mapeada (404).
// AllowAnonymous explícito para a FallbackPolicy não bloquear a própria documentação local.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi().AllowAnonymous();
    app.MapScalarApiReference(options => options
            .WithTitle("StorageStrategy API")
            .AddPreferredSecuritySchemes(new[] { BearerSecuritySchemeTransformer.SchemeName }))
        .AllowAnonymous();
}

app.UseHttpsRedirection();

app.UseCors("Allow");

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware(typeof(ExceptionErrorMiddleware));

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<StorageDbContext>();
    var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
    if (pendingMigrations.Any())
    {
        await context.Database.MigrateAsync();
        Console.WriteLine("Aplicando migrações pendentes no banco de dados...");
    }
    else
    {
        Console.WriteLine("Nenhum migração pendente encontrada no banco de dados.");
    }

    // Empresa e admin da plataforma: o admin só é criado com AppSettings:PlatformAdmin:InitialPassword
    // configurada fora do código; admin existente nunca é alterado.
    await scope.ServiceProvider.GetRequiredService<PlatformAdminSeeder>().SeedAsync();
}

app.Run();


void ConfigureCors()
{
    builder.Services.AddCors(p => p.AddPolicy("Allow", builder => {
        builder.AllowAnyOrigin();
        builder.AllowAnyHeader();
        builder.AllowAnyMethod();
    }));

    builder.Services.AddCors(p => p.AddPolicy("CorsStorage", builder => {
        builder.WithOrigins("http://localhost:3000", "http://localhost:19006", "http://localhost:19007", "https://storage-strategy-site.vercel.app");
        builder.AllowAnyHeader();
        builder.AllowAnyMethod();
    }));
}

void ConfigureDependencyInjection()
{
    builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(nameof(AppSettings)));
    builder.Services.AddSingleton<TokenService>();
    builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
    builder.Services.AddScoped<IProductRepository, ProductRepository>();
    builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
    builder.Services.AddScoped<ICommandRepository, CommandRepository>();
    builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
    builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
    builder.Services.AddScoped<IReportRepository, ReportRepository>();
    builder.Services.AddScoped<IClientRepository, ClientRepository>();
    builder.Services.AddScoped<ILogRepository, LogRepository>();
    builder.Services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
    builder.Services.AddScoped<IStorageFile, StorageFile>();
    builder.Services.AddScoped<IPixChargeRepository, PixChargeRepository>();
    builder.Services.AddScoped<IPixCredentialRepository, PixCredentialRepository>();
    builder.Services.AddScoped<IPrintStationRepository, PrintStationRepository>();
    builder.Services.AddScoped<IPrintJobRepository, PrintJobRepository>();
    builder.Services.AddSingleton<IReceiptRenderer, ReceiptRenderer>();
    builder.Services.AddScoped<ISecretProtector, SecretProtector>();
    builder.Services.AddScoped<IMercadoPagoGateway, MercadoPagoGateway>();
    builder.Services.AddScoped<PlatformAdminSeeder>();
    builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
    builder.Services.AddHostedService<PixWebhookProcessingService>();
    builder.Services.AddHostedService<PixExpirationJob>();
    builder.Services.AddSingleton(TimeProvider.System);
    builder.Services.AddScoped<PrintJobMaintenance>();
    builder.Services.AddHostedService<PrintJobMaintenanceJob>();
}

void ConfigureDataProtection()
{
    // Key ring persistido no próprio banco (tabela DataProtectionKeys) em vez do
    // sistema de arquivos: sobrevive a rebuilds locais e a redeploys de container
    // (o filesystem do container é recriado do zero a cada deploy), evitando que
    // segredos já cifrados no banco (ex.: PixCredential) fiquem indecifráveis.
    builder.Services.AddDataProtection()
        .SetApplicationName("StorageStrategy")
        .PersistKeysToDbContext<StorageDbContext>();
}

void ConfigureJwt()
{
    builder.Services.AddAuthentication(x => {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(x => {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["AppSettings:JwtKey"])),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    })
    // Agente de impressão (feature 006): chave da estação, usada só no PrintAgentController.
    .AddPrintStationAuthentication();
}
