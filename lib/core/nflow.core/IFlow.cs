namespace nflow.core
{

    using System;
    using System.Linq;

    public interface IFlow
    {
        IBus Bus { get; }
    }

    internal class Flow : IFlow
    {

        public IBus Bus { get; }
        public Flow(IMicrosResolver micros, FlowBus bus)
        {
            _micros = micros.All.ToArray();
            Bus = bus;
        }
        private readonly IMicro[] _micros;
    }
}