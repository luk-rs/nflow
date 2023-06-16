namespace Flow.Reactive.Playground
{
    using System;
    using System.Reflection;
    using ViewModels;
    using Wpf;
    using System.IO;

    public partial class App
    {

        public App()
        {
            ThisAssembly = typeof(App).Assembly;
            Host = new CaliburnHost<MainViewModel>(this,
                                                   ("Processing", ThisAssembly),
                                                   ("Reporting", ThisAssembly));

            InitializeLog();
        }

        private Assembly ThisAssembly { get; }
        private CaliburnHost<MainViewModel> Host { get; }

        private void InitializeLog()
        {
            var logFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Flow", "Playground", "Logs");

            if (!Directory.Exists(logFolder))
                Directory.CreateDirectory(logFolder);

            //var logConfiguration = DefaultLoggerConfiguration
            //    .CreateBaseConfiguration("1.0", "development", Guid.NewGuid().ToString())
            //    .ConfigureFileSync(logFolder)
            //    .CreateConfiguration();

            //Log.Initialize(new SerilogLoggerFactory(logConfiguration));
        }
    }

}