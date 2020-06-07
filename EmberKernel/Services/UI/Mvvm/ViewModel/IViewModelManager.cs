using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EmberKernel.Services.UI.Mvvm.ViewModel
{
    public interface IViewModelManager : INotifyPropertyChanged
    {
        /// <summary>
        /// Register Model Type to Model Manager
        /// <para>ViewModelManager will create a new singleton instance in <see cref="IViewModelManager"/>lifecycle</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        void Register<T>(T instance) where T : INotifyPropertyChanged;
        /// <summary>
        /// Unregister a Model in manager
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void Unregister<T>() where T : INotifyPropertyChanged;
    }
}
