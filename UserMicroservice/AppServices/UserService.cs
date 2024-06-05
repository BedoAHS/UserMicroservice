using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using UserMicroservice.IAppServices;
using UserMicroservice.Models;

namespace UserMicroservice.AppServices
{
    // Services/UserService.cs
    public class UserService : IUserService
    {
        private readonly UserContext _context;
        private readonly IDistributedCache _cache;

        public UserService(UserContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<User> GetUserAsync(int id)
        {
            string cacheKey = $"User_{id}";
            string cachedUser = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedUser))
            {
                return JsonConvert.DeserializeObject<User>(cachedUser);
            }

            var user = await _context.Users.FindAsync(id);

            if (user != null)
            {
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };
                await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(user), cacheOptions);
            }

            return user;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            string cacheKey = "AllUsers";
            string cachedUsers = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedUsers))
            {
                return JsonConvert.DeserializeObject<IEnumerable<User>>(cachedUsers);
            }

            var users = await _context.Users.ToListAsync();

            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };
            await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(users), cacheOptions);

            return users;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Invalidate the cache for the list of users
            await _cache.RemoveAsync("AllUsers");

            return user;
        }

        public async Task<bool> UpdateUserAsync(int id, User user)
        {
            if (id != user.Id)
            {
                return false;
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                // Invalidate the cache for this user and the list of users
                await _cache.RemoveAsync($"User_{id}");
                await _cache.RemoveAsync("AllUsers");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }

            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            // Invalidate the cache for this user and the list of users
            await _cache.RemoveAsync($"User_{id}");
            await _cache.RemoveAsync("AllUsers");

            return true;
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }


}
