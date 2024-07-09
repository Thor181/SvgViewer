using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace SvgViewer.V2
{
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; set; }

        public App()
        {
            ServiceProvider = ConfigureServices();
        }

        private static IServiceProvider ConfigureServices()
        {
            var servicesCollection = new ServiceCollection();

            var config = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                                                   .AddJsonFile("_appSettings.json")
                                                   .Build();

            servicesCollection.AddSingleton(_ => config);

            return servicesCollection.BuildServiceProvider();
        }

    }

}
