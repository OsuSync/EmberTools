using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmberWeb.Data
{
    public class EmberUser : IdentityUser
    {
        [Required]
        public string RegisterTime { get; set; }
    }
}
