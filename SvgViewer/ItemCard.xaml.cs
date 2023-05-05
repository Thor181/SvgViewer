using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SvgViewer
{
    public partial class ItemCard : UserControl
    {
        public string FilePath { get; private set; }
        public string FileName { get; private set; }

        public delegate void CopyHandler(ItemCard sender);
        public event CopyHandler Copied;

        private ItemCard()
        {
            InitializeComponent();
            CopyHandler copyHandler = delegate (ItemCard sender) { };
        }

        public ItemCard(string imagePath) : this()
        {
            NonVisibleLabel.Content = imagePath;
            FilePath = imagePath;
            FileName = Path.GetFileName(imagePath);
            SvgPlace.Source = new Uri(imagePath);
            NameTextblock.Text = Path.GetFileName(imagePath);
            MainGrid.MouseLeftButtonUp += delegate (object sender, MouseButtonEventArgs e)
            {
                Clipboard.SetText(NonVisibleLabel.Content.ToString() ?? "");
                Task.Run(() =>
                {
                    Dispatcher.Invoke(async () =>
                    {
                        Copied?.Invoke(this);

                        IsCopiedPopup.IsOpen = true;
                        await Task.Delay(1500);
                        IsCopiedPopup.IsOpen = false;
                    });
                });
            };
        }

        public ItemCard Clone()
        {
            return new ItemCard(FilePath);
        }
    }
}
