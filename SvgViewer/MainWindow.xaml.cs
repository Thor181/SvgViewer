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
        private ObservableCollection<string> pathsSvg = new ObservableCollection<string>();
        public IEnumerable<string> LastDirectories
        {
            get => DirectoriesWorker.ReadFromFile();
        }

        public MainWindow()
        {
            InitializeComponent();
            InitializeDelegates();
        }

        private void InitializeDelegates()
        {
            pathsSvg.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs e)
            {
                foreach (var item in e.NewItems)
                {
                    MainWrapPanel.Children.Add(new ItemCard(item.ToString()));
                }
            };

            SearchTextbox.TextChanged += async delegate (object sender, TextChangedEventArgs e)
            {
                var tempText = ((TextBox)sender).Text.ToLower();
                await Task.Delay(500);

                if (!((TextBox)sender).Text.ToLower().Equals(tempText))
                    return;

                var searchText = ((TextBox)sender).Text.ToLower();
                var filteredCollection = pathsSvg.Where(x => Path.GetFileName(x.ToLower()).Contains(searchText));

                MainWrapPanel.Children.Clear();
                foreach (var item in filteredCollection)
                {
                    MainWrapPanel.Children.Add(new ItemCard(item.ToString()));
                }
            };

            LastDirectoriesListbox.SelectionChanged += (object sender, SelectionChangedEventArgs e) =>
                DirectoryPathTextbox.Text = ((ListBox)sender).SelectedItem.ToString();

            MainWindowX.Deactivated += (object sender, EventArgs e) => MainWrapPanel.Focus();
        }

        private void DirectoryPathTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = ((TextBox)sender).Text;
            if (string.IsNullOrWhiteSpace(text) || string.IsNullOrEmpty(text))
                return;

            DirectoriesWorker.WriteToFile(text);
            CollectFiles(text);
        }

        private void CollectFiles(string rootPath)
        {
            if (!rootPath.Contains(":\\"))
                return;

            try
            {
                string[] filePaths = Directory.GetFiles(rootPath);
                Task.Run(() =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        foreach (var item in filePaths)
                        {
                            if (item.Contains(".svg") && !pathsSvg.Contains(item))
                            {
                                pathsSvg.Add(item);
                            }
                        }
                    });
                });

                if (InnerDirectoriesCheckbox.IsChecked == true)
                {
                    foreach (var item in Directory.GetDirectories(rootPath))
                    {
                        CollectFiles(item);
                    }
                }
            }
            catch (Exception)
            {
#if !DEBUG
                throw;
#endif
            }
        }
    }
}
