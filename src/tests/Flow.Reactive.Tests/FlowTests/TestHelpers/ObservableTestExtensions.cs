namespace Flow.Reactive.Tests.FlowTests.TestHelpers
{
    using FluentAssertions;
    using Microsoft.Reactive.Testing;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive;

    public static class ObservableTestExtensions
    {
        public static void ShouldBe<T>(
            this IList<Recorded<Notification<T>>> actualMessages,
            params (long TimeStamp, T Value)[] expectedMessages)
        {
            actualMessages
                .Select(message => (message.Time, message.Value.Value))
                .Should()
                .BeEquivalentTo(expectedMessages);
        }
    }
}
