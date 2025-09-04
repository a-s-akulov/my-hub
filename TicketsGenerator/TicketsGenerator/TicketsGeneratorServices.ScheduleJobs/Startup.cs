using TicketsGeneratorServices.ScheduleJobs.Configuration;
using TicketsGeneratorServices.ScheduleJobs.Configuration.Middleware;


namespace TicketsGeneratorServices.ScheduleJobs
{
    public static class Startup
    {
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            var options = builder.ConfigureOptions();

            builder.Services.AddControllers();
            builder.Services.AddHealthChecks();
            builder.Services.AddMemoryCache();

            builder.AddControllersInApp();                                          // Controllers
            builder.AddJsonSerializerInApp();                                       // JsonSerializer
            builder.AddSwaggerInApp(options);                                       // Swagger
            builder.AddApiVersioningInApp();                                        // ApiVersioning
            builder.AddSerilogInApp();                                              // Serilog
            builder.AddAccessApiAuthenticationInApp(options);                       // AccessApi
            builder.AddAutoMapperInApp();                                           // AutoMapper
            builder.AddOpenTelemetryInApp();                                        // OpenTelemetry
            builder.AddQuartzInApp(options);                                        // Quartz
            builder.AddMetricsInApp();                                              // Metrics

            builder.AddPartnersApiServiceInApp(options);
            builder.AddTicketsGeneratorStorageDbServiceInApp(options);



            return builder;
        }




        public static WebApplication ConfigureApplication(this WebApplication app)
        {
            app.UseSwaggerInApp();


            app.UseRouting();

            if (app.Environment.IsProduction())
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseMiddleware<ExceptionMiddleware>();

            app.MapControllers();
            app.MapHealthChecks("/healthz");

            return app;
        }
    }
}
