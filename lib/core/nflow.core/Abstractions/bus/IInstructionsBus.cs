namespace nflow.core
{
    using System;

    public interface IInstructionsBus<TCommand>
        where TCommand : ICommand
    {
        void CommandTo(Action<TCommand> command);
        IObservable<TCommand> Handle { get; }
    }
}

