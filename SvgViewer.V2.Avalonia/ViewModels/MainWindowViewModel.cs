using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Svg.Skia;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace SvgViewer.V2.Avalonia.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<StackPanel> Svgs { get; set; } = [];

        public MainWindowViewModel()
        {
            var svgs = Directory.EnumerateFiles("D:\\DRX_KNIPI", "*.svg", SearchOption.AllDirectories).ToArray();
            var count = svgs.Length;
            for (int i = 0; i < (int)Math.Ceiling(count / 10d); i++)
            {
                var sp = new StackPanel() {  Orientation = global::Avalonia.Layout.Orientation.Horizontal};
                sp.Background = new SolidColorBrush(Color.FromRgb((byte)(i % 255), (byte)(i % 255), (byte)(i % 255)));
                for (int j = 0; j < 10; j++)
                {
                    var index = i * 10 + j;

                    if (index == count)
                        break;

                    var path = svgs[index];
                    var svg = new global::Avalonia.Svg.Skia.Svg(new System.Uri(path));
                    svg.Path = path;
                    svg.Width = svg.Height = 70;
                    sp.Children.Add(svg);
                }

                Svgs.Add(sp);

            }
        }
    }
}
