using System.Diagnostics;
using MediatR;
using TicketsGeneratorServices.Api.RequestHandlers.Base;


namespace TicketsGeneratorServices.Api.Configuration.MediatR.Behaviors
{
    /// <summary>
    /// Логгер и трассировщик команд медиатора
    /// </summary>
    /// <typeparam name="TRequest">Запрос</typeparam>
    /// <typeparam name="TResponse">Ответ</typeparam>
    public class LoggingAndTracingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        #region Поля

        /// <summary>
        /// Логгер поведения
        /// </summary>
        private readonly ILogger<LoggingAndTracingBehavior<TRequest, TResponse>> _logger;

        /// <summary>
        /// Поставщик трассировки
        /// </summary>
        private readonly ActivitySource _activitySource;

        private readonly string _requestHandlerName;

        #endregion Поля


        #region Конструктор

        public LoggingAndTracingBehavior(IRequestHandler<TRequest, TResponse> requestHandler, ILogger<LoggingAndTracingBehavior<TRequest, TResponse>> logger, ActivitySource activitySource)
        {
            _logger = logger;
            _activitySource = activitySource;
            _requestHandlerName = requestHandler.GetType().Name;
        }

        #endregion Конструктор


        #region Методы

        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken) =>
            HandlePrivate(request, _requestHandlerName, next, _logger, _activitySource, cancellationToken);


        /// <summary>
        /// Обработчик команды
        /// </summary>
        private static async Task<TResponse> HandlePrivate(TRequest request, string requestHandlerName, RequestHandlerDelegate<TResponse> next, ILogger logger, ActivitySource activitySource, CancellationToken cancellationToken)
        {
            var startTime = DateTime.Now;
            MediatrLog.StartHandlerV1(logger, requestHandlerName);

            TResponse response;
            using (var tracingActivity = activitySource.StartActivity(name: requestHandlerName))
            {
                try
                {
                    response = await next().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    tracingActivity?.Stop();
                    var handlerException = ex as RequestHandlerException ?? new RequestHandlerException(ex, "QueryHandler");

                    MediatrLog.ErrorFinishHandlerV1(logger, handlerException, requestHandlerName, tracingActivity?.Duration.TotalMilliseconds ?? (DateTime.Now - startTime).TotalMilliseconds);
                    tracingActivity?.SetStatus(ActivityStatusCode.Error, description: handlerException.ToString());

                    throw handlerException;
                }

                tracingActivity?.Stop();
                MediatrLog.FinishHandlerV1(logger, requestHandlerName, tracingActivity?.Duration.TotalMilliseconds ?? (DateTime.Now - startTime).TotalMilliseconds);
            }

            return response;
        }

        #endregion Методы
    }
}
