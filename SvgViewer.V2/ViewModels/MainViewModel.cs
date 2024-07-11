using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Tools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Svg;
using SvgViewer.V2.Models;
using SvgViewer.V2.Services;
using SvgViewer.V2.Utils;
using SvgViewer.V2.Utils.Growl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace SvgViewer.V2.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly VersionService _versionService;
        private readonly ClipboardService _clipboardService;
        private readonly LastEntityService _lastDirectoriesService;
        private readonly LastEntityService _lastFilesService;
        private readonly ImageConverterService _imageConverterService;
        private readonly CacheService _cacheService;

        public ObservableCollection<VisualCard> Cards { get; set; } = [];

        private ObservableLinkedList<VisualCard> _lastFilesCards = new();
        public ObservableLinkedList<VisualCard> LastFilesCards { get => _lastFilesCards; set => SetProperty(ref _lastFilesCards, value); }

        private string[] _lastDirectories = [];
        public string[] LastDirectories { get => _lastDirectoriesService.Load(); set => SetProperty(ref _lastDirectories, value); }

        public ICommand ClickCommand { get; set; }
        public ICommand DirectoryInputCommand { get; set; }
        public ICommand SearchInputCommand { get; set; }

        public string Version { get => _versionService.Version ?? string.Empty; }

        private bool _subfolders = true;
        public bool Subfolders { get => _subfolders; set => SetProperty(ref _subfolders, value); }

        private bool _isLoading = false;
        public bool IsLoading { get => _isLoading; set => SetProperty(ref _isLoading, value); }

        private double _progress = 0;
        public double Progress { get => _progress; set => SetProperty(ref _progress, value); }

        private bool _cacheEnabled = false;
        private int _maxCountLastFiles = 0;

        public MainViewModel()
        {
            _versionService = App.ServiceProvider.GetRequiredService<VersionService>();
            _clipboardService = App.ServiceProvider.GetRequiredService<ClipboardService>();
            _lastDirectoriesService = App.ServiceProvider.GetRequiredKeyedService<LastEntityService>(LastEntityServiceKeys.Directory);
            _lastFilesService = App.ServiceProvider.GetRequiredKeyedService<LastEntityService>(LastEntityServiceKeys.File);
            _imageConverterService = App.ServiceProvider.GetRequiredService<ImageConverterService>();
            _cacheService = App.ServiceProvider.GetRequiredService<CacheService>();

            var configuration = App.ServiceProvider.GetRequiredService<IConfiguration>();

            _cacheEnabled = Convert.ToBoolean(configuration.GetRequiredSection("CacheEnabled").Value);
            _maxCountLastFiles = Convert.ToInt32(configuration.GetRequiredSection("MaxCountLastFiles").Value);

            InitializeLastFiles();

            ClickCommand = new RelayCommand<VisualCard>(HandleCardClick);
            DirectoryInputCommand = new RelayCommand<string>(HandleDirectoryInput);
            SearchInputCommand = new RelayCommand<string>(HandleSearchInput);
        }

        private void HandleSearchInput(string? searchText)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                foreach (var item in Cards)
                {
                    item.IsVisible = true;
                }

                return;
            }

            searchText = searchText.ToLower();

            foreach (var item in Cards)
            {
                item.IsVisible = item.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase);
            }
        }

        private void InitializeLastFiles()
        {
            var lastFiles = _lastFilesService.Load();

            foreach (var file in lastFiles)
            {
                var card = CreateCard(file);
                card.IsLastFile = true;

                LastFilesCards.AddFirst(card);
            }
        }

        private void HandleCardClick(VisualCard? card)
        {
            if (card == null)
            {
                HandyControl.Controls.Growl.Error(ErrorGrowlInfo.Instance);
                return;
            }

            try
            {
                _clipboardService.Set(card.FilePath);

                var cloneCard = card.Clone();
                cloneCard.IsLastFile = true;

                LastFilesCards.AddFirst(cloneCard);

                var removeLast = LastFilesCards.Count > _maxCountLastFiles;
                _lastFilesService.Save(card.FilePath, removeLast);

                if (removeLast)
                    LastFilesCards.RemoveLast();

                HandyControl.Controls.Growl.Success(SuccessGrowlInfo.Instance);
            }
            catch (Exception)
            {
                HandyControl.Controls.Growl.Error(ErrorGrowlInfo.Instance);
#if DEBUG
                throw;
#endif
            }
        }

        private void HandleDirectoryInput(string? parameter)
        {
            if (string.IsNullOrEmpty(parameter))
                return;

            if (!parameter.Contains(@":\"))
                return;

            parameter = parameter.Trim();

            string[] filePaths = Directory.GetFiles(parameter, "*.svg", new EnumerationOptions()
            {
                MatchCasing = MatchCasing.CaseInsensitive,
                IgnoreInaccessible = true,
                RecurseSubdirectories = Subfolders
            });

            for (int i = 0; i < filePaths.Length; i++)
            {
                string filePath = filePaths[i];

                App.Current.Dispatcher.Invoke(() =>
                {
                    var card = CreateCard(filePath);
                    Cards.Add(card);

                    Progress = (double)i / filePaths.Length * 100d;
                }, System.Windows.Threading.DispatcherPriority.Input);
            }

            _lastDirectoriesService.Save(parameter, LastDirectories.Length == 10);
        }

        private VisualCard CreateCard(string filePath)
        {
            byte[] thumbnail = Array.Empty<byte>();
            var loadedFromCache = false;

            if (_cacheService.TryLoadFromCache(filePath, out byte[] data))
            {
                thumbnail = data;
                loadedFromCache = true;
            }
            else
            {
                thumbnail = _imageConverterService.ConvertSvgToPng(filePath);
            }

            var fileName = Path.GetFileName(filePath);

            var card = new VisualCard(filePath, fileName, thumbnail, true);

            if (_cacheEnabled && !loadedFromCache)
                _cacheService.Cache(thumbnail, filePath);

            return card;
        }
    }
}
