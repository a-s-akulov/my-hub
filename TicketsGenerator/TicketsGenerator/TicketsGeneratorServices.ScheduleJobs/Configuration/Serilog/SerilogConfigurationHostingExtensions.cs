using Microsoft.Extensions.Options;
using Serilog;
using TicketsGeneratorServices.ScheduleJobs.Options;


namespace TicketsGeneratorServices.ScheduleJobs.Configuration
{
    public static class SerilogConfigurationHostingExtensions
    {
        public static IHostApplicationBuilder AddSerilogInApp(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((context, services, configuration) =>
            {
                configuration.ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.WithProperty("App", services.GetRequiredService<IOptions<AppOptions>>().Value.ServiceName);
            });


            return builder;
        }
    }
}
