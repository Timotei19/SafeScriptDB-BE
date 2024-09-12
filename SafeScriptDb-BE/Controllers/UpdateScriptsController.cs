using Business_Logic_Layer.IUpdateScripts;
using Microsoft.AspNetCore.Mvc;

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

      /*  [HttpGet("")]
        [ProducesResponseType(typeof(List<string>), 200)]
        public async Task<IActionResult> GetAllUsers()
        {
            var databases = new List<string>
            {
              //  new User { Id = 1, Name = "John Doe", Email = "johndoe@example.com" },
              //  new User { Id = 2, Name = "Jane Smith", Email = "janesmith@example.com" },
//new User { Id = 3, Name = "Alice Johnson", Email = "alice@example.com" }
            };

           // return Ok(users);
        }*/
    }
}
