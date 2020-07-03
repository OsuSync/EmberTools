using EmberKernel.Plugins.Components;
using EmberKernel.Services.UI.Mvvm.ViewComponent;
using MultiplayerDownloader.Services.DownloadProvider;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace MultiplayerDownloader.Services.UI
{

    public class UIDownloadProdiver
    {
        public string Name { get; set; }
        public string Id { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            if (obj is UIDownloadProdiver provider)
            {
                return provider.Id == Id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
    public class DownloadProvidersViewModel : ObservableCollection<UIDownloadProdiver>, IComponent
    {
        public static readonly string DownloadProdiversCategory = $"MultiplayerDownloader.{nameof(IDownloadProvier)}";
        public DownloadProvidersViewModel(IViewComponentManager manager)
        {
            manager.CollectionChanged += Manager_CollectionChanged;
            var tabTypes = manager.GetComponentType<IDownloadProvier>();
            ProcessNewItems(manager);
        }

        private void ProcessNewItems(IEnumerable enumerator)
        {
            foreach (var item in enumerator)
            {
                if (item.GetType().IsSameCategoryComponent<IDownloadProvier>())
                {
                    this.Add(new UIDownloadProdiver()
                    {
                        Id = item.GetType().Name,
                        Name = item.GetProviderListDisplayName(),
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
                    if (item.GetType().IsSameCategoryComponent<IDownloadProvier>())
                    {
                        this.Remove(this.First((model) => model.Id == e.OldItems[0].GetType().Name));
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems[0].GetType().IsSameCategoryComponent<IDownloadProvier>())
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
