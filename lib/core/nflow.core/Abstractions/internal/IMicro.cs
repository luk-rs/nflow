

namespace nflow.core
{
    internal interface IMicro
    {
        string Name => Registry.GetType().Name;
        string Namespace => Registry.Namespace;
        Registry Registry { get; }
        IBus Bus { get; }

    }
}