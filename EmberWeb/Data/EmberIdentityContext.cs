using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmberWeb.Data
{
    public class EmberIdentityContext : IdentityDbContext<EmberUser>
    {
        public EmberIdentityContext(DbContextOptions<EmberIdentityContext> options) : base(options)
        {
        }
    }
}
