using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SvgViewer.V2.Services
{
    public class ClipboardService
    {
        public void Set(string path)
        {
            Clipboard.SetDataObject(path, true);
        }
    }
}
