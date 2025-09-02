using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvgViewer.V2.Core.Utils
{
    public static class Constants
    {
        public static class Paths
        {
            public static readonly string RootDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SvgViewer");
        }
    }
}
