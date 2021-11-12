using Microsoft.Extensions.DependencyInjection;

namespace nflow.core
{

    public abstract class Registry : ServiceCollection
    {
        public string Namespace => GetType().Namespace;
    }
}

