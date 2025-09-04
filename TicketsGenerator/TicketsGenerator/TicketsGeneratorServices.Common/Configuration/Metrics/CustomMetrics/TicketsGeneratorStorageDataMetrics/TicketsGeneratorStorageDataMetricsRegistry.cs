using App.Metrics;
using App.Metrics.Gauge;


namespace TicketsGeneratorServices.Common.Configuration.Metrics.CustomMetrics.TicketsGeneratorStorageDataMetrics
{
    public static class TicketsGeneratorStorageDataMetricsRegistry
    {
        private static readonly string _contextName = "TicketsGenerator storage data";




        public static readonly GaugeOptions MyAwesomeProductsAllCount = new GaugeOptions
        {
            Context = _contextName,
            Name = "My awesome products all count",
            MeasurementUnit = Unit.Items
        };


        public static readonly GaugeOptions MyAwesomeProductsBooksAllCount = new GaugeOptions
        {
            Context = _contextName,
            Name = "My awesome products food all count",
            MeasurementUnit = Unit.Items
        };


        public static readonly GaugeOptions MyAwesomeProductsFoodAllCount = new GaugeOptions
        {
            Context = _contextName,
            Name = "My awesome products food all count",
            MeasurementUnit = Unit.Items
        };


        public static readonly GaugeOptions MyAwesomeProductsCarsAllCount = new GaugeOptions
        {
            Context = _contextName,
            Name = "My awesome products cars all count",
            MeasurementUnit = Unit.Items
        };
    }
}
