using nflow.core.Abstractions;

namespace nflow.core.tests.deps.Scan.Streams
{
    public class FooDepStream : IStream
    {
        public bool IsPublic => false;
    }
}