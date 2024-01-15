using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using P1_SECCL_API.Classes;
using Services.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Services.DataHandling
{
    public class Middleware : IMiddleware
    {
        private const string _route = "http://pfolio-api-staging.seccl.tech";

        public Authentication.AuthenticationToken AuthenticateSession(string _FirmID, string _UserID, string _Password)
        {
            // Constructing a JSON string from the above parameters which we can passthrough to the API Service.
            string jsonBody = "{ \"firmId\" : \"" + _FirmID + "\",    \"id\" : \"" + _UserID + "\",   \"password\" : \"" + _Password + "\"}";

            // Creating the Service Provider which can handle HttpClient
            var serviceProvider = new ServiceCollection()
            .AddHttpClient()
            .AddTransient<IAPIService, APIService>()
            .BuildServiceProvider();

            // Getting the Service we just created for the Interface
            var apiService = serviceProvider.GetRequiredService<IAPIService>();

            // Calling the Post Response with the connection string, endpoint, and constructed json body.
            var data = apiService.PostResponse(_route, "authenticate", jsonBody);

            // Deserializing the data
            Authentication.AuthData authData = JsonConvert.DeserializeObject<Authentication.AuthData>(data.Data.ToString());

            return new Authentication.AuthenticationToken(authData.Token, authData.UserName); ;
        }

        public List<Portfolio.PorfolioAccount> GetPortfoliosForFirm(string _FirmID, string _AuthenticationToken)
        {
            // Creating the Service Provider which can handle HttpClient
            var serviceProvider = new ServiceCollection()
            .AddHttpClient()
            .AddTransient<IAPIService, APIService>()
            .BuildServiceProvider();

            // Getting the Service we just created for the Interface
            var apiService = serviceProvider.GetRequiredService<IAPIService>();

            // Calling the Post Response with the connection string, endpoint, and constructed json body.
            var data = apiService.GetResponse(_route, $"portfolio/{_FirmID}", new Dictionary<string, string>
            {
                { "api-token", _AuthenticationToken }
            });

            List<Portfolio.PorfolioAccount> accounts = JsonConvert.DeserializeObject<List<Portfolio.PorfolioAccount>>(data.Data.ToString());

            return accounts;
        }

        public Portfolio.PortfolioSummary GetPortfoliosSummary(string _FirmID, string _ID, string _AuthenticationToken)
        {
            // Creating the Service Provider which can handle HttpClient
            var serviceProvider = new ServiceCollection()
            .AddHttpClient()
            .AddTransient<IAPIService, APIService>()
            .BuildServiceProvider();

            // Getting the Service we just created for the Interface
            var apiService = serviceProvider.GetRequiredService<IAPIService>();

            // Calling the Post Response with the connection string, endpoint, and constructed json body.
            var data = apiService.GetResponse(_route, $"portfolio/summary/{_FirmID}/{_ID}", new Dictionary<string, string>
            {
                { "api-token", _AuthenticationToken }
            });

            Portfolio.PortfolioSummary accounts = JsonConvert.DeserializeObject<Portfolio.PortfolioSummary>(data.Data.ToString());

            return accounts;
        }
    }
}
