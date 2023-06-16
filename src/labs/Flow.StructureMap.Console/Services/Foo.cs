namespace Flow.StructureMap.Console.Services
{

    using System;


    internal class Foo : IFoo
    {

        public int Next => new Random().Next(0, 100);

    }

}