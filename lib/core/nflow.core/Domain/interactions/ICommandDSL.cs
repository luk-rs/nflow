namespace nflow.core
{
    using System;

    public interface ICommandsDSL
    {
        void Send<TCommand>(TCommand whisper)
        where TCommand : ICommand;

        IObservable<TCommand> Handle<TCommand>()
        where TCommand : ICommand;
    }
}
