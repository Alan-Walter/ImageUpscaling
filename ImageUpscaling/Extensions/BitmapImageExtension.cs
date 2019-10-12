using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ImageUpscaling.Helpers;

namespace ImageUpscaling.Extensions
{
    static class BitmapImageExtension
    {
        public static byte[] GetBuffer(this BitmapSource bitmap)
        {
            int stride = BitmapHelper.GetStride(bitmap.PixelWidth, bitmap.Format.BitsPerPixel);
            var buffer = new byte[bitmap.PixelHeight * stride];
            bitmap.CopyPixels(buffer, stride, 0);
            return buffer;
        }
    }
}
