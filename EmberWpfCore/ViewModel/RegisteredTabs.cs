using EmberKernel.Plugins.Components;
using EmberKernel.Services.UI.Mvvm.ViewComponent;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace EmberWpfCore.ViewModel
{
    public class TabViewModel : TabItem
    {
        public override string ToString()
        {
            return Name;
        }
    }

    public class RegisteredTabs : ObservableCollection<TabViewModel>, IComponent
    {
        private IViewComponentManager ComponentManager { get; set; }
        public RegisteredTabs(IViewComponentManager manager)
        {
            this.ComponentManager = manager;
            manager.CollectionChanged += Manager_CollectionChanged;
            var tabTypes = manager.GetComponentType<ITabCategory>();
            //foreach (var type in tabTypes)
            //{
            //    this.Add(new TabViewModel()
            //    {
            //        Name = type.Name,
            //        Instance = Activator.CreateInstance(type) as UserControl,
            //    });
            //}
        }

        private void Manager_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    if (item.GetType().IsSameCategoryComponent<ITabCategory>())
                    {
                        this.Remove(this.First((model) => model.Content == e.OldItems[0]));
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems[0].GetType().IsSameCategoryComponent<ITabCategory>())
                {
                    foreach (var item in e.NewItems)
                    {
                        if (item.GetType().IsSameCategoryComponent<ITabCategory>())
                        {
                            this.Add(new TabViewModel()
                            {
                                Name = item.GetType().Name,
                                Header = item.GetType().Name,
                                Content = item as UserControl,
                            });
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            this.ClearItems();
        }
    }
}
