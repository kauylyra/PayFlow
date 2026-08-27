using AutoMapper;
using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using PayFlow.application.DI;
using PayFlow.application.Mapping;
using PayFlow.Infra.Context;

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();