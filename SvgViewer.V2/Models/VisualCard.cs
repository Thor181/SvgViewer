using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvgViewer.V2.Models
{
    public class VisualCard : Card
    {
        private bool _isVisible;
        public bool IsVisible { get => _isVisible; set => SetProperty(ref _isVisible, value); }

        public VisualCard(string path, string name, byte[] thumbnail, bool isVisible) : base(path, name, thumbnail)
        {
            IsVisible = isVisible;
        }

        public override VisualCard Clone()
        {
            return new VisualCard(this.FilePath, this.Name, this.Thumbnail, this.IsVisible) { IsLastFile = IsLastFile, IsFavorite = this.IsFavorite};
        }
    }
}
