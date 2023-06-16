namespace Flow.Rx.Extensions.Schedulers
{

    using System.Reactive.Concurrency;


    public interface ISchedulerProvider
    {
        /// <summary>
        ///     Gets a scheduler that schedules work as soon as possible on the current thread.
        /// </summary>
        IScheduler CurrentThread { get; }

        /// <summary>
        ///     Gets the scheduler that schedules units of work on the Dispatcher of the current application.
        /// </summary>
        IScheduler Dispatcher { get; }

        /// <summary>
        ///     Gets a scheduler that schedules work immediately on the current thread.
        /// </summary>
        IScheduler Immediate { get; }

        /// <summary>
        ///     Gets an instance of this scheduler that uses the default Thread constructor.
        /// </summary>
        IScheduler NewThread { get; }

        /// <summary>
        ///     Gets a scheduler that schedules work on the platform's default scheduler.
        /// </summary>
        IScheduler ThreadPool { get; }
    }
}