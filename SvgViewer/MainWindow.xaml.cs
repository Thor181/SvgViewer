using System;
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
        ObservableCollection<string> pathsSvg = new ObservableCollection<string>();

        public delegate void NewDelegate(string path);

        public MainWindow()
        {
            InitializeComponent();
            pathsSvg.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs e)
            {
                foreach (var item in e.NewItems)
                {
                    MainWrapPanel.Children.Add(new ItemCard(item.ToString()));
                }
            };
        }

        private void DirectoryPathTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = ((TextBox)sender).Text;
            if (string.IsNullOrWhiteSpace(text) || string.IsNullOrEmpty(text))
                return;

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
#if DEBUG
                throw;
#endif
            }
        }
    }
}
