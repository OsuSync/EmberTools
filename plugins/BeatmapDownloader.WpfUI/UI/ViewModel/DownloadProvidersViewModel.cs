using EmberKernel.Plugins.Components;
using EmberKernel.Services.UI.Mvvm.ViewComponent;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using BeatmapDownloader.Abstract.Models;
using BeatmapDownloader.Abstract.Services.DownloadProvider;
using BeatmapDownloader.Abstract.Services.UI;

namespace BeatmapDownloader.WpfUI.UI.ViewModel
{
    public class DownloadProvidersViewModel : ObservableCollection<DownloadProvider>, IComponent
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
                    this.Add(new DownloadProvider()
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
