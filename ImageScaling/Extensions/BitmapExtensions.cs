using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ImageScaling.Extensions
{
    /// <summary>
    /// Расширения класса Bitmap
    /// </summary>
    public static class BitmapExtensions
    {
        /// <summary>
        /// Конвертировать Bitmap в формат RGB
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Bitmap ToRGBBitmap(this Bitmap bitmap)
        {
            ExceptionHelper.ThrowIfNull(bitmap, "bitmap");

            if (bitmap.PixelFormat == PixelFormat.Format24bppRgb)
            {
                return bitmap;
            }

            Bitmap rgbBitmap = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format24bppRgb);

            using (var graph = Graphics.FromImage(rgbBitmap))
            {
                graph.DrawImage(bitmap, new Rectangle(0, 0, rgbBitmap.Width, rgbBitmap.Height));
            }

            return rgbBitmap;
        }

        /// <summary>
        /// Изменить размер Bitmap.
        /// https://stackoverflow.com/questions/1922040/how-to-resize-an-image-c-sharp
        /// </summary>
        /// <param name="bitmap">Входное изображение</param>
        /// <param name="width">Ширина</param>
        /// <param name="height">Высота</param>
        /// <returns>Изображение с нужным размером</returns>
        public static Bitmap Resize(this Bitmap bitmap, int width, int height)
        {
            ExceptionHelper.ThrowIfNull(bitmap, "bitmap");

            if (bitmap.Width == width && bitmap.Height == height)
            {
                return bitmap;
            }

            var rectangle = new Rectangle(0, 0, width, height);
            var scaledImage = new Bitmap(width, height);

            if (bitmap.HorizontalResolution > 0 && bitmap.VerticalResolution > 0)
            {
                scaledImage.SetResolution(bitmap.HorizontalResolution, bitmap.VerticalResolution);
            }

            using (var graphics = Graphics.FromImage(scaledImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using var wrapMode = new ImageAttributes();
                wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                graphics.DrawImage(bitmap, rectangle, 0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel, wrapMode);
            }

            return scaledImage;
        }

        /// <summary>
        /// Конвертировать bitmap в ByteImage
        /// </summary>
        /// <param name="bitmap">Объект изображения</param>
        /// <returns>Байтовое изображение</returns>
        public static ByteImage ToByteImage(this Bitmap bitmap)
        {
            ExceptionHelper.ThrowIfNull(bitmap, "bitmap");

            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
            var data = new byte[bitmapData.Stride * bitmapData.Height];
            Marshal.Copy(bitmapData.Scan0, data, 0, bitmapData.Stride * bitmapData.Height);
            bitmap.UnlockBits(bitmapData);
            return new ByteImage(data, bitmap.Width, bitmap.Height, bitmap.PixelFormat, bitmapData.Stride, bitmap.RawFormat, bitmap.HorizontalResolution, bitmap.VerticalResolution)
            {
                ColorPalette = bitmap.Palette
            };
        }
    }
}