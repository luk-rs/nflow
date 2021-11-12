namespace nflow.core.Test.Nanos
{
    using nflow.core.Test.Services;

    internal class NanoZ : INano
    {
        private readonly Ii _ii;

        public NanoZ(Ii ii)
        {
            _ii = ii;
        }

    }
}