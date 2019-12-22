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
    /// <summary>
    /// Расширение для класса BitmapSource
    /// </summary>
    static class BitmapImageExtension
    {
        /// <summary>
        /// Получение буффера байтов
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static byte[] GetBuffer(this BitmapSource bitmap)
        {
            int stride = BitmapHelper.GetStride(bitmap.PixelWidth, bitmap.Format.BitsPerPixel);
            var buffer = new byte[bitmap.PixelHeight * stride];
            bitmap.CopyPixels(buffer, stride, 0);
            return buffer;
        }
    }
}
