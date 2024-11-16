using Business_Logic_Layer.IAuditModule;
using Business_Logic_Layer.Interfaces;
using Business_Logic_Layer.IUpdateScripts;
using Data_Access_Layer.Repositories;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Models.Entities;
using System.Diagnostics.Contracts;

namespace SafeScriptDb_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IServerService _serverService;
        private readonly IAuditService _auditService;
        private readonly IUserService _userService;

        public UsersController(IServerService serverService, IAuditService auditService, IUserService userService)
        {
            _serverService = serverService;
            _auditService = auditService;
            _userService = userService;
        }


        [HttpGet("GetAllUsers")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(List<UserDTO>), 200)]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersWithRolesAsync();
            return Ok(users);
        }

        [HttpPost("PostFolderPath")]
        [ProducesResponseType(200)]
        public IActionResult PostFolderPath([FromBody] string folderPath)
        {
            // Process the folder path (e.g., save to database, perform operations, etc.)
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

        [ProducesResponseType(typeof(List<string>), 200)]
        public async Task<IActionResult> ExecuteSqlOnTenants([FromForm] List<string> databases, [FromForm] List<IFormFile> files)//, [FromForm] List<string> tenantDatabases)
        {
            if (files == null || files.Count == 0)
                return BadRequest("No files uploaded.");

            if (databases.Count == 0)
                return BadRequest("No databases selected.");


            await _serverService.ExecuteSqlScripts(databases, files);

            return Ok();
        }

        [HttpGet("GetAudits")]
        [ProducesResponseType(typeof(List<Audit>), 200)]
        public async Task<IActionResult> GetAudits([FromQuery] PagedRequest pagedRequest)
        {
            var audits = await _auditService.GetPagedAudits(pagedRequest);
            return Ok(audits);

            //var audits = await _auditService.GetAllAudits();

            //return Ok(audits);
        }
    }
}
