

namespace TicketsGeneratorServices.Common.Models.Base
{

    /// <summary>
    /// Запрос к API
    /// </summary>
    public abstract class ApiPagedRequest : ApiRequest
    {
        public ApiPagedRequest() : base()
        { }


        /// <summary>
        /// Информация о постраничности запроса к API
        /// <br/>Если не указана - будет возвращена первая страница с количеством элементов по умолчанию (зависит от метода)
        /// </summary>
        public ApiPaginationRequest? Pagination { get; set; }
    }
}