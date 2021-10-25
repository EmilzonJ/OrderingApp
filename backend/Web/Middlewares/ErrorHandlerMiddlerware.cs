using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Web.Errors;

namespace Web.Middlewares
{
    public class ErrorHandlerMiddlerware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddlerware> _logger;

        public ErrorHandlerMiddlerware(RequestDelegate next, ILogger<ErrorHandlerMiddlerware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception, _logger);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception,
            ILogger<ErrorHandlerMiddlerware> logger)
        {
            object errors = null;
            switch (exception)
            {
                case ApiException apiException:
                    logger.LogError(exception, "----------------------API ERROR---------------------");
                    errors = apiException.Errors;
                    context.Response.StatusCode = (int) apiException.Code;
                    break;

                case { } e:
                    logger.LogError(exception, "----------------------SERVER ERROR---------------------");
                    errors = string.IsNullOrEmpty(e.Message) ? "Error" : e.Message;
                    context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    break;
            }

            context.Response.ContentType = "application/json";

            if (errors != null)
            {
                var result = JsonSerializer.Serialize(new { errors });
                await context.Response.WriteAsync(result);
            }
        }
    }
}