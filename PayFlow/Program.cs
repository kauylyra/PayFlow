using Asp.Versioning;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PayFlow.application.DI;
using PayFlow.application.Mapping;
using PayFlow.Infra.Context;
using PayFlow.web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

ConfigServices.ConfigureDependenciesServices(builder.Services);
ConfigRepositories.ConfigureDependenciesRepositories(builder.Services);

builder.Services.AddDbContext<PayFlowDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("stringConnection")));

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingToDto>();
});

builder.Services.AddControllers();

builder.Services
    .AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("PayflowApp", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("PayflowApp");

app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program;
