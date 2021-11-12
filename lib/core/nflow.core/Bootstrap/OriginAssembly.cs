namespace nflow.core
{

    using System.Reflection;

    internal class OriginAssembly
    {
        public Assembly Instance => _assembly ?? Assembly.GetEntryAssembly();

        public OriginAssembly(Assembly assembly)
        {
            _assembly = assembly;
        }
        private readonly Assembly _assembly;
    }
}