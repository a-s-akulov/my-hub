using TicketsGeneratorServices.Common.Configuration;


namespace TicketsGeneratorServices.ScheduleJobs.Configuration
{
    public static class JsonSerializerConfigurationHostingExtensions
    {
        public static IHostApplicationBuilder AddJsonSerializerInApp(this IHostApplicationBuilder builder)
        {
            builder.Services.AddJsonSerializerInApp();
            return builder;
        }
    }
}
