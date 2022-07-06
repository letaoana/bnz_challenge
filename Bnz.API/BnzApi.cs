using Bnz.API.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Bnz.API
{
    public class BnzApi
    {
        public static async Task<HttpResponseMessage> GetUsers()
        {
            var response = await BnzApiClient.ApiClient.GetAsync("/users");
            return response;
        }

        public static async Task<HttpResponseMessage> GetUser(int userId)
        {
            var response = await BnzApiClient.ApiClient.GetAsync($"/users/{userId}");
            return response;
        }

        public static async Task<HttpResponseMessage> CreateUser(User user)
        {
            var requestContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var response = await BnzApiClient.ApiClient.PostAsync("/users", requestContent);
            return response;
        }
    }
}