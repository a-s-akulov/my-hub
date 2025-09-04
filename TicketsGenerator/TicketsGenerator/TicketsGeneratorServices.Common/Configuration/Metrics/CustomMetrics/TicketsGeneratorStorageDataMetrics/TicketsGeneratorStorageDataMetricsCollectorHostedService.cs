using System.Diagnostics;
using App.Metrics;
using AutoMapper;
using Microsoft.Extensions.Options;
using TicketsGeneratorServices.Common.Services.Base;
using TicketsGeneratorServices.Common.Services.TicketsGeneratorStorageService;


namespace TicketsGeneratorServices.Common.Configuration.Metrics.CustomMetrics.TicketsGeneratorStorageDataMetrics
{
    public class TicketsGeneratorStorageDataMetricsCollectorHostedService : ServiceBase, IHostedService, IDisposable
    {

        #region Поля

        private readonly IMetrics _metrics;
        private readonly TicketsGeneratorStorageDataMetricsCollectorOptions _options;
        private readonly ITicketsGeneratorStorageService _storageService;

        private Timer? _timer = null;
        private CancellationTokenSource? _cancellationTokenSource = null;

        #endregion Поля


        #region Конструкторы

        public TicketsGeneratorStorageDataMetricsCollectorHostedService(ITicketsGeneratorStorageService storageService, IOptions<TicketsGeneratorStorageDataMetricsCollectorOptions> options, IMetrics metrics, ILogger<TicketsGeneratorStorageDataMetricsCollectorHostedService> logger, IMapper mapper, ActivitySource activitySource) : base(logger, mapper, activitySource)
        {
            _storageService = storageService;
            _options = options.Value;
            _metrics = metrics;
        }

        #endregion Конструкторы


        #region Свойства



        #endregion Свойства


        #region Методы

        public void Dispose()
        {
            _timer?.Dispose();
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(CollectData, null, 1000, _options.CollectIntervalMilliseconds);

            return Task.CompletedTask;
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(-1, -1);

            return _cancellationTokenSource?.CancelAsync() ?? Task.CompletedTask;
        }


        private async void CollectData(object? state)
        {
            using var tracingActivity = Trace.StartActivity(name: "CollectTicketsGeneratorStorageDataMetrics");

            try
            {
                _cancellationTokenSource = new CancellationTokenSource();
                var storageMetrics = await _storageService.GetStorageDataMetrics(cancellationToken: _cancellationTokenSource.Token).ConfigureAwait(false);

                _metrics.Measure.Gauge.SetValue(TicketsGeneratorStorageDataMetricsRegistry.MyAwesomeProductsAllCount, storageMetrics.MyAwesomeProductsAllCount);
                _metrics.Measure.Gauge.SetValue(TicketsGeneratorStorageDataMetricsRegistry.MyAwesomeProductsBooksAllCount, storageMetrics.MyAwesomeProductsBooksAllCount);
                _metrics.Measure.Gauge.SetValue(TicketsGeneratorStorageDataMetricsRegistry.MyAwesomeProductsFoodAllCount, storageMetrics.MyAwesomeProductsFoodAllCount);
                _metrics.Measure.Gauge.SetValue(TicketsGeneratorStorageDataMetricsRegistry.MyAwesomeProductsCarsAllCount, storageMetrics.MyAwesomeProductsCarsAllCount);
            }
            catch (Exception ex)
            {
                var exception = new ScopedException("Failed to collect TicketsGenerator storage data metrics", ex, nameof(TicketsGeneratorStorageDataMetricsCollectorHostedService));
                tracingActivity?.SetStatus(ActivityStatusCode.Error, description: exception.ToString());
                Log.LogError(exception, "Failed to collect TicketsGenerator storage data metrics");
            }
        }

        #endregion Методы
    }



    public class TicketsGeneratorStorageDataMetricsCollectorOptions
    {
        /// <summary>
        /// Collect interval in milliseconds
        /// </summary>
        public int CollectIntervalMilliseconds { get; set; } = 10 * 60 * 1000; // 10 минут
    }
}