using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmberWeb.Model
{
    public class Plugin
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public EmberUser CreateUser { get; set; }
        [Required]
        public string Name { get; set; }
        public string Author { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public string Description { get; set; }
        public string SourceUrl { get; set; }
        public bool Approved { get; set; }
        public string LatestVersion { get; set; }
        public string EmberVersion { get; set; }
        public bool Deleted { get; set; }
        [Required]
        public string DownloadUrl { get; set; }
        [InverseProperty("Plugin")]
        public List<PluginVersion> PluginVersions { get; set; }

    }
}
