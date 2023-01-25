using GameInventory.Dto;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace GameInventory.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }            
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex.Message, HttpStatusCode.InternalServerError, "InternalServerError");
            }
        }

        private async Task HandleExceptionAsync(
            HttpContext context,
            string exMsg,
            HttpStatusCode httpStatusCode,
            string message)
        {
            _logger.LogError(exMsg);

            HttpResponse response = context.Response;

            response.ContentType = "application/json";
            response.StatusCode = (int)httpStatusCode;

            ErrorDto errorDto = new()
            {
                Message = message,
                StatusCode = (int)httpStatusCode
            };

            var result = errorDto.ToString();

            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };            

            await response.WriteAsJsonAsync(result, options);
        }
    }
}
