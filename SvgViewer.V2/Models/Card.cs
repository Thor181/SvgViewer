using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvgViewer.V2.Models
{
    public class Card
    {
        public string FilePath { get; set; }
        public string Name { get; set; }

        public Card(string path, string name)
        {
            FilePath = path;
            Name = name;
        }

        
    }
}
