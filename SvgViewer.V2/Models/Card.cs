using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SvgViewer.V2.Models
{
    public class Card : ObservableObject
    {
        public string FilePath { get; set; }
        public string Name { get; set; }
        public byte[] Thumbnail { get; set; }
        public bool IsLastFile { get; set; }

        private bool _isFavorite;
        public bool IsFavorite { get => _isFavorite; set => SetProperty(ref _isFavorite, value); }

        public Card(string path, string name, byte[] thumbnail)
        {
            FilePath = path;
            Name = name;
            Thumbnail = thumbnail;
        }

        public virtual Card Clone()
        {
            return new Card(this.FilePath, this.Name, this.Thumbnail) { IsLastFile = this.IsLastFile, IsFavorite = this.IsFavorite };
        }
    }
}
