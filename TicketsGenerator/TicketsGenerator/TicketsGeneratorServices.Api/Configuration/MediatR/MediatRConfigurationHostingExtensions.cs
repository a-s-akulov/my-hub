using System.Reflection;
using TicketsGeneratorServices.Api.Configuration.MediatR.Behaviors;


namespace TicketsGeneratorServices.Api.Configuration
{
    public static class MediatRConfigurationHostingExtensions
    {
        public static IHostApplicationBuilder AddMediatRInApp(this IHostApplicationBuilder builder)
        {
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddOpenBehavior(typeof(LoggingAndTracingBehavior<,>));
            });


            return builder;
        }
    }
}
