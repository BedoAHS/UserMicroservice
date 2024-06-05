using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GraphMicroservice.Services;

namespace GraphMicroservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GraphController : ControllerBase
    {
        private readonly GraphService _graphService;

        public GraphController(GraphService graphService)
        {
            _graphService = graphService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser(string userId)
        {
            try
            {
                var user = await _graphService.GetUserAsync(userId);
                return Ok(user);
            }
            catch (Microsoft.Graph.ServiceException ex)
            {
                return StatusCode(500, $"Error retrieving user: {ex.Message}");
            }
        }
    }
}
