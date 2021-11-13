namespace nflow.core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IBus
    {
        IOracleBus<TOracle> Oracle<TOracle>() where TOracle : IOracle;
        IWhispersBus<TWhisper> Whisper<TWhisper>() where TWhisper : IWhisper;
        IInstructionsBus<TCommand> Instruction<TCommand>() where TCommand : ICommand;
    }

    internal sealed class MicroBus : IBus
    {
        IOracleBus<TOracle> IBus.Oracle<TOracle>()
        => Oracles.SingleOrDefault(hook => hook.Holding<TOracle>()) as IOracleBus<TOracle>;
        IWhispersBus<TWhisper> IBus.Whisper<TWhisper>()
        => Whispers.SingleOrDefault(hook => hook.Holding<TWhisper>()) as IWhispersBus<TWhisper>;
        IInstructionsBus<TCommand> IBus.Instruction<TCommand>()
        => Instructions.SingleOrDefault(hook => hook.Holding<TCommand>()) as IInstructionsBus<TCommand>;

        public MicroBus(Registry registry, IStreamsResolver streams)
        {
            Oracles = streams.Hooks.OraclesOf(registry.Namespace).ToArray();
            Whispers = streams.Hooks.OraclesOf(registry.Namespace).ToArray();
            Instructions = streams.Hooks.OraclesOf(registry.Namespace).ToArray();
        }

        private readonly IHook[] Oracles;
        private readonly IHook[] Whispers;
        private readonly IHook[] Instructions;

    }
    internal sealed class FlowBus : IBus
    {
        public IInstructionsBus<TCommand> Instruction<TCommand>() where TCommand : ICommand
        => Filter<TCommand>(_iHooks)
            .Cast<IInstructionsBus<TCommand>>()
            .Single();

        public IOracleBus<TOracle> Oracle<TOracle>() where TOracle : IOracle
        => Filter<TOracle>(_oHooks)
            .Cast<IOracleBus<TOracle>>()
            .Single();

        public IWhispersBus<TWhisper> Whisper<TWhisper>() where TWhisper : IWhisper
        => Filter<TWhisper>(_wHooks)
            .Cast<IWhispersBus<TWhisper>>()
            .Single();



        public FlowBus(IStreamsResolver streamsResolver)
        {
            var publicTypes = streamsResolver.Types.Public.Select(stream => stream.GetType()).ToArray();


            IHook[] resolve(Func<MicroHooks, IEnumerable<IHook>> selector, Func<Type, Type, bool> isPublic) =>
                        selector(streamsResolver.Hooks)
                        .Where(stream => publicTypes.Any(@public => isPublic(stream.GetType().GenericTypeArguments.Single(), @public)))
                        .ToArray();

            _oHooks = resolve(hooks => hooks.Oracles, (hook, @public) => hook.IsAssignableFrom(@public));
            _wHooks = resolve(hooks => hooks.Whispers, (hook, @public) => hook.IsAssignableFrom(@public));
            _iHooks = resolve(hooks => hooks.Instructions, (hook, @public) => true);
        }

        private readonly IHook[] _oHooks;
        private readonly IHook[] _wHooks;
        private readonly IHook[] _iHooks;
        private IEnumerable<IHook> Filter<TStream>(IHook[] hooks) where TStream : IStream
        => from _hook in hooks
           let hook = _hook.GetType().GenericTypeArguments.Single()
           let request = typeof(TStream)
           where hook.IsAssignableFrom(request)
           select _hook;

    }
}
