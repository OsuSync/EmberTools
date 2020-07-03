using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MultiplayerDownloader.Services.UI
{
    public class DownloadProviderListAttribute : Attribute
    {
        public string Name { get; set; }
    }

    public static class DownloadProviderListAttributeExtension
    {
        public static string GetProviderListDisplayName(this Type type)
        {
            if (type.GetCustomAttribute<DownloadProviderListAttribute>()
                is DownloadProviderListAttribute attr)
            {
                return attr.Name ?? type.Name;
            }
            return type.Name;
        }
        public static string GetProviderListDisplayName(this object obj)
        {
            if (obj.GetType().GetCustomAttribute<DownloadProviderListAttribute>()
                is DownloadProviderListAttribute attr)
            {
                return attr.Name ?? obj.GetType().Name;
            }
            return obj.GetType().Name;
        }
    }
}
