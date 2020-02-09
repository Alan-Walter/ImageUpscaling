using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using ImageScaling;

using EnumPixelFormat = System.Drawing.Imaging.PixelFormat;

namespace ImageUpscaling.Desktop.Core.Extensions
{
    public static class ByteImageExtensions
    {
        public static BitmapSource ToBitmapSource(this ByteImage byteImage)
        {
            PixelFormat pixelFormat = GetPixelFormat(byteImage.PixelFormat);

            return BitmapSource.Create(byteImage.Width, byteImage.Height, byteImage.DpiX, byteImage.DpiY, pixelFormat, null, byteImage.GetData(), byteImage.Stride);
        }

        /// <summary>
        /// Получить структуру PixelFormat по перечислению
        /// https://stackoverflow.com/questions/5106505/converting-gdi-pixelformat-to-wpf-pixelformat
        /// </summary>
        /// <param name="enumPixelFormat"></param>
        /// <returns></returns>
        private static PixelFormat GetPixelFormat(EnumPixelFormat enumPixelFormat)
        {
            PixelFormat pixelFormat;
            switch (enumPixelFormat)
            {
                case EnumPixelFormat.Format32bppArgb:
                    pixelFormat = PixelFormats.Bgra32;
                    break;

                case EnumPixelFormat.Format24bppRgb:
                    pixelFormat = PixelFormats.Bgr24;
                    break;

                case EnumPixelFormat.Format32bppRgb:
                    pixelFormat = PixelFormats.Bgr32;
                    break;

                default:
                    throw new NotImplementedException("Cannot find pixel format for ByteImage");
            }

            return pixelFormat;
        }
    }
}