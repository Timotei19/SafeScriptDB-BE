using Business_Logic_Layer.Interfaces;
using Business_Logic_Layer.IUpdateScripts;
using Data_Access_Layer.Repositories;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Models.Entities;

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

        //[Authorize(Roles = "Admin")]
        [HttpGet("GetAllUsers")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(List<UserDTO>), 200)]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersWithRolesAsync();
            return Ok(users);
        }

        [HttpGet("GetUserRoles/{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(IEnumerable<Role>), 200)]
        public async Task<IActionResult> GetAllUserRoles(int userId)
        {
            var roles = await _userService.GetAllUserRolesAsync(userId);
            return Ok(roles);
        }

        [HttpDelete("DeleteUser/{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            await _userService.DeleteUserAsync(userId);

            return Ok(new { message = "User deleted successfully." });
        }

        [HttpGet("GetUserStatistics")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(IEnumerable<Role>), 200)]
        public async Task<IActionResult> GetUserStatistics([FromQuery] UserStatisticsRequest userStats)
        {
            var stats = await _userService.GetUserStatisticsAsync(userStats);

            return Ok(stats);
        }
    }
}
