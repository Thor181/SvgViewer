using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvgViewer.V2.Core.Services.Clipboard
{
    public interface IClipboardService
    {
        public void Set(string path);
    }
}
