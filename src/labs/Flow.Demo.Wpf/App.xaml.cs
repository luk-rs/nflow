namespace Flow.Demo.Wpf
{
    using Flow.Demo.Wpf.ViewModels;
    using Flow.Wpf;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App() => _ = new CaliburnHost<MainViewModel>(this);
    }
}
