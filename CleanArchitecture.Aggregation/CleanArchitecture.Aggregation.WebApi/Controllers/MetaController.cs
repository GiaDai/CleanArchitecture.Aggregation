using CleanArchitecture.Aggregation.Infrastructure.Shared.Environments;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Configuration;
using System.Diagnostics;

namespace CleanArchitecture.Aggregation.WebApi.Controllers
{
    public class MetaController : BaseApiController
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly IDatabaseSettingsProvider _databaseSettingsProvider;
        public MetaController(
            IWebHostEnvironment env,
            IConfiguration config,
            IDatabaseSettingsProvider databaseSettingsProvider
            )
        {
            _env = env;
            _config = config;
            _databaseSettingsProvider = databaseSettingsProvider;
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
            var postgre = _databaseSettingsProvider.GetMySQLConnectionString();
            return Ok(postgre);
        }
    }
}
