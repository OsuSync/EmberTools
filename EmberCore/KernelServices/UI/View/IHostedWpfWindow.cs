using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberCore.KernelServices.UI.View
{
    public interface IHostedWpfWindow
    {
        void Initialize(ILifetimeScope scope);
    }
}
