using TicketsGeneratorServices.Common.Options.Base;


namespace TicketsGeneratorServices.ScheduleJobs.Options
{
    public class AppOptions
    {
        /// <summary>
        /// Настройка подключения к API Partners
        /// </summary>
        public ApiConnectionOptions PartnersApiService { get; init; }

        /// <summary>
        /// Настройка подключения к БД TicketsGenerator
        /// </summary>
        public DbConnectionOptions TicketsGeneratorStorageDbService { get; init; }

        /// <summary>
        /// Настройка подключения к БД для Quartz
        /// </summary>
        public DbConnectionOptions QuartzDbService { get; init; }



        /// <summary>
        /// Настройка джобы MyAwesomeProductsImport
        /// </summary>
        public ScheduleJobOptions MyAwesomeProductsImportJob { get; init; }



        /// <summary>
        /// Название приложения
        /// </summary>
        public string ServiceName { get; init; }

        /// <summary>
        /// Постфикс базового адреса сервиса
        /// </summary>
        public string HostPostfix { get; set; } = string.Empty;

        /// <summary>
        /// Настройки для подключения к сервису AccessAPI
        /// </summary>
        public AccessContextOptions AccessContext { get; init; }
    }
}