using Asp.Versioning;


namespace TicketsGeneratorServices.Api.Configuration
{
    public static class ApiVersioningConfigurationHostingExtensions
    {
        public static IHostApplicationBuilder AddApiVersioningInApp(this IHostApplicationBuilder builder)
        {
            builder.Services.AddApiVersioning(options =>
                {
                    options.ReportApiVersions = true;
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                    options.ApiVersionReader = new UrlSegmentApiVersionReader();
                })
                .AddApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });


            return builder;
        }
    }
}
