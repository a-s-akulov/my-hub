using TicketsGeneratorServices.Common.Configuration;
using TicketsGeneratorServices.ScheduleJobs.Options;


namespace TicketsGeneratorServices.ScheduleJobs.Configuration
{
    /// <summary>
    /// Расширения для интеграции TicketsGeneratorStorageDbService в приложение
    /// </summary>
    public static class TicketsGeneratorStorageDbServiceConfigurationHostingExtensions
    {
        #region Методы

        public static IHostApplicationBuilder AddTicketsGeneratorStorageDbServiceInApp(this IHostApplicationBuilder builder, AppOptions appOptions)
        {
            builder.Services.AddTicketsGeneratorStorageDbServiceInApp(appOptions.TicketsGeneratorStorageDbService);

            return builder;
        }

        #endregion Методы
    }
}