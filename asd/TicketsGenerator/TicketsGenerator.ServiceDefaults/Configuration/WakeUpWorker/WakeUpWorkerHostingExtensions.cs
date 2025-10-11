using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using TicketsGenerator.ServiceDefaults.Configuration.WakeUpWorker;


namespace TicketsGenerator.ServiceDefaults.Configuration;


public static class WakeUpWorkerHostingExtensions
{
    public static IServiceCollection AddAppWakeUpWorker(this  IServiceCollection services, string targetUri)
    {
        services.AddHostedService<WakeUpWorkerHostedService>(serviceProvider => new WakeUpWorkerHostedService(
            serviceProvider.GetRequiredService<ILogger<WakeUpWorkerHostedService>>(),
            serviceProvider.GetRequiredService<ActivitySource>(),
            targetUri
        ));


        return services;
    }
}
