using System.Diagnostics;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using TicketsGeneratorServices.Common.Services.Base;
using TaskExtensions = TicketsGeneratorServices.Common.Extensions.TaskExtensions;


namespace TicketsGeneratorServices.Common.Services.PartnersService.Repository
{
    /// <summary>
    /// Сервис получения данных о магазинах
    /// </summary>
    public class PartnersApiService : ServiceBase, IPartnersService
    {
        #region Константы

        private const string _partnersMark = "ORSPartners";
        private const string _loadPartnersTaskMark = "LoadPartnersTask";
        private static readonly object _loadPartnersTaskLock = new();

        #endregion Константы


        #region Поля

        /// <summary>
        /// Клиент Partners API
        /// </summary>
        private readonly PartnersApiClient _partnersApiClient;

        /// <summary>
        /// Кеш для сохранения данных в памяти
        /// </summary>
        private readonly IMemoryCache _cache;

        #endregion Поля


        #region Конструктор

        public PartnersApiService(HttpClient httpClient, IMemoryCache cache, ILogger<PartnersApiService> logger, IMapper mapper, ActivitySource activitySource) : base(logger, mapper, activitySource)
        {
            _partnersApiClient = new PartnersApiClient(httpClient);
            _cache = cache;
        }

        #endregion Конструктор


        #region Методы

        public async Task<ICollection<PartnerOrsInfo>> GetPartnersInfo(ICollection<int>? partnersIdsFilter = null)
        {
            using var tracingActivity = Trace.StartActivity();

            try
            {
                var allPartners = await GetOrLoadAndSavePartners().ConfigureAwait(false);

                if (partnersIdsFilter == null || partnersIdsFilter.Count == 0)
                    return allPartners.Values;

                return partnersIdsFilter
                    .Where(i => i != 0 && allPartners.ContainsKey(i))
                    .Select(i => allPartners[i])
                    .ToList();
            }
            catch (Exception ex)
            {
                var message = "Failed to get partners info";
                var exception = new ScopedException(message, ex, nameof(IPartnersService));
                tracingActivity?.SetStatus(ActivityStatusCode.Error, description: exception.ToString());
                Log.LogError(exception, message);
                throw;
            }
        }


        public async Task<PartnerOrsInfo?> GetPartnerInfo(int partnerId)
        {
            using var tracingActivity = Trace.StartActivity();

            try
            {
                var allPartners = await GetOrLoadAndSavePartners().ConfigureAwait(false);

                allPartners.TryGetValue(partnerId, out var partner);
                return partner;
            }
            catch (Exception ex)
            {
                var message = "Failed to get partner info";
                var exception = new ScopedException(message, ex, nameof(IPartnersService));
                tracingActivity?.SetStatus(ActivityStatusCode.Error, description: exception.ToString());
                Log.LogError(exception, message);
                throw;
            }
        }

        #endregion Методы


        #region Методы - Система

        /// <summary>
        /// Получить все магазины из кэша
        /// <br/>Если данные по магазинам отсутствуют в кэше, то загрузить и добавить их туда, а после чего вернуть
        /// </summary>
        /// <returns>Данные по всем магазинам ОРС</returns>
        private async Task<IDictionary<int, PartnerOrsInfo>> GetOrLoadAndSavePartners()
        {
            var partners = _cache.Get<IDictionary<int, PartnerOrsInfo>>(_partnersMark);
            if (partners != null) // No need creation - data result is already in cache
                return partners;


            // Get working partners load task, or create one
            var isTaskCreated = false;
            Task<TaskExtensions.TimeoutTaskResult<LoadPartnersResult>>? loadPartnersTask;
            lock (_loadPartnersTaskLock)
            {
                loadPartnersTask = _cache.Get<Task<TaskExtensions.TimeoutTaskResult<LoadPartnersResult>>>(_loadPartnersTaskMark);

                if (loadPartnersTask == null)
                {
                    loadPartnersTask = LoadPartners()
                        .WithTimeout(20000);

                    _cache.Set(_loadPartnersTaskMark, loadPartnersTask, TimeSpan.FromMinutes(10)); // TODO: Expiration to config
                    isTaskCreated = true;
                }
            }

            // Wait for completing partners load task
            TaskExtensions.TimeoutTaskResult<LoadPartnersResult> loadPartnersTaskResult;
            try
            {
                loadPartnersTaskResult = await loadPartnersTask.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var message = "Failed to load partners info - unexpected exception";
                var exception = new ScopedException(message, ex, nameof(IPartnersService));
                Log.LogError(exception, message);

                _cache.Remove(_loadPartnersTaskMark);
                return new Dictionary<int, PartnerOrsInfo>(0);
            }

            // Check partners load task for timeout
            if (loadPartnersTaskResult.IsTimeout)
            {
                var message = "Failed to load partners info - handling timeout";
                var exception = new ScopedException(message, new TimeoutException(message), nameof(IPartnersService));
                Log.LogError(exception, message);

                _cache.Remove(_loadPartnersTaskMark);
                return new Dictionary<int, PartnerOrsInfo>(0);
            }

            // Get partners load task result + update cache if task created in this call and finished with success
            var result = loadPartnersTaskResult.Task.Result;
            if (isTaskCreated)
            {
                // If no success result - result not adding to cache and task is not removing from it.
                // So, new calls wil get result from completed task, not from cached result (because it is missing)
                // Task will be removed from cache after it's expiration time
                // Then a new task will be created and new request will be handled - this is will be a retry attempt to get valid data
                if (result is { IsPartnersBaseInfoLoaded: true })
                {
                    _cache.Set(_partnersMark, result, TimeSpan.FromHours(12)); // TODO: Expiration to config
                    _cache.Remove(_loadPartnersTaskMark);
                }
            }

            return result;
        }


