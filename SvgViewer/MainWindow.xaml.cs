using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SvgViewer
{
    public partial class MainWindow : Window
    {
        public ConfigWorker ConfigWorker { get; set; }

        public MainWindow()
        {
            ConfigWorker = new ConfigWorker();
            InitializeComponent();
            InitializeLastFiles();
            InitializeDelegates();
        }

        private void InitializeLastFiles()
        {
            var lastFiles = ConfigWorker.LastFiles;
            foreach (var file in lastFiles)
            {
                if (File.Exists(file))
                    AddLastFileToSecondWrapPanel(new ItemCard(file));
            }
        }

        private void InitializeDelegates()
        {
            SearchTextbox.TextChanged += async delegate (object sender, TextChangedEventArgs e)
            {
                var tempText = ((TextBox)sender).Text.ToLower();
                await Task.Delay(1000);

                if (!((TextBox)sender).Text.ToLower().Equals(tempText))
                    return;

                var searchText = ((TextBox)sender).Text.ToLower();

                foreach (var item in MainWrapPanel.Children)
                {
                    if (item is ItemCard card)
                        card.Visibility = !card.FileName.Contains(searchText, StringComparison.InvariantCultureIgnoreCase)
                            ? Visibility.Collapsed : Visibility.Visible;
                }
            };

            LastDirectoriesListbox.SelectionChanged += (object sender, SelectionChangedEventArgs e) =>
                DirectoryPathTextbox.Text = ((ListBox)sender).SelectedItem.ToString();

            MainWindowX.Deactivated += (object sender, EventArgs e) => MainWrapPanel.Focus();
        }

        private async void DirectoryPathTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = ((TextBox)sender).Text;
            if (string.IsNullOrWhiteSpace(text))
                return;

            if (!LastDirectoriesListbox.Items.Contains(text))
                ConfigWorker.AddLastDirectory(text);

            await CollectFiles(text);
        }

        private async Task CollectFiles(string rootPath)
        {
            if (!rootPath.Contains(":\\"))
                return;

            string[] filePaths = Directory.GetFiles(rootPath, "*.svg", new EnumerationOptions()
            {
                MatchCasing = MatchCasing.CaseInsensitive,
                IgnoreInaccessible = true,
                RecurseSubdirectories = InnerDirectoriesCheckbox.IsChecked == true
            }).ToArray();

            try
            {
                await Task.Run(async () =>
                {
                    foreach (var path in filePaths)
                    {
                        await Dispatcher.InvokeAsync(() =>
                        {
                            var card = new ItemCard(path);
                            card.Copied += Card_Copied;
                            MainWrapPanel.Children.Add(card);
                            CountTextblock.Text = MainWrapPanel.Children.Count.ToString();
                        });
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Card_Copied(ItemCard sender)
        {
            AddLastFileToSecondWrapPanel(sender);
            ConfigWorker.AddLastFiles(sender.FilePath);
        }

        private void AddLastFileToSecondWrapPanel(ItemCard itemCard)
        {
            if (SecondWrapPanel.Children.Count == ConfigWorker.MaxCountLastFiles)
                SecondWrapPanel.Children.RemoveAt(ConfigWorker.MaxCountLastFiles - 1);

            SecondWrapPanel.Children.Insert(0, itemCard.Clone());
        }

    }
}
