using EmberWeb.Model;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EmberWeb.Data
{
    public class EmberIdentityContext : ApiAuthorizationDbContext<EmberUser>
    {
        public EmberIdentityContext(DbContextOptions<EmberIdentityContext> options, IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }
    }
}
