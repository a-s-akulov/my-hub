

namespace TicketsGeneratorServices.Common.Services.PartnersService
{

    /// <summary>
    /// Сервис получения данных о магазинах
    /// </summary>
    public interface IPartnersService
    {
        /// <summary>
        /// Получение информации о магазинах
        /// <br/>Если фильтр не задан - будут возвращены все магазины
        /// </summary>
        /// <param name="partnersIdsFilter">Фильтр - коллекция идентификаторов магазинов</param>
        /// <returns>Коллекция информации о магазинах</returns>
        Task<ICollection<PartnerOrsInfo>> GetPartnersInfo(ICollection<int>? partnersIdsFilter = null);


        /// <summary>
        /// Получение информации о магазине по фильтру
        /// </summary>
        /// <param name="partnerId">Идентификатор магазина</param>
        /// <returns>Информация о магазине, либо <see langword="null"/>, если указанный магазин не найден</returns>
        Task<PartnerOrsInfo?> GetPartnerInfo(int partnerId);
    }
}
