using TicketsGeneratorServices.Common.Configuration;


namespace TicketsGeneratorServices.Api.Configuration
{
    public static class MetricsConfigurationHostingExtensions
    {
        public static IHostApplicationBuilder AddMetricsInApp(this WebApplicationBuilder builder)
        {
            builder.Services.AddMetricsInApp(builder.Host);


            return builder;
        }
    }
}