using System;
using System.Collections.Generic;
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

namespace SvgViewer
{
    /// <summary>
    /// Логика взаимодействия для ItemCard.xaml
    /// </summary>
    public partial class ItemCard : UserControl
    {
        private string _fullPath = default;
        private ItemCard()
        {
            InitializeComponent();
            MainGrid.MouseLeftButtonUp += delegate (object sender, MouseButtonEventArgs e)
            {
                Clipboard.SetText(_fullPath ?? "");
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
        public ItemCard(string imagePath) : this()
        {
            if (!imagePath.Contains(".svg"))
                return;

            _fullPath = imagePath;
            SvgPlace.Source = new Uri(imagePath);
            NameTextblock.Text = Path.GetFileName(imagePath);
        }
    }
}
