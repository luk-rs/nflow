using nflow.core.Abstractions;

namespace nflow.core.tests.deps.Streams
{
    public class FooDepStream : IStream
    {
        public bool IsPublic => false;
    }
}