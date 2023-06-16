namespace Flow.Rx.Extensions.Schedulers
{

    using System.Reactive.Concurrency;


    public class SchedulerProvider : ISchedulerProvider
    {
        public SchedulerProvider(IScheduler dispatcher)
        {
            Dispatcher = dispatcher;
        }

        public IScheduler CurrentThread => Scheduler.CurrentThread;

        public IScheduler Dispatcher { get; }

        public IScheduler Immediate => Scheduler.Immediate;

        public IScheduler NewThread => NewThreadScheduler.Default;

        public IScheduler ThreadPool => Scheduler.Default;
    }
}