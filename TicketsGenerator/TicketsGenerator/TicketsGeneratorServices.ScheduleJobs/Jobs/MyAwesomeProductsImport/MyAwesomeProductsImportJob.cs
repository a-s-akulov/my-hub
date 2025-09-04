using System.Diagnostics;
using AutoMapper;
using Microsoft.Extensions.Options;
using Quartz;
using TicketsGeneratorServices.Common.Services.TicketsGeneratorStorageService;
using TicketsGeneratorServices.Db.Entities;
using TicketsGeneratorServices.ScheduleJobs.Jobs.Base;
using TicketsGeneratorServices.ScheduleJobs.Options;


namespace TicketsGeneratorServices.ScheduleJobs.Jobs
{
    public class MyAwesomeProductsImportJob : ScheduleJobBase<MyAwesomeProductsImportJob>
    {
        #region Поля

        public static readonly JobKey Key = new("my-awesome-products-import", "import-handlers");


        private readonly ITicketsGeneratorStorageService _storageService;

        #endregion Поля


        #region Конструкторы

        public MyAwesomeProductsImportJob(
                ITicketsGeneratorStorageService storageService,
                IOptions<AppOptions> appOptions, ILogger<MyAwesomeProductsImportJob> logger, IMapper mapper, ActivitySource activitySource) :
                    base(appOptions.Value.MyAwesomeProductsImportJob, logger, mapper, activitySource)
        {
            _storageService = storageService;
        }

        #endregion Конструкторы


        #region Свойства

        public override JobKey JobKey => Key;

        #endregion Свойства


        #region Методы

        protected override async Task ExecuteCore(IJobExecutionContext context)
        {
            //await Task.Delay(2_000); // KATTEST
            Log.LogInformation("Retrieving my awesome products from source (mockup)...");
            var sourceProducts = new List<MyAwesomeProduct> { new MyAwesomeProduct() };
            Log.LogInformation($"Received my awesome products for import from source: {sourceProducts.Count}");

            if (sourceProducts.Count == 0)
                return;

            foreach (var sourceProduct in sourceProducts)
            {
                try
                {
                    Log.LogInformation("Trying to import my awesome product with name " + $"'{sourceProduct.Name}' into storage...");
                    var storageProduct = sourceProduct; // TODO: Switch to Map.Map<MyAwesomeProduct>(sourceProduct);
                    var addOrUpdateResult = await _storageService.TryAddOrUpdate(storageProduct, cancellationToken: context.CancellationToken).ConfigureAwait(false);
                    storageProduct = addOrUpdateResult.Entity;


                    if (addOrUpdateResult.IsAdded)
                        Log.LogInformation("My awesome product with ID '{MyAwesomeProductId}' and name " + $"'{storageProduct.Name}' was added to storage", storageProduct.Id);
                    else if (addOrUpdateResult.IsUpdated)
                        Log.LogInformation("My awesome product with ID '{MyAwesomeProductId}' and name " + $"'{storageProduct.Name}' was updated in storage", storageProduct.Id);
                    else if (addOrUpdateResult.IsNoChanges)
                        Log.LogInformation("My awesome product with ID '{MyAwesomeProductId}' and name " + $"'{storageProduct.Name}' has no changes and was left in storage", storageProduct.Id);
                    else
                        Log.LogWarning("My awesome product with ID '{MyAwesomeProductId}' and name " + $"'{storageProduct.Name}' has unexpected import result!", storageProduct.Id);
                }
                catch (Exception ex)
                {
                    Log.LogError(
                        ex,
                        "Failed to import my awesome product with name " + $"'{sourceProduct.Name}' into storage! Product was skipped"
                    );
                }
            }
        }

        #endregion Методы
    }
}
