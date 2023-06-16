namespace Flow.Reactive.TestingTools
{
    using AutoFixture;
    using AutoFixture.AutoNSubstitute;
    using AutoFixture.NUnit3;

    public class AutoMockDataAttribute : AutoDataAttribute
    {
        public AutoMockDataAttribute()
            : base(() => new CustomizedFixture())
        {
        }
    }

    public class CustomizedFixture : Fixture
    {
        public CustomizedFixture()
        {
            Customize(new AutoNSubstituteCustomization());
        }
    }
}
