using Microsoft.Graph;
using Microsoft.Identity.Web;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserMicroservice.Models;

namespace GraphMicroservice.Services
{
    public class GraphService
    {
        private readonly GraphServiceClient _graphServiceClient;

        public GraphService(GraphServiceClient graphServiceClient)
        {
            _graphServiceClient = graphServiceClient;
        }
        public async Task<User> GetUserAsync(string userId)
        {
            try
            {
                // Get user details from Microsoft Graph API
                var user = await _graphServiceClient.Users[userId].Request().GetAsync();
                return user;
            }
            catch (Microsoft.Graph.ServiceException ex)
            {
                throw ex;
            }
        }
    }
}
