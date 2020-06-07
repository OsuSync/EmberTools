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
        private readonly Dictionary<string, object> instanceBinding = new Dictionary<string, object>();
        private readonly Dictionary<Type, string> typeBinding = new Dictionary<Type, string>();
        private readonly ILifetimeScope Scope;
        private readonly ILogger<IModelManager> Logger;

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChangeEvent(object sender, PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(sender, args);
            Logger.LogInformation($"Property changed: {args.PropertyName}");
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

        public DependencyObject<T> Register<T>(DependencyObject<T> instance) where T : class
        {
            var type = typeof(T);
            var fullName = GetFullName<T>();
            instanceBinding.Add(fullName, instance);
            typeBinding.Add(type, fullName);
            RaisePropertyChangeEvent(this, new PropertyChangedEventArgs(fullName));
            instance.PropertyChanged += RaisePropertyChangeEvent;
            return Current<T>();
        }

        public DependencyObject<T> Current<T>() where T : class
        {
            return (DependencyObject<T>)instanceBinding[typeBinding[typeof(T)]];
        }

        public void Unregister<T>() where T : class
        {
            var type = typeof(T);
            var fullName = GetFullName<T>();
            var instance = (DependencyObject<T>)instanceBinding[fullName];
            instance.PropertyChanged -= RaisePropertyChangeEvent;
            instanceBinding.Remove(fullName);
            typeBinding.Remove(type);
            PropertyChanged(this, new PropertyChangedEventArgs(fullName));
        }
    }
}
