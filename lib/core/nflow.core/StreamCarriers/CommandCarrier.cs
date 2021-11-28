namespace nflow.core
{
	internal sealed class CommandCarrier<TCommand> : StreamCarrier<TCommand>
    where TCommand : ICommand
    {
    }
}

