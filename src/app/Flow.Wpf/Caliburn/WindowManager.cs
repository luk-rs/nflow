using Caliburn.Micro;

namespace Flow.Wpf.Caliburn
{

    using System.Collections.Generic;
    using System.Threading;
    using System.Windows;


    public class WindowManager : global::Caliburn.Micro.WindowManager
    {

        protected override Window CreateWindow(object rootModel, bool isDialog, object context, IDictionary<string, object> settings)
        {
            var view = ViewLocator.LocateForModel(rootModel, default, context);
            ViewModelBinder.Bind(rootModel, view, context);
            var window = new Window
            {
                    //Top = 0d,
                    //Left = 0d,
                    //ResizeMode = ResizeMode.NoResize,
                    //WindowStyle = WindowStyle.SingleBorderWindow,
                    Content = view
            };
            var sync = SynchronizationContext.Current;

            window.Closed += (sender, args) => sync.Send(_ => Application.Current.Shutdown(-100), null);

            return window;
        }

    }

}