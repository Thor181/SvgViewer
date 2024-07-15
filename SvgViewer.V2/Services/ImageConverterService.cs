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
            if (string.IsNullOrEmpty(svgPath))
                return Array.Empty<byte>();

            var svg = SvgDocument.Open(svgPath);

            var heightToWidthRatio = svg.Height / svg.Width;

            var width = (int)(100);
            var height = (int)(100 * heightToWidthRatio);

            var bmp = svg.Draw(width, height);

            if (bmp == null)
                return Array.Empty<byte>();

            using var ms = new MemoryStream();

            bmp.Save(ms, ImageFormat.Png);

            return ms.ToArray();
        }
    }
}
