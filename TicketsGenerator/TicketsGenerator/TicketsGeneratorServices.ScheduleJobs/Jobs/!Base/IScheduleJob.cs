using Quartz;


namespace TicketsGeneratorServices.ScheduleJobs.Jobs.Base
{
    public interface IScheduleJob<TJob> : IJob where TJob : IScheduleJob<TJob>
    {
        /// <summary>
        /// Идентификатор типа джобы
        /// </summary>
        public JobKey JobKey { get; }
    }
}