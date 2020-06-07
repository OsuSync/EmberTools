using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EmberKernel.Services.UI.Mvvm.Model
{
    public interface IModelManager : INotifyPropertyChanged
    {
        /// <summary>
        /// Register Model Type to Model Manager
        /// <para>ModelManager will create a new singleton instance in <see cref="IModelManager"/>lifecycle</para>
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
