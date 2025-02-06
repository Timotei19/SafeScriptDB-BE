using Business_Logic_Layer.IUpdateScripts;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Models.Models;

namespace SafeScriptDb_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateScriptsController : ControllerBase
    {
        private readonly IServerService _serverService;

        public UpdateScriptsController(IServerService serverService)
        {
            _serverService = serverService;
        }

        [HttpPost("PostFolderPath")]
        [ProducesResponseType(200)]
        public IActionResult PostFolderPath([FromBody] string folderPath)
        {
            return Ok($"Received folder path: {folderPath}");
        }

        [HttpPost("ServerConnect")]
        [ProducesResponseType(typeof(List<string>), 200)]
        public IActionResult ServerConnect(DbServer credentials)
        {
            var databases = _serverService.GetDatabases(credentials);

            return Ok(databases);
        }

        [HttpPost("executeSqlOnTenants")]
        [ProducesResponseType(typeof(ScriptsResultModel), 200)]
        public async Task<IActionResult> ExecuteSqlOnTenants([FromForm] List<string> databases, [FromForm] List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
                return BadRequest("No files uploaded.");

            if (databases.Count == 0)
                return BadRequest("No databases selected.");


            var results = await _serverService.ExecuteSqlScripts(databases, files);

            return Ok(results);
        }
    }
}
