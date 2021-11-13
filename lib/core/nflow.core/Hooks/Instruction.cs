namespace nflow.core
{
    using System;
    public interface IInstructionsBus<TCommand>
        where TCommand : ICommand
    {
        void CommandTo(Action<TCommand> command);
        IObservable<TCommand> Handle { get; }
    }

    internal sealed class Instruction<TCommand> : Hook<TCommand>, IInstructionsBus<TCommand>
    where TCommand : ICommand, new()
    {

        IObservable<TCommand> IInstructionsBus<TCommand>.Handle => Self.Socket;

        void IInstructionsBus<TCommand>.CommandTo(Action<TCommand> instruction)
        {

            var command = new TCommand();
            instruction(command);

            Self.Route(command);
        }
    }
}

