﻿using System;
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

            System.Collections.Concurrent.ConcurrentBag<ItemCard> cards = new();

            try
            {
                string[] filePaths = Directory.GetFiles(rootPath).Where(x => x.Contains(".svg")).ToArray();
                await Task.Run(async () =>
                {
                    Parallel.ForEach(filePaths, x =>
                    {
                        Dispatcher.Invoke(() =>
                        {
                            var card = new ItemCard(x);
                            card.Copied += Card_Copied;
                            cards.Add(card);
                        });
                    });

                    foreach (var item in cards)
                    {
                        await Dispatcher.InvokeAsync(() =>
                        {
                            MainWrapPanel.Children.Add(item);
                            CountTextblock.Text = MainWrapPanel.Children.Count.ToString();
                        });
                    }
                });

                if (InnerDirectoriesCheckbox.IsChecked == true)
                {
                    var subfolders = Directory.GetDirectories(rootPath);
                    foreach (var subfolder in subfolders)
                        await CollectFiles(subfolder);
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (UnauthorizedAccessException)
            {
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
