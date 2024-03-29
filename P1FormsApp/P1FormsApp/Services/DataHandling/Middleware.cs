﻿using Microsoft.Extensions.DependencyInjection;
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

        /// <summary>
        /// Calls the Authentication API to get a valid AuthToken for our session. This MUST be called prior to any other API calls.
        /// </summary>
        /// <param name="_FirmID">Generated ID for our Firm</param>
        /// <param name="_UserID">User ID that was generated for the SECCL API</param>
        /// <param name="_Password">Its a password for the account</param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns the average Current Value for a list of Portfolios.
        /// </summary>
        /// <param name="_Portfolios">Portfolios to get the average for.</param>
        /// <returns>Decimal value for the average</returns>
        public decimal GetFirmAverageCashValue(List<Portfolio.PorfolioAccount> _Portfolios)
        {
            decimal average = 0;

            for(int index = 0; index < _Portfolios.Count; index++)
            {
                average += _Portfolios[index].CurrentValue;
            }

            average = average / _Portfolios.Count;

            return average;
        }

        /// <summary>
        /// Returns the total value for all Positions for a Portfolio
        /// </summary>
        /// <param name="_PortfolioSummary">Portfolio which we want the total for</param>
        /// <returns>Returns decimal value for the total</returns>
        public decimal GetTotalPositionValue(Portfolio.PortfolioSummary _PortfolioSummary) 
        { 
            decimal total = 0;

            foreach(Portfolio.PortfolioPosition position in _PortfolioSummary.Positions)
            {
                total += position.CurrentValue;
            }

            return total;
        }

        /// <summary>
        /// Returns the total value for all Accounts for a Portfolio
        /// </summary>
        /// <param name="_PortfolioSummary">Portfolio which we want the total for</param>
        /// <returns>Returns decimal value for the total</returns>
        public decimal GetTotalAccountsValue(Portfolio.PortfolioSummary _PortfolioSummary)
        {
            decimal total = 0;

            foreach (Portfolio.SubAccount account in _PortfolioSummary.Accounts)
            {
                total += account.CurrentValue;
            }

            return total;
        }

        /// <summary>
        /// Loads all of the portfolios for a given FirmID. This gives portfolios at a header level.
        /// </summary>
        /// <param name="_FirmID">Firm ID that we would like to load a list of all of the portfolios for.</param>
        /// <param name="_AuthenticationToken">*REQUIRED* Valid Authentication Token to all the APIs to validate a correct session.</param>
        /// <returns></returns>
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

            // Getting 3 of the portfolios and returning them
            return accounts.Take(3).ToList();
        }


        /// <summary>
        /// Loads a specific portfolio account for a Firm. This gives the account in more detail.
        /// </summary>
        /// <param name="_FirmID">Firm ID associated with the portfolio account</param>
        /// <param name="_ID">ID for the specific portfolio account we want to load</param>
        /// <param name="_AuthenticationToken">*REQUIRED* Valid Authentication Token to all the APIs to validate a correct session.</param>
        /// <returns></returns>
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
