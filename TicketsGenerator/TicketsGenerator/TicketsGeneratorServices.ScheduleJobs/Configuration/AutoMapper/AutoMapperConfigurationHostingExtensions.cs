using TicketsGeneratorServices.Common.Configuration;


namespace TicketsGeneratorServices.ScheduleJobs.Configuration
{
    public static class AutoMapperConfigurationHostingExtensions
    {
        public static IHostApplicationBuilder AddAutoMapperInApp(this IHostApplicationBuilder builder)
        {
            builder.Services.AddAutoMapperInApp();


            return builder;
        }
    }
}
