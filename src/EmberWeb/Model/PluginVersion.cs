using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmberWeb.Model
{
    public class PluginVersion
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Key]
        public Plugin Plugin { get; set; }
        [Required]
        public string Version { get; set; }
        [Required]
        public string DownloadUrl { get; set; }

    }
}
