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

        // KATTEST
        string[] summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];
        app.MapGet("/weatherforecast", () =>
        {
            var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
                .ToArray();
            return forecast;
        })
        .WithName("GetWeatherForecast");
        return app;
    }
}


record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}