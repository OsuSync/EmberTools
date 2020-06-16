using EmberKernel.Plugins.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace EmberCore.KernelServices.UI.View
{
    public class WpfUIComponent<TWindow> : UIComponent
        where TWindow : Window, new()
    {
        private TWindow window;
        private Application application;

        public override bool Run()
        {
            application = new Application();
            window = new TWindow();
            application.Run(window);
            return true;
        }
        public override void Stop()
        {
            application.Shutdown();
        }
    }
}
