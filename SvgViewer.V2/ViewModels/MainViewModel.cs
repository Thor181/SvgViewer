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

        public ObservableCollection<Card> Cards { get; set; } = [];
        public ObservableCollection<Card> LastFilesCards { get; set; } = [];

        private string[] _lastDirectories = [];
        public string[] LastDirectories { get => _lastDirectoriesService.Load(); set => SetProperty(ref _lastDirectories, value); }

        public ICommand ClickCommand { get; set; }
        public ICommand DirectoryInputCommand { get; set; }

        public string Version { get => _versionService.Version ?? string.Empty; }

        private bool _subfolders = true;
        public bool Subfolders { get => _subfolders; set => SetProperty(ref _subfolders, value); }

        private bool _isLoading = false;
        public bool IsLoading { get => _isLoading; set => SetProperty(ref _isLoading, value); }

        private double _progress = 0;
        public double Progress { get => _progress; set => SetProperty(ref _progress, value); }

        private string _selectedPath = string.Empty;
        public string SelectedPath { get => _selectedPath; set { SetProperty(ref _selectedPath, value); } }

        private bool _cacheEnabled = false;

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

            LastFilesCards.CollectionChanged += LastFilesCardsChanged;

            ClickCommand = new RelayCommand<Card>(HandleClick);
            DirectoryInputCommand = new RelayCommand<string>(HandleDirectoryInput);
        }

        private void LastFilesCardsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems?[0] is SvgViewer.V2.Models.Card card)
                {
                    _lastFilesService.Save(card.FilePath);
                }
            }
        }

        private void HandleClick(Card? card)
        {
            if (card == null)
            {
                HandyControl.Controls.Growl.Error(ErrorGrowlInfo.Instance);
                return;
            }

            try
            {
                _clipboardService.Set(card.FilePath);
                LastFilesCards.Add(card);

                HandyControl.Controls.Growl.Success(SuccessGrowlInfo.Instance);
            }
            catch (Exception)
            {
                HandyControl.Controls.Growl.Error(ErrorGrowlInfo.Instance);
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
                    byte[] thumbnail = Array.Empty<byte>();
                    var loadedFromCache = false;

                    if (_cacheService.TryLoadFromCache(filePath, out byte[] data))
                    {
                        thumbnail = data;
                        loadedFromCache = true;
                    }
                    else
                        thumbnail = _imageConverterService.ConvertSvgToPng(filePath);

                    var fileName = Path.GetFileName(filePath);
                    Cards.Add(new Card(filePath, fileName, thumbnail));

                    if (_cacheEnabled && !loadedFromCache)
                        _cacheService.Cache(thumbnail, filePath);

                    Progress = (double)i / filePaths.Length * 100d;
                }, System.Windows.Threading.DispatcherPriority.Input);
            }

            _lastDirectoriesService.Save(parameter);
        }

        protected override void OnPropertyChanging(PropertyChangingEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedPath))
            {
                HandleDirectoryInput(SelectedPath);
            }

            base.OnPropertyChanging(e);
        }
    }
}
