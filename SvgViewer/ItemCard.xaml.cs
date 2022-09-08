using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SvgViewer
{
    public partial class ItemCard : UserControl
    {
        public string FullPath { get; private set; }
        public string FileName { get; private set; }

        private ItemCard()
        {
            InitializeComponent();
        }

        public ItemCard(string imagePath) : this()
        {
            NonVisibleLabel.Content = imagePath;
            FullPath = imagePath;
            FileName = Path.GetFileName(imagePath);
            SvgPlace.Source = new Uri(imagePath);
            NameTextblock.Text = Path.GetFileName(imagePath);
            MainGrid.MouseLeftButtonUp += delegate (object sender, MouseButtonEventArgs e)
            {
                Clipboard.SetText(FullPath ?? "");
                Task.Run(() =>
                {
                    Dispatcher.Invoke(async () =>
                    {
                        IsCopiedPopup.IsOpen = true;
                        await Task.Delay(1500);
                        IsCopiedPopup.IsOpen = false;
                    });
                });
            };
        }
    }
}
