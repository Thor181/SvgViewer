using SvgViewer.V2.ViewModels;
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
            ContentGrid.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void MainScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            
        }

        private void ListBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var box = sender as TextBlock;

            DirectoryInput.Text = box.Text;

            DirectoryInput.Focus();
        }
    }
}