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
        public bool IsFavorite { get; private set; }

        public delegate void CopyHandler(ItemCard sender);
        public event CopyHandler Copied;
        public event CopyHandler FavoriteClicked;

        private ItemCard()
        {
            InitializeComponent();
        }

        public ItemCard(string imagePath, bool isFavorite) : this()
        {
            NonVisibleLabel.Content = imagePath;
            FilePath = imagePath;
            FileName = Path.GetFileName(imagePath);
            SvgPlace.Source = new Uri(imagePath);
            NameTextblock.Text = Path.GetFileName(imagePath);
            IsFavorite = isFavorite;

            isFavorite = false;
            
            FavoriteButton.Visibility = isFavorite ? Visibility.Visible : Visibility.Collapsed;
            FavoriteOutImage.Visibility = isFavorite ? Visibility.Collapsed : Visibility.Visible;

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

            FavoriteButton.MouseLeftButtonUp += delegate (object sender, MouseButtonEventArgs e)
            {
                FavoriteClicked?.Invoke(this);
            };
        }

        public ItemCard Clone()
        {
            return new ItemCard(FilePath, IsFavorite);
        }
    }
}
