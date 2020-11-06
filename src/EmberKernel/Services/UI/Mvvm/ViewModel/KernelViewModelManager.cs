using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace EmberKernel.Services.UI.Mvvm.ViewModel
{
    public class KernelViewModelManager : IViewModelManager
    {
        private readonly Dictionary<string, INotifyPropertyChanged> instanceBinding = new Dictionary<string, INotifyPropertyChanged>();
        private readonly Dictionary<Type, string> typeBinding = new Dictionary<Type, string>();

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChangeEvent(object sender, PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(sender, args);
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
