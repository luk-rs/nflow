namespace nflow.core
{
    using System;
    using System.Reactive.Subjects;

    internal sealed class OracleCarrier<TOracle> : StreamCarrier<TOracle>
    where TOracle : IOracle
    {
    }
}

