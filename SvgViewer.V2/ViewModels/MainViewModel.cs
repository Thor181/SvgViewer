using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SvgViewer.V2.Models;
using SvgViewer.V2.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace SvgViewer.V2.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        public ObservableCollection<Card> Cards { get; set; } 

        public ICommand ClickCommand { get; set; }
        public ICommand DirectoryInputCommand { get; set; }

        public bool Subfolders { get; set; } = true;

        public MainViewModel()
        {
            Cards = [];

            ClickCommand = new RelayCommand<Card>(HandleClick);
            DirectoryInputCommand = new RelayCommand<string>(HandleDirectoryInput);
        }

        private void HandleClick(Card? card)
        {
            HandyControl.Controls.Growl.Success(SuccessGrowlInfo.Instance);
        }

        private void HandleDirectoryInput(string? parameter)
        {
            if (string.IsNullOrEmpty(parameter))
                return;

            if (!parameter.Contains(":\\"))
                return;

            string[] filePaths = Directory.GetFiles(parameter, "*.svg", new EnumerationOptions()
            {
                MatchCasing = MatchCasing.CaseInsensitive,
                IgnoreInaccessible = true,
                RecurseSubdirectories = Subfolders
            });



            foreach (string filePath in filePaths)
            {

                App.Current.Dispatcher.Invoke(() => {

                    Cards.Add(new Card(filePath, Path.GetFileName(filePath)));

                });

            }

        }


    }
}
