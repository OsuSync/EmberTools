using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace EmberCore.KernelServices.UI.View
{
    public interface ICoreWpfPlugin
    {
        void BuildApplication(Application application);
    }
}
