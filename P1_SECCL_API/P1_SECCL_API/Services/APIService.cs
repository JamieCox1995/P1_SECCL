using Classes;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Services
{
    public class APIService : IAPIService
    {
        private readonly HttpClient _client;

        public APIService(HttpClient client)
        {
            _client = client;
        }

        public APIResponse GetAPIResponse()
        {
            throw new NotImplementedException();
        }

        public APIResponse GetResponse(string _APIRoute, string _EndPoint)
        {
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Creating a rudementary json string for the request body which includes all of the data to authenticate the user
            string jsonBody = "{ \"firmId\" : \"P1IMX\",    \"id\" : \"nelahi6642@4tmail.net\",   \"password\" : \"DemoBDM1\"}";

            // Creating a http content we can then pass through to the PostAsync Method. We call the .Result to then force the application to wait for the result, rather than being async
            var httpContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = _client.PostAsync(new Uri($"{_APIRoute}/{_EndPoint}"), httpContent).Result;

            string content = response.Content.ReadAsStringAsync().Result;

            APIResponse parsed = JsonConvert.DeserializeObject<APIResponse>(content);

            return parsed;
        }


    }
}
