namespace nflow.core
{
    using System;

    internal sealed class CommandCarrier<TCommand> : StreamCarrier<TCommand>
    where TCommand : ICommand
    {
    }
}

