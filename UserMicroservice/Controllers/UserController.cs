using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using UserMicroservice.AppServices;
using UserMicroservice.IAppServices;
using UserMicroservice.Models;

namespace UserMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly GraphService _graphService;

        public UserController(GraphService graphService,IUserService userService)
        {
            _userService = userService;
            _graphService = graphService;
        }

        //[HttpGet("graph/{userId}")]
        //public async Task<ActionResult<Microsoft.Graph.Models.User>> GetGraphUser(string userId)
        //{
        //    var user = await _graphService.GetGraphUserAsync(userId);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(user);
        //}

        //[HttpPut("graph/{userId}")]
        //public async Task<IActionResult> UpdateGraphUser(string userId, [FromBody] Microsoft.Graph.Models.User user)
        //{
        //    if (userId != user.Id)
        //    {
        //        return BadRequest();
        //    }

        //    await _graphService.UpdateGraphUserAsync(user);
        //    return NoContent();
        //}

        // GET: api/User/5

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userService.GetUserAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            var createdUser = await _userService.CreateUserAsync(user);
            return CreatedAtAction("GetUser", new { id = createdUser.Id }, createdUser);
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            var success = await _userService.UpdateUserAsync(id, user);
            if (!success)
            {
                return BadRequest();
            }
            return NoContent();
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var success = await _userService.DeleteUserAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }

}
