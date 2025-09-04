using System.Diagnostics;
using AutoMapper;
using Quartz;
using TicketsGeneratorServices.Common.Services.Base;
using TicketsGeneratorServices.ScheduleJobs.Options;


namespace TicketsGeneratorServices.ScheduleJobs.Jobs.Base
{
    /// <summary>
    /// Базовая Реализация джобы IInventoriesCreatingJob
    /// </summary>
    public abstract class ScheduleJobBase<TJob> : ServiceBase, IScheduleJob<TJob> where TJob : ScheduleJobBase<TJob>
    {
        #region Конструкторы

        protected ScheduleJobBase(ScheduleJobOptions jobOptions, ILogger<TJob> logger, IMapper mapper, ActivitySource activitySource) : base(logger, mapper, activitySource)
        {
            JobOptions = jobOptions;
        }

        #endregion Конструкторы


        #region Свойства

        /// <inheritdoc/>
        public abstract JobKey JobKey { get; }


        /// <summary>
        /// Параметры джобы
        /// </summary>
        protected ScheduleJobOptions JobOptions { get; }

        #endregion Свойства


        #region Методы

        /// <inheritdoc/>
        public async Task Execute(IJobExecutionContext context)
        {
            Log.StartJob(JobKey.Name, context.FireInstanceId);
            if (!JobOptions.EnableJob)
            {
                Log.CancelJobByConfiguration(JobKey.Name, context.FireInstanceId);
                return;
            }

            var startTime = DateTime.Now;
            using var tracingActivity = Trace.StartActivity(name: JobKey.Name);

            try
            {
                var checkResult = await CanExecute(context).ConfigureAwait(false);
                if (!checkResult)
                {
                    Log.CancelJobByCondition(JobKey.Name, tracingActivity?.Duration.TotalMilliseconds ?? (DateTime.Now - startTime).TotalMilliseconds, context.FireInstanceId);
                    return;
                }

                await ExecuteCore(context).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                tracingActivity?.Stop();
                var jobException = ex as ScheduleJobException ?? new ScheduleJobException(ex, JobKey.Name);

                Log.ErrorFinishJob(jobException, JobKey.Name, tracingActivity?.Duration.TotalMilliseconds ?? (DateTime.Now - startTime).TotalMilliseconds, context.FireInstanceId);
                tracingActivity?.SetStatus(ActivityStatusCode.Error, description: jobException.ToString());

                return;
            }

            tracingActivity?.Stop();
            Log.FinishJob(JobKey.Name, tracingActivity?.Duration.TotalMilliseconds ?? (DateTime.Now - startTime).TotalMilliseconds, context.FireInstanceId);
        }


        /// <summary>
        /// Проверка, может ли задание быть выполнено сейчас
        /// </summary>
        protected virtual Task<bool> CanExecute(IJobExecutionContext context)
        {
            return Task.FromResult(true);
        }


        /// <summary>
        /// Основной метод выполнения джобы для реализации
        /// </summary>
        protected abstract Task ExecuteCore(IJobExecutionContext context);

        #endregion Методы
    }
}
