using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace SvgViewer
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public ConfigWorker ConfigWorker { get; set; }

        private bool _isLoading = false;

        public event PropertyChangedEventHandler PropertyChanged;

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
                    AddLastFileToSecondWrapPanel(new ItemCard(file, false));
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
                    _isLoading = true;

                    foreach (var path in filePaths)
                    {
                        await Dispatcher.InvokeAsync(() =>
                        {
                            var card = new ItemCard(path, false);
                            card.Copied += Card_Copied;
                            card.FavoriteClicked += Card_FavoriteClicked;
                            MainWrapPanel.Children.Add(card);
                            CountTextblock.Text = MainWrapPanel.Children.Count.ToString();
                        }, System.Windows.Threading.DispatcherPriority.Input);
                    }

                    _isLoading = false;

                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Card_FavoriteClicked(ItemCard sender)
        {
            FavoriteWrapPanel.Children.Add(sender);
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

        private void Grid_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (_isLoading)
                return;

            const int minSize = 80;

            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Dispatcher.Invoke(() =>
                {
                    foreach (ItemCard item in MainWrapPanel.Children)
                    {
                        var newWidth = 10 * (e.Delta / Math.Abs(e.Delta));
                        var newHeight = 10 * (e.Delta / Math.Abs(e.Delta));
                        item.Width += newWidth;
                        item.Height += newHeight;

                        if (item.Width < minSize)
                            item.Width = minSize;

                        if (item.Height < minSize)
                            item.Height = minSize;
                    }
                }, System.Windows.Threading.DispatcherPriority.Send);

            }
        }
    }
}
