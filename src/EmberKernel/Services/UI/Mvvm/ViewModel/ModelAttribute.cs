using System;
using System.Collections.Generic;
using System.Text;

namespace EmberKernel.Services.UI.Mvvm.ViewModel
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ModelAttribute : Attribute
    {
        public string Id { get; set; }
    }
}
