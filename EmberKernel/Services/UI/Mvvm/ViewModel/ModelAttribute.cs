using System;

namespace EmberKernel.Services.UI.Mvvm.ViewModel
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ModelAttribute : Attribute
    {
        public string Id { get; set; }
    }
}
