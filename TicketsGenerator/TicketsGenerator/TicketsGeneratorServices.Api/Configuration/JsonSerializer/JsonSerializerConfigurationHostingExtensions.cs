using TicketsGeneratorServices.Common.Configuration;


namespace TicketsGeneratorServices.Api.Configuration
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
