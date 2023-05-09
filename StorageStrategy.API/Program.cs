using MediatR;
using StorageStrategy.Data.Context;
using StorageStrategy.Data.Repository;
using StorageStrategy.Domain.Commands.Category;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

ConfigureCors();
builder.Services.AddDbContext<StorageDbContext>();

ConfigureDependencyInjection();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CorsStorage");

app.UseAuthorization();

app.MapControllers();

app.Run();


void ConfigureCors()
{
    builder.Services.AddCors(p => p.AddPolicy("CorsStorage", builder => {
        builder.WithOrigins("http://localhost:3000", "http://localhost:19006", "http://localhost:19007");
        builder.AllowAnyHeader();
        builder.AllowAnyMethod();
    }));
}

void ConfigureDependencyInjection()
{
    builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
    builder.Services.AddScoped<IProductRepository, ProductRepository>();
    builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
    builder.Services.AddScoped<ICommandRepository, CommandRepository>();
    builder.Services.AddScoped<IExpensesRepository, ExpensesRepository>();
    builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
}

 