using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Memory;


namespace TicketsGeneratorServices.Common.Extensions
{
    public static class MemoryCacheExtensions
    {
        private static readonly ConcurrentDictionary<string, object> _lockableObjects = new();



        public static async Task<TResult> GetOrLoad<TResult>(this IMemoryCache memoryCache, Func<Task<TResult>> loader, string? resultCacheKey = null, Func<TResult, bool>? successLoadPredicate = null, int loadTimeout = 30, int resultExpiration = 3600) where TResult : class
        {
            resultCacheKey ??= typeof(TResult).FullName ?? throw new ArgumentException($"Failed to generate cache key for type '{typeof(TResult).Name}'");
            var resultLoadTaskCacheKey = $"{resultCacheKey}__LOADING_TASK_SYSTEM_MARK";

            var result = memoryCache.Get<TResult>(resultCacheKey);
            if (result != null) // No need creation - data result is already in cache
                return result;

            var lockableObject = _lockableObjects.GetOrAdd(resultCacheKey, key => new());

            // Get working load task, or create one
            var isTaskCreated = false;
            Task<TaskExtensions.TimeoutTaskResult<TResult>>? loadTask;
            lock (lockableObject)
            {
                loadTask = memoryCache.Get<Task<TaskExtensions.TimeoutTaskResult<TResult>>>(resultLoadTaskCacheKey);

                if (loadTask == null)
                {
                    loadTask = loader()
                        .WithTimeout(loadTimeout * 1000); // ms

                    memoryCache.Set(resultLoadTaskCacheKey, loadTask, TimeSpan.FromSeconds(loadTimeout * 3));
                    isTaskCreated = true;
                }
            }

            // Wait for completing load task
            TaskExtensions.TimeoutTaskResult<TResult> loadTaskResult;
            try
            {
                loadTaskResult = await loadTask.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                lock (lockableObject)
                {
                    memoryCache.Remove(resultLoadTaskCacheKey);
                }
                throw new Exception("Failed to load info - unexpected exception", ex);
            }

            // Check load task for timeout
            if (loadTaskResult.IsTimeout)
            {
                lock (lockableObject)
                {
                    memoryCache.Remove(resultLoadTaskCacheKey);
                }
                throw new TimeoutException("Failed to load info - handling timeout");
            }

            // Get load task result + update cache if task created in this call and finished with success
            result = loadTaskResult.Task.Result;
            if (isTaskCreated)
            {
                lock (lockableObject)
                {
                    // If no success result - result not adding to cache and task is not removing from it.
                    // So, new calls wil get result from completed task, not from cached result (because it is missing)
                    // Task will be removed from cache after it's expiration time
                    // Then a new task will be created and new request will be handled - this is will be a retry attempt to get valid data
                    if (successLoadPredicate?.Invoke(result) ?? true)
                        memoryCache.Set(resultCacheKey, result, TimeSpan.FromSeconds(resultExpiration));

                    memoryCache.Remove(resultLoadTaskCacheKey);
                }
            }

            return result;
        }
    }
}