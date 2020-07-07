using System;
using System.Reflection;

namespace BeatmapDownloader.Abstract.Services.UI
{
    public class DownloadProviderListAttribute : Attribute
    {
        public string Name { get; set; }
    }

    public static class DownloadProviderListAttributeExtension
    {
        public static string GetProviderListDisplayName(this Type type)
        {
            foreach (var attribute in type.GetCustomAttributes())
            {
                if (attribute.GetType().FullName == typeof(DownloadProviderListAttribute).FullName)
                {
                    var prop = attribute.GetType().GetProperty(nameof(DownloadProviderListAttribute.Name));
                    return prop.GetValue(attribute)?.ToString() ?? type.Name;
                }
            }
            return type.Name;
        }
        public static string GetProviderListDisplayName(this object obj)
        {
            return GetProviderListDisplayName(obj.GetType());
        }
    }
}
