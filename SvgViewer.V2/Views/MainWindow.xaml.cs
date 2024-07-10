﻿using SvgViewer.V2.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SvgViewer.V2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindowX_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3 || (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.F)))
            {
                SearchTextBox.Focus();
            }
        }

        private void MainWindowX_Deactivated(object sender, EventArgs e)
        {
            
            //MainScrollViewer.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var box = sender as ListBox;
            
            DirectoryInput.Text = box.SelectedValue as string;

            DirectoryInput.Focus();
        }
    }
}