using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using TicketsGeneratorServices.Common.Options.Base;
using TicketsGeneratorServices.Common.Services.PartnersService;
using TicketsGeneratorServices.Common.Services.PartnersService.Repository;


namespace TicketsGeneratorServices.Common.Configuration
{
    /// <summary>
    /// Расширения для интеграции PartnersApiService в приложение
    /// </summary>
    public static class PartnersApiServiceConfigurationHostingExtensions
    {
        #region Методы

        public static IServiceCollection AddPartnersApiServiceInApp(this IServiceCollection services, ApiConnectionOptions apiConnectionOptions)
        {
            services.AddSingleton<IPartnersService, PartnersApiService>();
            services.AddHttpClient<IPartnersService, PartnersApiService>((serviceProvider, httpClient) =>
                {
                    var options = apiConnectionOptions;
                    httpClient.BaseAddress = new Uri(options.Host.EndsWith('/') ? options.Host.Trim() : options.Host.Trim() + "/");

                    if (!string.IsNullOrEmpty(options.ApiKey))
                        httpClient.DefaultRequestHeaders.Add("ApiKey", options.ApiKey);

                    httpClient.Timeout = TimeSpan.FromMinutes(30); // Handled by Polly, but no longer then 30 mins
                })
                .UseSocketsHttpHandler((handler, _) =>
                    handler.PooledConnectionLifetime = TimeSpan.FromMinutes(2)
                ) // Recreate connection every 2 minutes
                .SetHandlerLifetime(Timeout.InfiniteTimeSpan) // Disable rotation, as it is handled by PooledConnectionLifetime
                .AddPolicyHandler((serviceProvider, requestMessage) => GetOrCreateRetryPolicy(serviceProvider.GetRequiredService<IServiceScopeFactory>()));

            return services;
        }


        private static IAsyncPolicy<HttpResponseMessage>? _retryPolicy = null;
        private static IAsyncPolicy<HttpResponseMessage> GetOrCreateRetryPolicy(IServiceScopeFactory serviceFactory)
        {
            var serviceProvider = serviceFactory.CreateScope().ServiceProvider;

            return _retryPolicy ??= Policy.WrapAsync(
                    Policy.TimeoutAsync<HttpResponseMessage>(           // Global timeout - 30 seconds
                            30,
                            TimeoutStrategy.Optimistic,
                            onTimeoutAsync: (_, timespan, _) =>
                            {
                                serviceProvider
                                    .GetRequiredService<ILogger<IPartnersService>>()
                                    .LogWarning(
                                        $"Partners API request failed due to global timeout after {timespan.TotalSeconds}s.");

                                return Task.CompletedTask;
                            }
                    ),


                    HttpPolicyExtensions                                // Retry policy
                        .HandleTransientHttpError()
                        .Or<TimeoutRejectedException>() // For Polly timeouts handling
                        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                        .WaitAndRetryAsync(
                            1,                                         // 1 Retry
                            retryAttempt => TimeSpan.FromSeconds(Math.Pow(3, retryAttempt)),
                            onRetry: (_, timespan, retryAttempt, _) =>
                            {
                                serviceProvider
                                    .GetRequiredService<ILogger<IPartnersService>>()
                                    .LogWarning($"Partners API request failed. Delaying for {timespan.TotalSeconds}s, then making retry: {retryAttempt}.");
                            }),


                    Policy.TimeoutAsync<HttpResponseMessage>(           // Local timeout - 10 seconds
                        10,
                        TimeoutStrategy.Optimistic,
                        onTimeoutAsync: (_, timespan, _) =>
                        {
                            serviceProvider
                                .GetRequiredService<ILogger<IPartnersService>>()
                                .LogWarning(
                                    $"Partners API request failed due to local timeout after {timespan.TotalSeconds}s.");

                            return Task.CompletedTask;
                        }
                    )
                );
        }

        #endregion Методы
    }
}