using SvgViewer.V2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace SvgViewer.V2.Views
{
    public partial class CardView : UserControl
    {
        public static readonly DependencyProperty CardProperty = DependencyProperty.Register(nameof(Card), typeof(Card), typeof(CardView));
        public static readonly DependencyProperty ClickCommandProperty = DependencyProperty.Register(nameof(ClickCommand), typeof(ICommand), typeof(CardView));
        public static readonly DependencyProperty ClickCommandParameterProperty = DependencyProperty.Register(nameof(ClickCommandParameter), typeof(Card), typeof(CardView));

        public Card Card { get => (Card)GetValue(CardProperty); set => SetValue(CardProperty, value); }
        public ICommand ClickCommand { get => (ICommand)GetValue(ClickCommandProperty); set => SetValue(ClickCommandProperty, value); }
        public Card ClickCommandParameter { get => (Card)GetValue(ClickCommandParameterProperty); set => SetValue(ClickCommandParameterProperty, value); }

        //public byte[] ThumbnailPath { get => Card.ThumbnailPath; set => Card.ThumbnailPath = value; }

        public CardView()
        {
            InitializeComponent();
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ClickCommand?.Execute(ClickCommandParameter);
        }
    }
}
