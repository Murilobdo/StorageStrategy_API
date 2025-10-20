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

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

ConfigureCors();
builder.Services.AddDbContext<StorageDbContext>();

ConfigureDependencyInjection();
ConfigureJwt();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

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
    if(pendingMigrations.Any())
        await context.Database.MigrateAsync();
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
    });
}

 