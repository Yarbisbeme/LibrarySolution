using System.Data.Common;
using System.Net;
using System.Text.Json;
using Library.Common.Dto;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Library.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Recurso no encontrado");
                await HandleExceptionAsync(context, ex.Message, HttpStatusCode.NotFound);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Esta expresion es incorrecta");
                await HandleExceptionAsync(context, ex.Message, HttpStatusCode.BadRequest);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error de operación");
                await HandleExceptionAsync(context, ex.Message, HttpStatusCode.InternalServerError);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error en base de datos");
                await HandleExceptionAsync(context,
                    "Error al guardar datos en la base de datos.",
                    HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado");
                await HandleExceptionAsync(context,
                    "Error interno del servidor.",
                    HttpStatusCode.InternalServerError);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, string message, HttpStatusCode statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = ApiResponse<object>.ErrorResponse(message);
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }

    // Extensión para usarlo fácilmente
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
