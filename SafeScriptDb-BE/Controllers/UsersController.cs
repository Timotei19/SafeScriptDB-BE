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
        [HttpGet("GetPagedUsers")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(List<UserDTO>), 200)]
        public async Task<IActionResult> GetPagedUsers([FromQuery] PagedUserRequest pagedRequest)
        {
            try
            {

            }catch(Exception ex)
            {
                return Ok();
            }
            var users = await _userService.GetPagedUsersWithRolesAsync(pagedRequest);
            return Ok(users);
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

        [HttpPut("UpdateUser/{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateUser([FromBody] UserDTO updateUserDto)
        {
            try
            {
            if (updateUserDto == null)
            {
                return BadRequest(new { message = "Invalid user data." });
            }

            var updatedUser = await _userService.UpdateUserAsync(updateUserDto);

            if (updatedUser == null)
            {
                return NotFound(new { message = "User not found." });
            }

            return Ok(updatedUser);
        }
            catch(Exception ex) 
            {
                throw new Exception($"User not found.");
    }

}
    }
}
