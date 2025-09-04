using System.Net;
using System.Net.Mime;


namespace TicketsGeneratorServices.Api.Configuration.Middleware
{
    public class ExceptionMiddleware
    {
        #region Поля

        /// <summary>
        /// Делегат запроса
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// Логгер модуля
        /// </summary>
        private readonly ILogger<ExceptionMiddleware> _logger;

        #endregion Поля


        #region Конструктор

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        #endregion Конструктор


        #region Методы

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");

                await HandleExceptionAsync(httpContext, ex);
            }
        }


        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var errorDetails = new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.ToString()
            };

            await context.Response.WriteAsJsonAsync(errorDetails);
        }

        #endregion Методы


        #region Типы

        private record ErrorDetails
        {
            public int StatusCode { get; init; }

            public string Message { get; init; } = string.Empty;
        }

        #endregion Типы
    }
}