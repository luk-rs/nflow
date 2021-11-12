namespace nflow.core
{

    using System;
    using System.Linq;

    public interface IFlow
    {
        IOracles Oracles { get; }
        IWhisperers Whisperers { get; }
        ICommands Commands { get; }
    }

    internal class Flow : IFlow
    {

        IOracles IFlow.Oracles => throw new NotImplementedException();

        IWhisperers IFlow.Whisperers => throw new NotImplementedException();

        ICommands IFlow.Commands => throw new NotImplementedException();


        public Flow(IMicrosResolver micros)
        {
            _micros = micros.All.ToArray();
        }
        private readonly IMicro[] _micros;
    }
}