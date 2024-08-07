﻿using HandyControl.Tools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SvgViewer.V2.Services;
using SvgViewer.V2.Services.Dialog;
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
            base.DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"Unhandled exception is occurred\n{e.Exception.Message}\n{e.Exception.StackTrace}");
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
            servicesCollection.AddScoped<IDialogService, DialogService>();


            var rootDirectory = Constants.Paths.RootDirectory;

            servicesCollection.AddKeyedSingleton<LastEntityService>(LastEntityServiceKeys.File, 
                (x, key) => new LastEntityService(Path.Combine(rootDirectory, "Configuration", "LastFiles.json")));
            servicesCollection.AddKeyedSingleton<LastEntityService>(LastEntityServiceKeys.Directory, 
                (x, key) => new LastEntityService(Path.Combine(rootDirectory, "Configuration", "LastDirectories.json")));
            servicesCollection.AddKeyedSingleton<LastEntityService>(LastEntityServiceKeys.Favorite,
                (x, key) => new LastEntityService(Path.Combine(rootDirectory, "Configuration", "FavoriteFiles.json")));

            servicesCollection.AddSingleton<ImageConverterService>();
            servicesCollection.AddSingleton<CacheService>();

            return servicesCollection.BuildServiceProvider();
        }

    }

}
