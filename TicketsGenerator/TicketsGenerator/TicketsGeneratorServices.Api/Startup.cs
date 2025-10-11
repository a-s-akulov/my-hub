using TicketsGenerator.ServiceDefaults.Configuration;
using TicketsGeneratorServices.Api.Configuration;
using TicketsGeneratorServices.Api.Configuration.Middleware;


namespace TicketsGeneratorServices.Api;


public static class Startup
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        var options = builder.ConfigureOptions();

        // Add service defaults & Aspire client integrations.
        builder.AddServiceDefaults();

        //builder.Services.AddHealthChecks();
        builder.Services.AddMemoryCache();

        builder.AddControllersInApp();                                          // Controllers
        builder.AddJsonSerializerInApp();                                       // JsonSerializer
        builder.AddSwaggerInApp(options);                                       // Swagger
        builder.AddApiVersioningInApp();                                        // ApiVersioning
        builder.AddSerilogInApp();                                              // Serilog
        builder.AddAutoMapperInApp();                                           // AutoMapper
        builder.AddMediatRInApp();                                              // MediatR
        builder.AddMetricsInApp();                                              // Metrics

        builder.AddTicketsGeneratorStorageDbServiceInApp(options);

        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        // WakeUp
        var webUri = builder.Environment.IsDevelopment() ? "http://localhost:5119" : "https://my-hub-web.onrender.com";
        builder.Services.AddAppWakeUpWorker(webUri);



        return builder;
    }




    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        app.MapDefaultEndpoints();
        app.UseSwaggerInApp();


        app.UseRouting();

        if (app.Environment.IsProduction())
        {
            app.UseHsts();
        }

        // TODO: ENABLE HTTPS REDIRECTION
        //app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseMiddleware<ExceptionMiddleware>();

        app.MapControllers();
        //app.MapHealthChecks("/healthz");

        // WakeUp
        app.MapGet("/", static () => 200);

        return app;
    }
}


record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}