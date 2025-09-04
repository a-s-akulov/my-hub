using TicketsGeneratorServices.Common.Configuration.Metrics.CustomMetrics.TicketsGeneratorStorageDataMetrics;


namespace TicketsGeneratorServices.Common.Configuration.Metrics.CustomMetrics
{
    public static class TicketsGeneratorStorageDataMetricsConfigurationHostingExtensions
    {
        public static IServiceCollection AddAppMetricsTicketsGeneratorStorageDataMetricsCollector(this IServiceCollection services, Action<TicketsGeneratorStorageDataMetricsCollectorOptions>? optionsSetup = null)
        {
            var collectorOptions = new TicketsGeneratorStorageDataMetricsCollectorOptions();
            optionsSetup?.Invoke(collectorOptions);
            services.AddSingleton(Microsoft.Extensions.Options.Options.Create(collectorOptions));

            services.AddHostedService<TicketsGeneratorStorageDataMetricsCollectorHostedService>();


            return services;
        }
    }
}
