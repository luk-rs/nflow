namespace nflow.core
{

    using System.Reflection;

    internal class Hook
    {
        public Assembly Assembly => _assembly ?? Assembly.GetEntryAssembly();

        public Hook(Assembly assembly)
        {
            _assembly = assembly;
        }
        private readonly Assembly _assembly;
    }
}