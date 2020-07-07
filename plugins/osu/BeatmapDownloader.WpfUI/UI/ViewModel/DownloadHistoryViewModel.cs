using BeatmapDownloader.Abstract.Models;
using BeatmapDownloader.Database.Database;
using BeatmapDownloader.Database.Model;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.EventBus.Handlers;
using EmberKernel.Services.UI.Mvvm.ViewComponent.Window;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IComponent = EmberKernel.Plugins.Components.IComponent;

namespace BeatmapDownloader.WpfUI.UI.ViewModel
{
    public class DownloadHistoryViewModel : IComponent,
        INotifyPropertyChanged,
        IEventHandler<BeatmapDownloaded>,
        IEventHandler<DownloadingProcessChanged>
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private BeatmapDownloaderDatabaseContext Db { get; }
        private IWindowManager WindowManager { get; }
        public DownloadHistoryViewModel(BeatmapDownloaderDatabaseContext db, IWindowManager windowManager, IOptions<MpDownloaderConfiguration> config)
        {
            Db = db;
            WindowManager = windowManager;
            RecentDownloaded = Db
            .DownloadedBeatmapSets
            .OrderByDescending(d => d.Id)
            .ToList();

            this.Source = config.Value.DownloadProvider.Name;
        }

        public List<DownloadBeatmapSet> RecentDownloaded { get; private set; }

        public async ValueTask Handle(BeatmapDownloaded @event)
        {
            RecentDownloaded = await Db
            .DownloadedBeatmapSets
            .OrderByDescending(d => d.Id)
            .ToListAsync();

            await WindowManager.BeginUIThreadScope(() =>
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RecentDownloaded)));
            });

        }

        public string CurrentStatus { get; private set; } = "Idle";
        public int Percentage { get; set; } = 0;
        public string Source { get; set; }

        private string _getCurrentStatus(DownloadingProcessChanged @event)
        {
            if (@event.Idle) return "Idle";
            if (@event.SearchingBeatmap) return "Searching...";
            if (!@event.IsCompleted) return "Downloading...";
            return "Extracting...";
        }

        public ValueTask Handle(DownloadingProcessChanged @event)
        {
            if (Source != @event.ProviderName)
            {
                Source = @event.ProviderName;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Source)));
            }
            var status = _getCurrentStatus(@event);
            if (CurrentStatus != status)
            {
                CurrentStatus = status;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentStatus)));
            }
            Percentage = @event.Percentage;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Percentage)));
            return default;
        }

        public void Dispose()
        {
        }
    }
}
