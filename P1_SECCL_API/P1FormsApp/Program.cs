using Microsoft.Extensions.DependencyInjection;
using Services.DataHandling;

namespace P1FormsApp
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var services = new ServiceCollection().AddHttpClient().AddTransient<IMiddleware, Middleware>().BuildServiceProvider();

            var apiService = services.GetRequiredService<IMiddleware>();

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new PortfolioViewer(apiService));
        }
    }
}