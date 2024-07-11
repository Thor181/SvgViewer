using SvgViewer.V2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public partial class CardView : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty CardProperty = DependencyProperty.Register(nameof(Card), typeof(VisualCard), typeof(CardView));
        public static readonly DependencyProperty ClickCommandProperty = DependencyProperty.Register(nameof(ClickCommand), typeof(ICommand), typeof(CardView));
        public static readonly DependencyProperty ClickCommandParameterProperty = DependencyProperty.Register(nameof(ClickCommandParameter), typeof(VisualCard), typeof(CardView));
        public static readonly DependencyProperty FavoriteClickCommandProperty = DependencyProperty.Register(nameof(FavoriteClickCommand), typeof(ICommand), typeof(CardView));
        public static readonly DependencyProperty CanFavoriteClickCommandProperty = DependencyProperty.Register(nameof(CanFavoriteClickCommand), typeof(bool), typeof(CardView), new PropertyMetadata(false));

        public event PropertyChangedEventHandler? PropertyChanged;
        
        public VisualCard Card { get => (VisualCard)GetValue(CardProperty); set => SetValueInternal(CardProperty, value); }

        public ICommand ClickCommand { get => (ICommand)GetValue(ClickCommandProperty); set => SetValueInternal(ClickCommandProperty, value); }
        public VisualCard ClickCommandParameter { get => (VisualCard)GetValue(ClickCommandParameterProperty); set => SetValueInternal(ClickCommandParameterProperty, value); }

        public ICommand FavoriteClickCommand { get => (ICommand)GetValue(FavoriteClickCommandProperty); set => SetValueInternal(FavoriteClickCommandProperty, value); }
        public bool CanFavoriteClickCommand { get => (bool)GetValue(CanFavoriteClickCommandProperty); set => SetValueInternal(CanFavoriteClickCommandProperty, value); }

        public CardView()
        {
            InitializeComponent();
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ClickCommand?.Execute(ClickCommandParameter);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FavoriteClickCommand?.Execute(ClickCommandParameter);
        }

        private void SetValueInternal(DependencyProperty dp, object value, [CallerMemberName] string? prop = null)
        {
            SetValue(dp, value);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
