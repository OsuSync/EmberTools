using BeatmapDownloader.Abstract.Models;
using BeatmapDownloader.Abstract.Models.Events;
using BeatmapDownloader.Database.Database;
using BeatmapDownloader.Database.Model;
using EmberKernel.Services.EventBus.Handlers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using IComponent = EmberKernel.Plugins.Components.IComponent;

namespace BeatmapDownloader.WpfUI.UI.ViewModel
{
    public class DownloadHistoryViewModel : IComponent,
        INotifyPropertyChanged,
        IEventHandler<BeatmapDownloadTaskStarted>,
        IEventHandler<BeatmapDownloadTaskProgressUpdated>,
        IEventHandler<BeatmapDownloadTaskCompleted>
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private BeatmapDownloaderDatabaseContext Db { get; }
        public DownloadHistoryViewModel(
            BeatmapDownloaderDatabaseContext db,
            IOptions<BeatmapDownloaderConfiguration> config)
        {
            Db = db;
            RecentDownloaded = Db
            .DownloadedBeatmapSets
            .OrderByDescending(d => d.Id)
            .ToList();

            this.Source = config.Value.DownloadProvider.Name;
        }

        public List<DownloadBeatmapSet> RecentDownloaded { get; private set; }

        private bool DownloadFlag { get; set; } = false;
        public string CurrentStatus { get; private set; } = "Idle";
        public int Percentage { get; set; } = 0;
        public string Source { get; set; }

        private string GetCurrentStatus(BeatmapDownloadTaskProgressUpdated _)
        {
            if (DownloadFlag)
                return "Downloading...";
            else return "Idle";
        }

        public ValueTask Handle(BeatmapDownloadTaskStarted @event)
        {
            DownloadFlag = true;
            CurrentStatus = "Connecting";
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentStatus)));
            return default;
        }

        public ValueTask Handle(BeatmapDownloadTaskProgressUpdated @event)
        {
            if (Source != @event.Task.DownloadProviderName)
            {
                Source = @event.Task.DownloadProviderName;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Source)));
            }
            var status = GetCurrentStatus(@event);
            if (CurrentStatus != status)
            {
                CurrentStatus = status;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentStatus)));
            }
            Percentage = @event.PercentCompleted;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Percentage)));
            return default;
        }

        public async ValueTask Handle(BeatmapDownloadTaskCompleted @event)
        {
            RecentDownloaded = await Db
            .DownloadedBeatmapSets
            .OrderByDescending(d => d.Id)
            .ToListAsync();

            DownloadFlag = false;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RecentDownloaded)));

            CurrentStatus = "Completed";
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentStatus)));

            Percentage = 0;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Percentage)));
        }

        public void Dispose()
        {
        }
    }
}
