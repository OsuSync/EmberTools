using EmberWeb.Model;
using EmberWeb.Services;
using EmberWeb.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmberWeb.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PluginsController : ControllerBase
    {
        private readonly IPluginContextService _pluginContextService;
        public PluginsController(IPluginContextService pluginsService)
        {
            this._pluginContextService = pluginsService;
        }
        
        [Route("all")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<Plugin>> GetAllPlugins()
        {
            return await _pluginContextService.SearchPlugins(limit: 50).Async();
        }

        [Route("my")]
        [HttpGet]
        public async Task<IEnumerable<Plugin>> MyPlugins()
        {
            return await _pluginContextService.SearchPlugins(limit: 50).Async();
        }
    }
}