        private async Task<LoadPartnersResult> LoadPartners()
        {
            var partnersBaseInfoTask = LoadPartnersInfoBase();
            //var partnersWebInfoTask = LoadPartnersInfoWeb(CancellationToken.None);

            await Task.WhenAll(
                partnersBaseInfoTask
            //partnersWebInfoTask
            ).ConfigureAwait(false);

            var partnersBaseInfo = partnersBaseInfoTask.Result;
            var partnersBaseInfoDict = new Dictionary<int, DTOPartnerFullInfo>(partnersBaseInfo.Count);
            foreach (var partnerBaseInfo in partnersBaseInfo)
                if (partnerBaseInfo.PtId.HasValue)
                    partnersBaseInfoDict[partnerBaseInfo.PtId.Value] = partnerBaseInfo;


            //var partnersWebInfo = partnersWebInfoTask.Result;
            //var partnersWebInfoDict = new Dictionary<int, PartnerWebInfo>(partnersWebInfo.Length);
            //foreach (var partnerWebInfo in partnersWebInfo)
            //    partnersWebInfoDict[partnerWebInfo.PartnerId] = partnerWebInfo;


            var result = new LoadPartnersResult(partnersBaseInfoDict.Count)
            {
                IsPartnersBaseInfoLoaded = partnersBaseInfoDict.Count != 0
                //IsPartnersWebInfoLoaded = partnersWebInfoDict.Count != 0
            };

            // From base info
            foreach (var partnerBaseInfoKvp in partnersBaseInfoDict)
            {
                if (result.ContainsKey(partnerBaseInfoKvp.Key))
                    continue;

                //partnersWebInfoDict.TryGetValue(partnerBaseInfo.PartnerId, out var partnerWebInfo);

                var partner = new PartnerOrsInfo()
                {
                    PartnerId = partnerBaseInfoKvp.Key,
                    InfoBase = partnerBaseInfoKvp.Value
                };
                result[partner.PartnerId] = partner;
            }


            // From web info
            //foreach (var partnerWebInfo in partnersWebInfoDict.Values)
            //{
            //    if (result.ContainsKey(partnerWebInfo.PartnerId))
            //        continue;

            //    partnersBaseInfoDict.TryGetValue(partnerWebInfo.PartnerId, out var partnerBaseInfo);

            //    var partner = new PartnerOrsInfo()
            //    {
            //        PartnerId = partnerWebInfo.PartnerId,
            //        InfoBase = partnerBaseInfo,
            //        InfoWeb = partnerWebInfo
            //    };
            //    result[partner.PartnerId] = partner;
            //}

            return result;
        }


        private async Task<ICollection<DTOPartnerFullInfo>> LoadPartnersInfoBase()
        {
            using var tracingActivity = Trace.StartActivity();

            try
            {
                var partners = await _partnersApiClient.ApiV3OrsExtpartnersAsync().ConfigureAwait(false);
                return partners;
            }
            catch (Exception ex)
            {
                var message = "Failed to load partners info from PartnersAPI";
                var exception = new ScopedException(message, new ScopedException(message, ex, nameof(PartnersApiClient)), nameof(IPartnersService));
                tracingActivity?.SetStatus(ActivityStatusCode.Error, description: exception.ToString());
                Log.LogError(exception, message);
                return [];
            }
        }


        //private async Task<PartnerWebInfo[]> LoadPartnersInfoWeb(CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        var jsonApiResponse = await _oldDeliveryService.GetPickpointsV2(cancellationToken).ConfigureAwait(false);
        //        if (!jsonApiResponse.HasResult)
        //        {
        //            var message = "Failed to retrieve main for partners from WEB's old-delivery service - no result";
        //            var exception = new ScopedException(
        //                message,
        //            new ScopedException(
        //                    message,
        //                    jsonApiResponse.Errors.ToException(),
        //                    nameof(IOldDeliveryService)
        //                ),
        //                nameof(IPartnerService)
        //            );
        //            _logger.LogError(exception, message);
        //            throw exception;
        //        }

        //        var records = _map.Map<PartnerWebInfo[]>(jsonApiResponse.Result);
        //        return records ?? [];
        //    }
        //    catch (Exception ex)
        //    {
        //        var message = "Failed to load partners info from WEB's old-delivery service";
        //        var exception = new ScopedException(message, new ScopedException(message, ex, nameof(IOldDeliveryService)), nameof(IPartnerService));
        //        _logger.LogError(exception, message);
        //        return [];
        //    }
        //}

        #endregion Методы - Система


        #region Внутренние объекты

        private class LoadPartnersResult : Dictionary<int, PartnerOrsInfo>
        {
            public LoadPartnersResult(int capacity) : base(capacity) { }


            public bool IsPartnersBaseInfoLoaded { get; init; }

            //public bool IsPartnersWebInfoLoaded { get; init; }
        }

        #endregion Внутренние объекты
    }
}