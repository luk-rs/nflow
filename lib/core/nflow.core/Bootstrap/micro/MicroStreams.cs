
namespace nflow.core
{
    using System.Collections.Generic;
    using System.Linq;

    internal record MicroStreams(IEnumerable<IStream> All)
    {

        public IEnumerable<IStream> Public => All.Where(stream => stream.IsPublic);
        IEnumerable<IStream> Private => All.Except(Public);
        public IEnumerable<IStream> Of(string @namespace) => All.Where(stream => stream.GetType().Namespace.StartsWith(@namespace));

    }
}