using AccessApi.ClientLibCore.Helpers;
using AccessApi.ClientLibCore.Model;
using TicketsGeneratorServices.ScheduleJobs.Auth;
using TicketsGeneratorServices.ScheduleJobs.Options;


namespace TicketsGeneratorServices.ScheduleJobs.Configuration
{
    public static class AccessApiConfigurationHostingExtensions
    {
        public static IHostApplicationBuilder AddAccessApiAuthenticationInApp(this IHostApplicationBuilder builder, AppOptions options)
        {
            Action<AccessApiAuthSchemeOptions> optionsBuilder = o =>
            {
                o.UrlPasswordAuth = options.AccessContext.UrlPasswordAuth;
                o.UrlWindowsAuth = options.AccessContext.UrlWindowsAuth;
                o.ApplicationName = options.AccessContext.ApplicationName;
                o.ApplicationActions = Enum.GetValues<enAppAction>().ToDictionary(x => (int)x, x => x.GetDescription());
            };

            builder.Services
                .AddAuthentication(AccessApiAuthSchemas.APIKEY)
                .AddAccessApiKey(optionsBuilder, ServiceLifetime.Singleton)
                .AddAccessApiBasic(optionsBuilder, ServiceLifetime.Singleton);

            return builder;
        }
    }
}
