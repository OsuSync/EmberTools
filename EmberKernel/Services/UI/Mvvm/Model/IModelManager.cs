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
        DependencyObject<T> Register<T>(DependencyObject<T> instance) where T : class;
        /// <summary>
        /// Unregister a Model in manager
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void Unregister<T>() where T : class;
        /// <summary>
        /// Get <typeparamref name="T"/> instance in <see cref="IModelManager"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        DependencyObject<T> Current<T>() where T : class;
    }
}
