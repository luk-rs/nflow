namespace nflow.core.Flow
{
    using System;

    public interface ICommandBus
    {
        IObservable<ICommand> CommandsSent { get; }

        void RouteCommand(ICommand command);
    }
}

