namespace TicketsGeneratorServices.Common.Models.Base
{

    /// <summary>
    /// Исключение с указанной областью
    /// </summary>
    public class ScopedException : Exception
    {
        public ScopedException(string exceptionScope) : this(null, null, exceptionScope)
        { }

        public ScopedException(Exception? exception, string exceptionScope) : this(null, exception, exceptionScope)
        { }

        public ScopedException(string? message, Exception? exception, string exceptionScope) : base(message, exception)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(exceptionScope);

            ExceptionScope = exceptionScope;
        }

        /// <summary>
        /// Область исключения
        /// </summary>
        public string ExceptionScope { get; init; }
    }
}
