using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace RoomReservation.API.Middlewares
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
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Erro de serialização JSON");
                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, "Erro ao ler os dados enviados. Verifique se o JSON está válido.");
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Erro de validação");
                var errors = ex.Errors.Select(e => new { campo = e.PropertyName, erro = e.ErrorMessage });
                await HandleJsonAsync(context, HttpStatusCode.BadRequest, new
                {
                    sucesso = false,
                    mensagem = "Erro de validação",
                    erros = errors
                });
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqliteException sqliteEx)
            {
                _logger.LogError(ex, "Erro de banco de dados");

                var message = sqliteEx.SqliteErrorCode switch
                {
                    19 => "Violação de integridade no banco de dados. Verifique se os dados relacionados existem.",
                    _ => "Erro ao salvar dados no banco de dados."
                };

                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, message);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Acesso não autorizado");
                await HandleExceptionAsync(context, HttpStatusCode.Unauthorized, "Acesso não autorizado.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno inesperado");
                await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, "Ocorreu um erro inesperado no servidor.");
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var result = JsonSerializer.Serialize(new
            {
                sucesso = false,
                status = (int)statusCode,
                mensagem = message
            });

            await context.Response.WriteAsync(result);
        }

        private async Task HandleJsonAsync(HttpContext context, HttpStatusCode statusCode, object response)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            var result = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(result);
        }
    }
}
