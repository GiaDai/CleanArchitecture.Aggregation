using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace CleanArchitecture.Aggregation.WebApi.Controllers
{
    public class MetaController : BaseApiController
    {
        private readonly IWebHostEnvironment _env;
        public MetaController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet("/info")]
        public ActionResult<string> Info()
        {
            var assembly = typeof(Startup).Assembly;

            var lastUpdate = System.IO.File.GetLastWriteTime(assembly.Location);
            var version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

            return Ok($"Version: {version}, Last Updated: {lastUpdate}");
        }

        // Response Evniroment is production or development
        [HttpGet("/env")]
        public ActionResult<string> Env()
        {
            return Ok($"Environment is production: {_env.IsProduction()}");
        }
    }
}
