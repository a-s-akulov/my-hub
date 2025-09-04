namespace TicketsGeneratorServices.Common.Extensions
{

    public static class TaskExtensions
    {
        public record TimeoutTaskResult<TResult>(Task<TResult> Task, bool IsTimeout);
        public static async Task<TimeoutTaskResult<TResult>> WithTimeout<TResult>(this Task<TResult> task, int timeout) where TResult : class
        {
            if (await Task.WhenAny(task, Task.Delay(timeout)) == task)
            {
                // Task completed within timeout.
                // Consider that the task may have faulted or been canceled.
                // We re-await the task so that any exceptions/cancellation is rethrown.

                return new TimeoutTaskResult<TResult>(task, false);

            }
            else
            {
                return new TimeoutTaskResult<TResult>(task, true);
            }
        }


        public static async Task<ResultOrException<T>> WrapResultOrException<T>(this Task<T> task)
        {
            try
            {
                return new ResultOrException<T>(await task);
            }
            catch (Exception ex)
            {
                return new ResultOrException<T>(ex);
            }
        }
    }


    public class ResultOrException<T>
    {
        public ResultOrException(T result)
        {
            IsSuccess = true;
            Result = result;
        }

        public ResultOrException(Exception ex)
        {
            IsSuccess = false;
            Exception = ex;
        }


        public bool IsSuccess { get; }

        public T Result { get; }

        public Exception Exception { get; }
    }
}
