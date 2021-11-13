namespace nflow.core
{

    using System;
    using System.Linq;

    public interface IFlow
    {
        // IOracles Oracles { get; }
        // IWhispers Whisperers { get; }
        // IInstructionsBus Commands { get; }
        IBus Bus { get; }
    }

    internal class Flow : IFlow
    {

        // IOracles IFlow.Oracles => throw new NotImplementedException();

        // IWhispers IFlow.Whisperers => throw new NotImplementedException();

        // IInstructionsBus IFlow.Commands => throw new NotImplementedException();

        public IBus Bus { get; }
        public Flow(IMicrosResolver micros, FlowBus bus)
        {
            _micros = micros.All.ToArray();
            Bus = bus;
        }
        private readonly IMicro[] _micros;
    }
}