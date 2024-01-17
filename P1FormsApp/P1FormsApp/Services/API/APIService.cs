using Classes;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Services.API
{
    public class APIService : IAPIService
    {
        private readonly HttpClient _client;

        public APIService(HttpClient client)
        {
            _client = client;
        }

        /// <summary>
        /// GET API call to the specified URL and endpoint, with a list of custom headers.
        /// </summary>
        /// <param name="_APIRoute">URL location of the API</param>
        /// <param name="_EndPoint">Endpoint we want to target our GET call to</param>
        /// <param name="_Headers">Custom Dictionary of headers we want added to the HTTP Request</param>
        /// <returns>Returns a generic API Response object, which contains a String that will contain the json response from the API call.</returns>
        public APIResponse GetResponse(string _APIRoute, string _EndPoint, Dictionary<string, string> _Headers)
        {
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            foreach (KeyValuePair<string, string> kvp in _Headers)
            {
                _client.DefaultRequestHeaders.Add(kvp.Key, kvp.Value);
            }

            var httpResponse = _client.GetAsync(new Uri($"{_APIRoute}/{_EndPoint}")).Result;

            string content = httpResponse.Content.ReadAsStringAsync().Result;

            APIResponse parsed = JsonConvert.DeserializeObject<APIResponse>(content);

            return parsed;
        }

        /// <summary>
        /// POST API call to the specified URL and endpoint. Passes through a string that contains the body of the request.
        /// </summary>
        /// <param name="_APIRoute">URL location of the API</param>
        /// <param name="_EndPoint">Endpoint we want to target our GET call to</param>
        /// <param name="_Body">String containing the formatted JSON to be passed through to the body of the request</param>
        /// <returns>Returns a generic API Response object, which contains a String that will contain the json response from the API call.</returns>
        public APIResponse PostResponse(string _APIRoute, string _EndPoint, string _Body)
        {
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Creating a http content we can then pass through to the PostAsync Method. We call the .Result to then force the application to wait for the result, rather than being async
            var httpContent = new StringContent(_Body, Encoding.UTF8, "application/json");

            var response = _client.PostAsync(new Uri($"{_APIRoute}/{_EndPoint}"), httpContent).Result;

            string content = response.Content.ReadAsStringAsync().Result;

            APIResponse parsed = JsonConvert.DeserializeObject<APIResponse>(content);

            return parsed;
        }
    }
}
