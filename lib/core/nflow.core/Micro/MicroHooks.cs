
namespace nflow.core
{
    using System.Collections.Generic;
    using System.Linq;

    //TODO delete me
    // internal record MicroHooks(IEnumerable<IStreamCarrier> Oracles, IEnumerable<IStreamCarrier> Whispers, IEnumerable<IStreamCarrier> Instructions)
    // {
    //     IEnumerable<IStreamCarrier> All => Oracles.Concat(Whispers).Concat(Instructions);
    //     IEnumerable<IStreamCarrier> Of(string @namespace) => All.Where(hook => hook.GetType().Namespace.StartsWith(@namespace));
    //     public IEnumerable<IStreamCarrier> OraclesOf(string @namespace) => Oracles.Where(hook => hook.GetType().Namespace.StartsWith(@namespace));
    //     public IEnumerable<IStreamCarrier> WhispersOf(string @namespace) => Whispers.Where(hook =>
    //     {
    //         var type = hook.GetType();
    //         return type.Namespace.StartsWith(@namespace);
    //     });
    //     public IEnumerable<IStreamCarrier> InstructionsOf(string @namespace) => Instructions.Where(hook => hook.GetType().Namespace.StartsWith(@namespace));

    // }
}