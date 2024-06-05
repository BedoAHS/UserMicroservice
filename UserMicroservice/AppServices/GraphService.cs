using Microsoft.Graph.Users;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace UserMicroservice.AppServices
{
    public class GraphService
    {
        private readonly GraphServiceClient _graphClient;

        public GraphService(GraphServiceClient graphClient)
        {
            _graphClient = graphClient;
        }

        public async Task<User> GetGraphUserAsync(string userId) => await _graphClient.Users[userId].GetAsync();

        //public async Task UpdateGraphUserAsync(User user) => await _graphClient.Users[user.Id].Request().UpdateAsync(user);
    }
}
