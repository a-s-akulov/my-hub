using TicketsGeneratorServices.Common.Configuration;


namespace TicketsGeneratorServices.Api.Configuration
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
