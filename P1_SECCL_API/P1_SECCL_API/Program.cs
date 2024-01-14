using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Nodes;
using P1_SECCL_API.Classes;
using Services.DataHandling;

public class Program
{
    private const string _firmID = "P1IMX";
    private const string _userID = "nelahi6642@4tmail.net";
    private const string _password = "DemoBDM1";

    static void Main(string[] args)
    {
        var services = new ServiceCollection().AddHttpClient().AddTransient<IMiddleware, Middleware>().BuildServiceProvider();

        var apiService = services.GetRequiredService<IMiddleware>();

        Authentication.AuthenticationToken token = apiService.AuthenticateSession(_firmID, _userID, _password);

        apiService.GetPortfoliosForFirm(_firmID, token.Token);
    }
}
