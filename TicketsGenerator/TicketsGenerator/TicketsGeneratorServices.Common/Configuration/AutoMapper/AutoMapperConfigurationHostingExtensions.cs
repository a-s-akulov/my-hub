using System.Reflection;
using Microsoft.Extensions.DependencyInjection;


namespace TicketsGeneratorServices.Common.Configuration
{
    public static class AutoMapperConfigurationHostingExtensions
    {
        public static IServiceCollection AddAutoMapperInApp(this IServiceCollection services)
        {
            services.AddAutoMapper(
                opt => { },
                [
                    Assembly.GetEntryAssembly(),
                    Assembly.GetAssembly(typeof(AutoMapperConfigurationHostingExtensions))
                ],
                ServiceLifetime.Singleton
            );


            return services;
        }
    }
}
