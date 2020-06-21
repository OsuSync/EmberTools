using EmberKernel.Plugins.Components;
using EmberKernel.Services.UI.Mvvm.ViewComponent;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace EmberWpfCore.ViewModel
{
    public class RegisteredTabs : ObservableCollection<TabItem>, IComponent
    {
        public RegisteredTabs(IViewComponentManager manager)
        {
            manager.CollectionChanged += Manager_CollectionChanged;
            var tabTypes = manager.GetComponentType<ITabCategory>();
            ProcessNewItems(manager);
        }

        private void ProcessNewItems(IEnumerable enumerator)
        {
            foreach (var item in enumerator)
            {
                if (item.GetType().IsSameCategoryComponent<ITabCategory>())
                {
                    this.Add(new TabItem()
                    {
                        Name = item.GetType().Name,
                        Header = item.GetType().Name,
                        Content = item as UserControl,
                    });
                }
            }
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
                    ProcessNewItems(e.NewItems);
                }
            }
        }

        public void Dispose()
        {
        }
    }
}
