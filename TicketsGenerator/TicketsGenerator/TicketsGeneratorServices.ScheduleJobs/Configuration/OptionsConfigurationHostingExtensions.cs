using TicketsGeneratorServices.Common.Configuration;
using TicketsGeneratorServices.ScheduleJobs.Options;


namespace TicketsGeneratorServices.ScheduleJobs.Configuration
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
