namespace TicketsGeneratorServices.ScheduleJobs.Jobs.Base
{

    /// <summary>
    /// Оптимизированное логирование для частых логов
    /// </summary>
    public static partial class ScheduleJobBaseLog
    {
        [LoggerMessage(
            EventId = 2001,
            Level = LogLevel.Information,
            Message = "Запуск джобы {JobName} '{JobExecutionInstanceId}'")]
        public static partial void StartJob(this ILogger logger, string jobName, string jobExecutionInstanceId);

        [LoggerMessage(
            EventId = 2002,
            Level = LogLevel.Information,
            Message = "Джоба {JobName} была отменена - джоба отключена конфигурацией приложения")]
        public static partial void CancelJobByConfiguration(this ILogger logger, string jobName, string jobExecutionInstanceId);

        [LoggerMessage(
            EventId = 2003,
            Level = LogLevel.Information,
            Message = "Джоба {JobName} была отменена условием выполнения через {JobDuration}мс. '{JobExecutionInstanceId}'")]
        public static partial void CancelJobByCondition(this ILogger logger, string jobName, double jobDuration, string jobExecutionInstanceId);

        [LoggerMessage(
            EventId = 2004,
            Level = LogLevel.Information,
            Message = "Джоба {JobName} успешно завершилась через {JobDuration}мс. '{JobExecutionInstanceId}'")]
        public static partial void FinishJob(this ILogger logger, string jobName, double jobDuration, string jobExecutionInstanceId);

        [LoggerMessage(
            EventId = 2005,
            Level = LogLevel.Error,
            Message = "Джоба {JobName} завершилась с ошибкой через {JobDuration}мс. '{JobExecutionInstanceId}'")]
        public static partial void ErrorFinishJob(this ILogger logger, Exception ex, string jobName, double jobDuration, string jobExecutionInstanceId);
    }
}
