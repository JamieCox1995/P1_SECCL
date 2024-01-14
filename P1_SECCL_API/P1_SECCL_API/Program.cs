using P1_SECCL_API.Classes;
using System.Text.Json.Nodes;

public class Program
{
    private const string _connectionString = "http://pfolio-api-staging.seccl.tech";
    private const string _firmID = "P1IMX";
    private const string _userID = "nelahi6642@4tmail.net";
    private const string _password = "DemoBDM1";

    static void Main(string[] args)
    {
        Authentication.AuthenticationToken token;
        token = SECCL.GetAuthenticationToken(_connectionString, _firmID, _userID, _password);

        if(token == null)
        {
            Console.WriteLine("Not authorised");
            return;
        }

        SECCL.GetFirmPortfolios(_connectionString, _firmID, token);
    }
}
