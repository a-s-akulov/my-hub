using TicketsGeneratorServices.Api.Options;
using TicketsGeneratorServices.Common.Configuration;


namespace TicketsGeneratorServices.Api.Configuration
{
    /// <summary>
    /// Расширения для интеграции TicketsGeneratorStorageDbService в приложение
    /// </summary>
    public static class TicketsGeneratorStorageDbServiceConfigurationHostingExtensions
    {
        public static IHostApplicationBuilder AddTicketsGeneratorStorageDbServiceInApp(this IHostApplicationBuilder builder, AppOptions appOptions)
        {
            builder.Services.AddTicketsGeneratorStorageDbServiceInApp(appOptions.TicketsGeneratorStorageDbService);

            return builder;
        }
    }
}