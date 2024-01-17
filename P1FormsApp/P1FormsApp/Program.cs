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
            // Setting up the service for the Middleware dependency injection.
            var services = new ServiceCollection()
                .AddHttpClient()                                // Seeing as this is using a HTTP client to make API calls, we need to add it to the ServiceProvider. Otherwise it will crash.
                .AddTransient<IMiddleware, Middleware>().
                BuildServiceProvider();

            // Getting the service for our actual middleware implementation.
            var apiService = services.GetRequiredService<IMiddleware>();

            ApplicationConfiguration.Initialize();
            // Now we can pass it through to our Form, so that we can display all the magic.
            Application.Run(new PortfolioViewer(apiService));
        }
    }
}