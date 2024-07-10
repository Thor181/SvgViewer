using HandyControl.Tools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SvgViewer.V2.Services;
using SvgViewer.V2.Utils;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;

namespace SvgViewer.V2
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; set; }

        public App()
        {
        }

        static App()
        {
            ServiceProvider = ConfigureServices();
        }

        private static IServiceProvider ConfigureServices()
        {
            var servicesCollection = new ServiceCollection();

            IConfiguration config = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                                                   .AddJsonFile("_appSettings.json")
                                                   .Build();

            servicesCollection.AddSingleton(_ => config);
            servicesCollection.AddSingleton<VersionService>();
            servicesCollection.AddSingleton<ClipboardService>();

            servicesCollection.AddKeyedSingleton<LastEntityService>(LastEntityServiceKeys.File, 
                (x, key) => new LastEntityService(Path.Combine("Configuration", "LastFiles.json")));
            servicesCollection.AddKeyedSingleton<LastEntityService>(LastEntityServiceKeys.Directory, 
                (x, key) => new LastEntityService(Path.Combine("Configuration", "LastDirectories.json")));

            servicesCollection.AddSingleton<ImageConverterService>();
            servicesCollection.AddSingleton<CacheService>();

            return servicesCollection.BuildServiceProvider();
        }

    }

}
