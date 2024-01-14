using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace P1_SECCL_API.Classes
{
    public class SECCL
    {
        public static Authentication.AuthenticationToken GetAuthenticationToken(string _ConnectionString, string _FirmID, string _UserID, string _Password) 
        {
            Authentication.AuthenticationToken token = null;

            // Creating the HTTP Client to make the request and setting the base address for the API calls
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{_ConnectionString}/authenticate");

            // Now we are setting up the headers to let the API know that we are going ot pass through data as JSON
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Creating a rudementary json string for the request body which includes all of the data to authenticate the user
            string jsonBody = "{ \"firmId\" : \"P1IMX\",    \"id\" : \"nelahi6642@4tmail.net\",   \"password\" : \"DemoBDM1\"}";

            // Creating a http content we can then pass through to the PostAsync Method. We call the .Result to then force the application to wait for the result, rather than being async
            var httpContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var httpResponse = client.PostAsync(client.BaseAddress, httpContent).Result;

            // Now we want to check to see if our request was successful
            if(httpResponse.IsSuccessStatusCode)
            {
                //Getting the response and then converting it to a custom data struct so that we can easily extract the token.
                string c1 = httpResponse.Content.ReadAsStringAsync().Result;

                Authentication.AuthenticationObject result = JsonConvert.DeserializeObject<Authentication.AuthenticationObject>(c1);

                // Finally we are going to return our Authentication Token.
                token = new Authentication.AuthenticationToken(result.Data.Token, result.Data.UserName);
                return token;
            }

            return token;
        }


        public static List<Portfolio.PorfolioAccount> GetFirmPortfolios(string _ConnectionString, string _FirmID, Authentication.AuthenticationToken _AuthToken)
        {
            List<Portfolio.PorfolioAccount> portfolios = new List<Portfolio.PorfolioAccount>();

            // Creating the HTTP Client to make the request and setting the base address for the API calls
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{_ConnectionString}/portfolio/{_FirmID}");

            // Now we are setting up the headers to let the API know that we are going ot pass through data as JSON
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("api-token", _AuthToken.Token);


            var httpResponse = client.GetAsync(client.BaseAddress).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                string jsonResponse = httpResponse.Content.ReadAsStringAsync().Result;

                Portfolio.PortfolioObject pos = JsonConvert.DeserializeObject<Portfolio.PortfolioObject>(jsonResponse);

                foreach (Portfolio.PortfolioData data in pos.Data)
                {
                    Portfolio.PorfolioAccount account = new Portfolio.PorfolioAccount();

                    account.ID = data.ID;
                    account.FirmID = data.FirmID; 
                    account.Name = data.Name;
                    account.Status = data.Status;
                    account.Currency = data.Currency;
                    account.CurrentValue = data.CurrentValue;
                    account.Accounts = data.Accounts;
                    account.UninvestedCash = data.UninvestedCash;
                    account.Growth = data.Growth;
                    account.GrowthPercent = data.GrowthPercent;
                    account.AdjustedGrowth = data.AdjustedGrowth;
                    account.AdjustedGrowthPercent = data.AdjustedGrowthPercent;

                    portfolios.Add(account);
                }
            }

            return portfolios;
        }
    }
}

