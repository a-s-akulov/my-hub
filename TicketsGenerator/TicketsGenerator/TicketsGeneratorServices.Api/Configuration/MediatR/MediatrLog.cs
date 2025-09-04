namespace TicketsGeneratorServices.Api.Configuration.MediatR
{

    /// <summary>
    /// Оптимизированное логирование для частых логов
    /// </summary>
    public static partial class MediatrLog
    {
        [LoggerMessage(
            EventId = 1001,
            Level = LogLevel.Information,
            Message = "Запуск обработчика V1 {HandlerName}")]
        public static partial void StartHandlerV1(ILogger logger, string handlerName);

        [LoggerMessage(
            EventId = 1002,
            Level = LogLevel.Information,
            Message = "Обработчик V1 {HandlerName} успешно завершился через {HandlerDuration}мс.")]
        public static partial void FinishHandlerV1(ILogger logger, string handlerName, double handlerDuration);

        [LoggerMessage(
            EventId = 1003,
            Level = LogLevel.Error,
            Message = "Обработчик V1 {HandlerName} завершился с ошибкой через {HandlerDuration}мс.")]
        public static partial void ErrorFinishHandlerV1(ILogger logger, Exception ex, string handlerName, double handlerDuration);
    }
}
