using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json.Serialization;


namespace TicketsGeneratorServices.Common.Models.Base
{
    /// <summary>
    /// Ответ на запрос от API
    /// </summary>
    public class ApiResponse
    {
        public ApiResponse(HttpStatusCode statusCode, ICollection<ApiError>? errors = null)
        {
            StatusCode = statusCode;
            Errors = errors;
        }

        /// <summary>
        /// Ошибки, возникшие в результате  обработки запроса
        /// </summary>
        public ICollection<ApiError>? Errors { get; }

        /// <summary>
        /// Код статуса ответа
        /// </summary>
        [JsonIgnore]
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Есть ли ошибки?
        /// </summary>
        [MemberNotNullWhen(true, nameof(Errors))]
        public bool HasErrors => Errors is { Count: > 0 };

        /// <summary>
        /// Указывает ли код статуса ответа на успешный результат?
        /// </summary>
        [JsonIgnore]
        public bool IsSuccessStatusCode => (int)StatusCode >= 200 && (int)StatusCode <= 299;

        /// <summary>
        /// Бросает исключение, если код статуса ответа не указывает на успешный результат
        /// </summary>
        public ApiResponse EnsureSuccessStatusCode()
        {
            if (!IsSuccessStatusCode)
                throw new HttpRequestException($"Response status code does not indicate success: {(int)StatusCode}", null, StatusCode);

            return this;
        }
    }



    /// <summary>
    /// Ответ на запрос от API
    /// </summary>
    public class ApiResponse<TResult> : ApiResponse
    {
        public ApiResponse(TResult? result, ICollection<ApiError>? errors = null) : base(errors == null || errors.Count == 0 ? HttpStatusCode.OK : HttpStatusCode.InternalServerError, errors: errors)
        {
            Result = result;
        }
        public ApiResponse(TResult? result, HttpStatusCode statusCode, ICollection<ApiError>? errors = null) : base(statusCode, errors: errors)
        {
            Result = result;
        }


        /// <summary>
        /// Результат запроса
        /// </summary>
        public TResult? Result { get; }

        /// <summary>
        /// Успешен ли запрос и есть ли результат?
        /// </summary>
        [MemberNotNullWhen(true, nameof(Result))]
        public bool HasResult => !HasErrors && Result != null;

        /// <summary>
        /// Бросает исключение, если код статуса ответа не указывает на успешный результат
        /// </summary>
        public new ApiResponse<TResult> EnsureSuccessStatusCode() => (ApiResponse<TResult>)base.EnsureSuccessStatusCode();
    }
}