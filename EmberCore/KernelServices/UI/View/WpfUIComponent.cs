using Autofac;
using EmberKernel.Plugins.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace EmberCore.KernelServices.UI.View
{
    public class WpfUIComponent<TWindow> : UIComponent
        where TWindow : Window, IHostedWpfWindow, new()
    {
        private TWindow window;
        private Application application;
        private ILifetimeScope scope;
        public WpfUIComponent(ILifetimeScope scope)
        {
            this.scope = scope;
        }
        public override bool Run()
        {
            application = new Application();
            window = new TWindow();
            window.Initialize(scope);
            application.Run(window);
            return true;
        }
        public override void Stop()
        {
            application.Shutdown();
        }
    }
}
