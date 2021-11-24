namespace nflow.core
{
    internal partial class BootstrapRegistry
    {
        abstract class StreamCarrierGenerator<TTargetStream> : IStreamCarrierGenerator<TTargetStream>
        where TTargetStream : IStream
        {
            //* still to implement
            public abstract IStreamCarrier CreateInstance<TStream>(object stream) where TStream : TTargetStream;


            //? default behavior
            bool IStreamCarrierGenerator.For(IStream stream) => Self.Handling(stream);
            public IStreamCarrier New<TStream>(TStream stream)
            where TStream : class, IStream
            => Self.CreateInstance<TTargetStream>(stream);

            private IStreamCarrierGenerator<TTargetStream> Self => ((IStreamCarrierGenerator<TTargetStream>)this);
        }
    }

}