using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using TicketsGeneratorServices.Common.Services.JsonSerializerService;
using TicketsGeneratorServices.Common.Services.JsonSerializerService.Repository;


namespace TicketsGeneratorServices.Common.Configuration
{
    public static class JsonSerializerConfigurationHostingExtensions
    {
        public static IServiceCollection AddJsonSerializerInApp(this IServiceCollection services)
        {
            var encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic);

            var jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerOptions.Default)
            {
                Encoder = encoder
            };

            services.AddSingleton(Microsoft.Extensions.Options.Options.Create(jsonSerializerOptions));
            services.AddSingleton<IJsonSerializerService, SystemTextJsonSerializerService>();

            return services;
        }
    }
}
