using TicketsGeneratorServices.Common.Configuration;


namespace TicketsGeneratorServices.ScheduleJobs.Configuration
{
    public static class MetricsConfigurationHostingExtensions
    {
        public static IHostApplicationBuilder AddMetricsInApp(this WebApplicationBuilder builder)
        {
            builder.Services.AddMetricsInApp(builder.Host, enableTicketsGeneratorStorageDataMetricsCollector: true);


            return builder;
        }
    }
}