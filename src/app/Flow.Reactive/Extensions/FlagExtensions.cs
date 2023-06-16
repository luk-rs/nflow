using System.Reflection;

namespace Flow.Reactive.Extensions
{
    internal static class FlagExtensions
    {
        public static bool HasFlag(this TypeAttributes source, TypeAttributes target) => (source & target) == target;
        public static bool DoesntHaveFlag(this TypeAttributes source, TypeAttributes target) => !HasFlag(source, target);

    }
}
