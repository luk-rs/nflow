namespace Flow.Reactive.Playground.MicroServices.Reporting.Commands
{

    using SharedKernel;
    using System.Collections.Generic;
    using Reactive.Streams.Ephemeral.Commands;


    public class ProcessIntegers : Command
    {
        public ProcessIntegers(IReadOnlyCollection<Integer> integers)
        {
            Integers = integers;
        }

        public IReadOnlyCollection<Integer> Integers { get; }
    }
}
