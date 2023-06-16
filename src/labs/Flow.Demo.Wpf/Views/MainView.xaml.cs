namespace Flow.Demo.Wpf.Views
{
    using System.Linq;
    using System.Windows;
    using Presentation;

    public partial class MainView
    {
        public MainView()
        {
            InitializeComponent();

            Application.Current.ApplyTheme(ThemeManager.Themes.Last());
        }
    }
}
