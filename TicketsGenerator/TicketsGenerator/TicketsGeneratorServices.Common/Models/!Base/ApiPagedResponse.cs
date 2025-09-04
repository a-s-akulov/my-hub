using System.Net;


namespace TicketsGeneratorServices.Common.Models.Base
{
    /// <summary>
    /// Постраничный ответ на запрос от JSON API
    /// </summary>
    public class ApiPagedResponse<TResult> : ApiResponse<TResult> where TResult : class
    {
        public ApiPagedResponse(TResult? result, ICollection<ApiError>? errors = null) : base(result, errors: errors)
        { }
        public ApiPagedResponse(TResult? result, HttpStatusCode statusCode, ICollection<ApiError>? errors = null) : base(result, statusCode, errors: errors)
        { }

        public ApiPagination Pagination { get; set; } = new();
    }
}