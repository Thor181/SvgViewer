using Svg;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvgViewer.V2.Services
{
    public class ImageConverterService
    {
        public byte[] ConvertSvgToPng(string svgPath)
        {
            var svg = SvgDocument.Open(svgPath);
            var bmp = svg.Draw(100, 100);

            using var ms = new MemoryStream();

            bmp.Save(ms, ImageFormat.Png);

            return ms.ToArray();
        }
    }
}
