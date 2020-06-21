using Autofac;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace EmberKernel.Services.UI.Mvvm.ViewComponent
{
    public interface IViewComponentManager : INotifyPropertyChanged, INotifyCollectionChanged, ICollection<object>
    {
        void RegisterComponent<ICategory, IComponent>() where IComponent : IViewComponent, new();
        void RegisterComponent<IComponent>(string category) where IComponent : IViewComponent, new();
        void RegisterComponent<IComponent>() where IComponent : IViewComponent, new();
        IEnumerable<Type> GetComponentType<ICategory>();
        Task InitializeComponent(ILifetimeScope scope, Type type);
        Task UninitializeComponent(ILifetimeScope scope, Type type);
    }
}
