
namespace nflow.core
{
    using System.Collections.Generic;
    using System.Linq;

    internal record MicroHooks(IEnumerable<IHook> Oracles, IEnumerable<IHook> Whispers, IEnumerable<IHook> Instructions)
    {
        IEnumerable<IHook> All => Oracles.Concat(Whispers).Concat(Instructions);
        IEnumerable<IHook> Of(string @namespace) => All.Where(hook => hook.GetType().Namespace.StartsWith(@namespace));
        public IEnumerable<IHook> OraclesOf(string @namespace) => Oracles.Where(hook => hook.GetType().Namespace.StartsWith(@namespace));
        public IEnumerable<IHook> WhispersOf(string @namespace) => Whispers.Where(hook =>
        {
            var type = hook.GetType();
            return type.Namespace.StartsWith(@namespace);
        });
        public IEnumerable<IHook> InstructionsOf(string @namespace) => Instructions.Where(hook => hook.GetType().Namespace.StartsWith(@namespace));

    }
}