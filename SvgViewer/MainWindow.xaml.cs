using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SvgViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<string> pathsSvg = new ObservableCollection<string>();

        public MainWindow()
        {
            InitializeComponent();
            pathsSvg.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs e)
            {
                //MainWrapPanel.Children.Clear();
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
                foreach (var item in filePaths)
                {
                    pathsSvg.Add(item);
                }
                if (InnerDirectoriesCheckbox.IsChecked == true)
                {
                    foreach (var item in Directory.GetDirectories(rootPath))
                    {
                        CollectFiles(item);
                    }
                }

                if (Directory.GetDirectories(rootPath).Any())
                {
                    return;
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
