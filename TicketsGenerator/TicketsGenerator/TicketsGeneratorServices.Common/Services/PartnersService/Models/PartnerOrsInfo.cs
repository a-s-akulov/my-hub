using TicketsGeneratorServices.Common.Services.PartnersService.Repository;


namespace TicketsGeneratorServices.Common.Services.PartnersService
{
    /// <summary>
    /// Информация о партнере
    /// </summary>
    public record PartnerOrsInfo
    {
        #region Свойства

        /// <summary>
        /// Уникальный идентификатор партнера
        /// </summary>
        public int PartnerId { get; init; }

        /// <summary>
        /// Информация о партнере (базовая)
        /// </summary>
        public DTOPartnerFullInfo InfoBase { get; init; } = new();

        #endregion Свойства
    }
}