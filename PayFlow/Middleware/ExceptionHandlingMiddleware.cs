using Microsoft.AspNetCore.Mvc;
using PayFlow.domain.Models;
using System.Net;
using System.Text.Json;

namespace PayFlow.web.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(
    HttpContext context,
    Exception exception)
    {
        var statusCode = exception switch
        {
            ExceptionValidation => HttpStatusCode.BadRequest,

            KeyNotFoundException => HttpStatusCode.NotFound,

            ArgumentException => HttpStatusCode.BadRequest,

            InvalidOperationException => HttpStatusCode.Conflict,

            _ => HttpStatusCode.InternalServerError
        };


        var problem = new ProblemDetails
        {
            Status = (int)statusCode,
            Title = "Erro na requisição",
            Detail = exception.Message,
            Instance = context.Request.Path
        };

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(problem);
    }

}