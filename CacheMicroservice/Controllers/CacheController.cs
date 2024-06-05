// Controllers/CacheController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace CacheMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        private readonly IDistributedCache _cache;

        public CacheController(IDistributedCache cache)
        {
            _cache = cache;
        }

        [HttpGet("{key}")]
        public async Task<IActionResult> GetCache(string key)
        {
            try
            {
                var value = await _cache.GetStringAsync(key);
                if (value == null)
                    return NotFound();

                return Ok(value);
            }
            catch (RedisServerException ex)
            {
                return StatusCode(500, $"Redis error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("{key}")]
        public async Task<IActionResult> SetCache(string key, [FromBody] string value)
        {
            try
            {
                await _cache.SetStringAsync(key, value);
                return Ok();
            }
            catch (RedisServerException ex)
            {
                return StatusCode(500, $"Redis error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("{key}")]
        public async Task<IActionResult> DeleteCache(string key)
        {
            try
            {
                await _cache.RemoveAsync(key);
                return Ok();
            }
            catch (RedisServerException ex)
            {
                return StatusCode(500, $"Redis error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
