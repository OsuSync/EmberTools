using Autofac;
using EmberKernel.Services.UI.Mvvm.ViewComponent.Window;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace EmberKernel.Services.UI.Mvvm.ViewComponent
{
    public class KernelViewComponentManager : ObservableCollection<object>, IViewComponentManager
    {
        private Dictionary<string, LinkedList<Type>> CategoryComponents { get; } = new Dictionary<string, LinkedList<Type>>();
        private Dictionary<Type, ILifetimeScope> ComponentScopes { get; } = new Dictionary<Type, ILifetimeScope>();
        private Dictionary<Type, LinkedList<string>> ComponentCategories { get; } = new Dictionary<Type, LinkedList<string>>();
        private Dictionary<Type, object> Instance { get; } = new Dictionary<Type, object>();

        private IWindowManager WindowManager { get; }
        public KernelViewComponentManager(IWindowManager manager)
        {
            this.WindowManager = manager;
        }

        public async Task InitializeComponent(ILifetimeScope scope, Type type)
        {
            if (!type.IsAssignableTo<IViewComponent>())
            {
                throw new InvalidCastException();
            }
            ComponentScopes.Add(type, scope);
            await WindowManager.BeginUIThreadScope(async() =>
            {
                var instance = scope.Resolve(type) as IViewComponent;
                await instance.Initialize(ComponentScopes[type]);
                Instance.Add(type, instance);
                this.Add(instance);
            });
        }

        public async Task UninitializeComponent(ILifetimeScope scope, Type type)
        {
            if (!type.IsAssignableTo<IViewComponent>())
            {
                throw new InvalidCastException();
            }
            await WindowManager.BeginUIThreadScope(async () =>
            {
                var instance = scope.Resolve(type) as IViewComponent;
                await instance.Uninitialize(scope);
                foreach (var category in ComponentCategories[type])
                {
                    CategoryComponents[category].Remove(type);
                }
                ComponentScopes.Remove(type);
                Instance.Remove(type);
                this.Remove(instance);
            });
        }

        public IEnumerable<Type> GetComponentType<ICategory>()
        {
            var category = typeof(ICategory).GetViewComponentNamespace();
            if (!CategoryComponents.ContainsKey(category))
            {
                CategoryComponents.Add(category, new LinkedList<Type>());
            }
            return CategoryComponents[category];
        }

        public object GetComponent(Type type)
        {
            return Instance[type];
        }

        public void RegisterComponent<IComponent>(string category) where IComponent : IViewComponent, new()
        {
            if (!CategoryComponents.ContainsKey(category))
            {
                CategoryComponents.Add(category, new LinkedList<Type>());
            }
            if (!ComponentCategories.ContainsKey(typeof(IComponent)))
            {
                ComponentCategories.Add(typeof(IComponent), new LinkedList<string>());
            }
            CategoryComponents[category].AddLast(typeof(IComponent));
            ComponentCategories[typeof(IComponent)].AddLast(category);
        }
        public void RegisterComponent<ICategory, IComponent>() where IComponent : IViewComponent, new()
        {
            RegisterComponent<IComponent>(typeof(ICategory).GetViewComponentNamespace());
        }

        public void RegisterComponent<IComponent>() where IComponent : IViewComponent, new()
        {
            foreach (var @namespace in typeof(IComponent).GetAllViewComponentNamespace())
            {
                RegisterComponent<IComponent>(@namespace);
            }
        }
    }
}
