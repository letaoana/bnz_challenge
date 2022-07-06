using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Bnz.API
{
    public class BnzApiClient
    {
        internal static HttpClient ApiClient { get; set; }

        public static void InitializeBnzApiClient()
        {
            ApiClient = new HttpClient
            {
                BaseAddress = new Uri("https://jsonplaceholder.typicode.com/"),
            };
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}