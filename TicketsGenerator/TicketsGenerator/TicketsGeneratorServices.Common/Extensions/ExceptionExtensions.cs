

namespace TicketsGeneratorServices.Common.Extensions
{

    public static class ExceptionExtensions
    {
        public static ApiError ToApiError(this Exception exception, string errorCode = "", System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.InternalServerError)
        {
            if (errorCode == string.Empty)
                errorCode = statusCode.ToString();

            return new ApiError()
            {
                Code = errorCode,
                Title = $"[{exception.GetType().Name}] {exception.Message}",
                Detail = exception.ToString(),
                Source = exception.StackTrace?.Trim() ?? string.Empty
            };
        }


        public static ICollection<ApiError> ToApiErrors(this Exception exception, string errorCode = "", System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.InternalServerError)
        {
            var errors = new List<ApiError>(10);
            ToApiErrorsCore(exception, errors, errorCode: errorCode, statusCode: statusCode);

            return errors;
        }


        private static void ToApiErrorsCore(Exception exception, List<ApiError> errors, string errorCode = "", System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.InternalServerError)
        {
            var error = exception.ToApiError(errorCode: errorCode, statusCode: statusCode);
            errors.Add(error);

            if (exception.InnerException != null)
                ToApiErrorsCore(exception.InnerException, errors, errorCode: errorCode, statusCode: statusCode);

            if (exception is AggregateException aggregateException)
            {
                foreach (var innerException in aggregateException.InnerExceptions)
                    ToApiErrorsCore(innerException, errors, errorCode: errorCode, statusCode: statusCode);
            }
        }
    }
}