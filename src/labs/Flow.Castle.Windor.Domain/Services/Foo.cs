namespace Flow.Castle.Windsor.Domain.Services
{
    using System;


    public class Foo : IFoo
    {

        public int Next => new Random().Next(0, 100);

    }

}