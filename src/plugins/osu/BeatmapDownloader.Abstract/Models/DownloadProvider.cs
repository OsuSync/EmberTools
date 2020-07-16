using System;

namespace BeatmapDownloader.Abstract.Models
{
    public class DownloadProvider
    {
        public string Name { get; set; }
        public string Id { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            if (obj is DownloadProvider provider)
            {
                return provider.Id == Id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
