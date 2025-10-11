using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;


namespace TicketsGenerator.ServiceDefaults.Configuration.WakeUpWorker;


public class WakeUpWorkerHostedService : IHostedService, IDisposable
{

    #region Поля

    private readonly ILogger _log;
    private readonly ActivitySource _trace;
    private readonly Uri _targetUri;

    private Timer? _timer = null;
    private CancellationTokenSource? _cancellationTokenSource = null;

    #endregion Поля


    #region Конструкторы

    public WakeUpWorkerHostedService(ILogger<WakeUpWorkerHostedService> logger, ActivitySource activitySource, string targetUri, int? pingIntervalMilliseconds = null)
    {
        _log = logger;
        _trace = activitySource;
        _targetUri = new Uri(targetUri);

        if (pingIntervalMilliseconds != null)
            PingIntervalMilliseconds = pingIntervalMilliseconds.Value;
    }

    #endregion Конструкторы


    #region Свойства

    public int PingIntervalMilliseconds { get; set; } = 1 * 60 * 1000; // 1 minute

    #endregion Свойства


    #region Методы

    public void Dispose()
    {
        _timer?.Dispose();
    }


    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(PingTarget, null, 1000, PingIntervalMilliseconds);

        return Task.CompletedTask;
    }


    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(-1, -1);

        return _cancellationTokenSource?.CancelAsync() ?? Task.CompletedTask;
    }


    private async void PingTarget(object? state)
    {
        using var tracingActivity = _trace.StartActivity(name: "WakeUpTarget");

        try
        {
            _log.LogInformation("Trying to wake up target...");
            _cancellationTokenSource = new CancellationTokenSource();
            
            using var client = new HttpClient();
            using var response = await client.GetAsync(_targetUri, _cancellationTokenSource.Token).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            _log.LogInformation("Target is waked up");
        }
        catch (Exception ex)
        {
            var exception = new Exception("Failed wake up target", ex);
            tracingActivity?.SetStatus(ActivityStatusCode.Error, description: exception.ToString());
            _log.LogError(exception, "Failed to collect YMReturns storage data metrics");
        }
    }

    #endregion Методы
}
