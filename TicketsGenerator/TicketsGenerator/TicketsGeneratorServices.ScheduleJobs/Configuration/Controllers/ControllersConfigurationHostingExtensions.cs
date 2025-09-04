using TicketsGeneratorServices.Common.Configuration;


namespace TicketsGeneratorServices.ScheduleJobs.Configuration
{
    public static class ControllersConfigurationHostingExtensions
    {
        public static IHostApplicationBuilder AddControllersInApp(this IHostApplicationBuilder builder)
        {
            builder.Services.AddControllersInApp();


            return builder;
        }
    }
}
