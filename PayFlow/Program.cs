using Asp.Versioning;
using FluentValidation;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PayFlow.application.DI;
using PayFlow.application.Crosscuttint.Dtos.Request.Customers;
using PayFlow.application.Crosscuttint.Dtos.Request.Payments;
using PayFlow.application.Validators;
using PayFlow.Infra.Context;
using PayFlow.web.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

ConfigServices.ConfigureDependenciesServices(builder.Services);
ConfigRepositories.ConfigureDependenciesRepositories(builder.Services);

var connectionString = builder.Configuration.GetConnectionString("stringConnection")
    ?? throw new InvalidOperationException("Connection string 'stringConnection' não configurada.");
builder.Services.AddDbContext<PayFlowDbContext>(options => options.UseSqlServer(connectionString));

builder.Host.UseSerilog((context, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console());

builder.Services.AddHealthChecks().AddSqlServer(connectionString);
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingToDto>());
builder.Services.AddControllers();
builder.Services.AddScoped<IValidator<AddCustomersRequest>, AddCustomersRequestValidator>();
builder.Services.AddScoped<IValidator<UpdateCustomerRequest>, UpdateCustomerRequestValidator>();
builder.Services.AddScoped<IValidator<AddPaymentRequest>, AddPaymentRequestValidator>();

builder.Services.AddApiVersioning(options =>
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
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "PayFlow API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Informe apenas o token JWT."
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddCors(options => options.AddPolicy("PayflowApp", policy =>
    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("PayflowApp");
app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health", new HealthCheckOptions { ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse });
app.Run();

public partial class Program;
