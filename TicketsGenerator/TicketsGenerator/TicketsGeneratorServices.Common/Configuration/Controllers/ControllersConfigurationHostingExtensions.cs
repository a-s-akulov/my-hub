using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;


namespace TicketsGeneratorServices.Common.Configuration
{
    public static class ControllersConfigurationHostingExtensions
    {
        public static IServiceCollection AddControllersInApp(this IServiceCollection services)
        {
            services
                .AddControllers()
                .AddJsonOptions(jsonOpt =>
                {
                    jsonOpt.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.SnakeCaseLower;
                    jsonOpt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                })
                .AddMetrics();


            return services;
        }
    }
}