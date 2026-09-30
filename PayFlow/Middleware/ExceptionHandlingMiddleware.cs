using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayFlow.domain.Models;
using System.Net;

namespace PayFlow.web.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unhandled exception for {RequestPath}", context.Request.Path);
            await HandleExceptionAsync(context, exception);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = exception switch
        {
            ExceptionValidation or ValidationException or ArgumentException => HttpStatusCode.BadRequest,
            KeyNotFoundException => HttpStatusCode.NotFound,
            InvalidOperationException or DbUpdateConcurrencyException => HttpStatusCode.Conflict,
            _ => HttpStatusCode.InternalServerError
        };
        var title = statusCode switch
        {
            HttpStatusCode.BadRequest => "Bad request",
            HttpStatusCode.NotFound => "Resource not found",
            HttpStatusCode.Conflict => "Conflict",
            _ => "Internal server error"
        };
        var problem = new ProblemDetails
        {
            Type = $"https://httpstatuses.com/{(int)statusCode}",
            Status = (int)statusCode,
            Title = title,
            Detail = exception.Message,
            Instance = context.Request.Path
        };
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsJsonAsync(problem);
    }
}
