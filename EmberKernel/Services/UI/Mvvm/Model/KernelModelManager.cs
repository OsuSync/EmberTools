using Autofac;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EmberKernel.Services.UI.Mvvm.Model
{
    public class KernelModelManager : IModelManager
    {
        private readonly Dictionary<string, INotifyPropertyChanged> instanceBinding = new Dictionary<string, INotifyPropertyChanged>();
        private readonly Dictionary<Type, string> typeBinding = new Dictionary<Type, string>();
        private readonly ILifetimeScope Scope;
        private readonly ILogger<IModelManager> Logger;

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChangeEvent(object sender, PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(sender, args);
            Logger.LogDebug($"Property changed: {args.PropertyName}");
        }

        public KernelModelManager(ILifetimeScope scope, ILogger<IModelManager> logger)
        {
            this.Scope = scope;
            this.Logger = logger;
        }

        private string GetFullName(Type type)
        {
            if (type.GetCustomAttribute<ModelAttribute>() is ModelAttribute attr)
            {
                return $"{type.Namespace}{attr.Id}";
            }
            return $"{type.Namespace}{type.Name}";
        }
        private string GetFullName<T>() => GetFullName(typeof(T));

        public void Register<T>(T instance) where T : INotifyPropertyChanged
        {
            var type = typeof(T);
            var fullName = GetFullName<T>();
            instanceBinding.Add(fullName, instance);
            typeBinding.Add(type, fullName);
            instance.PropertyChanged += RaisePropertyChangeEvent;
        }

        public void Unregister<T>() where T : INotifyPropertyChanged
        {
            var type = typeof(T);
            var fullName = GetFullName<T>();
            var instance = instanceBinding[fullName];
            instance.PropertyChanged -= RaisePropertyChangeEvent;
            instanceBinding.Remove(fullName);
            typeBinding.Remove(type);
        }
    }
}
