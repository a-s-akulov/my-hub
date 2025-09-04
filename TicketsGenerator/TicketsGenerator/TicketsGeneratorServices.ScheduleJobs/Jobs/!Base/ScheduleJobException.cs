

namespace TicketsGeneratorServices.ScheduleJobs.Jobs.Base
{

    public class ScheduleJobException : ScopedException
    {
        public ScheduleJobException(Exception? exception) : base(exception, "ScheduleJob")
        { }

        public ScheduleJobException(string exceptionScope) : base(exceptionScope)
        { }

        public ScheduleJobException(Exception? exception, string exceptionScope) : base(exception, exceptionScope)
        { }

        public ScheduleJobException(string? message, Exception? exception, string exceptionScope) : base(message, exception, exceptionScope)
        { }
    }
}
