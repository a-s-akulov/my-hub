using TicketsGeneratorServices.Db.Entities;


namespace TicketsGeneratorServices.Common.Services.TicketsGeneratorStorageService
{
    /// <summary>
    /// Сервис хранилища данных TicketsGenerator
    /// </summary>
    public interface ITicketsGeneratorStorageService
    {
        #region MyAwesomeProduct

        /// <summary>
        /// Получить метрики данных из хранилища
        /// </summary>
        public Task<StorageDataMetrics> GetStorageDataMetrics(CancellationToken cancellationToken = default);

        /// <summary>
        /// Добавить или обновить базовую информацию о продукте
        /// </summary>
        public Task<TryAddOrUpdateBaseResult<MyAwesomeProduct>> TryAddOrUpdate(MyAwesomeProductBase recordToSet, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получить продукты
        /// </summary>
        public Task<List<MyAwesomeProduct>> GetMyAwesomeProducts(ICollection<Guid>? idsFilter = null, ICollection<enAwesomeProductType?>? productTypesFilter = null, bool includeReferences = false, bool includeLogs = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Удалить продукт
        /// </summary>
        public Task<bool> DeleteMyAwesomeProduct(Guid id, CancellationToken cancellationToken = default);

        #endregion MyAwesomeProduct
    }
}
