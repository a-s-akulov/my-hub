using TicketsGeneratorServices.Api.Options;
using TicketsGeneratorServices.Common.Configuration;


namespace TicketsGeneratorServices.Api.Configuration
{
    public static class OptionsConfigurationHostingExtensions
    {
        public static AppOptions ConfigureOptions(this IHostApplicationBuilder builder)
        {
            var appOptions = builder.Services.ConfigureOptions<AppOptions>(builder.Configuration);

            return appOptions;
        }
    }
}
