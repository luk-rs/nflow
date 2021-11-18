namespace nflow.core
{
    public interface IBus
    {
        IOracleBus<TOracle> Oracle<TOracle>() where TOracle : IOracle;
        IWhispersBus<TWhisper> Whisper<TWhisper>() where TWhisper : IWhisper;
        IInstructionsBus<TCommand> Instruction<TCommand>() where TCommand : ICommand;
    }
}
