using ImageScaling.Extensions;

using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace ImageScaling.IO
{
    /// <summary>
    /// Менеджер файлов изображений ByteImage
    /// </summary>
    public static class ByteImageFileManager
    {
        /// <summary>
        /// Прочитать изображение из файла
        /// </summary>
        /// <param name="path">Путь до файла</param>
        /// <returns>Объект изображения</returns>
        public static ByteImage ReadFile(string path)
        {
            var bitmap = Bitmap.FromFile(path) as Bitmap;
            return bitmap.ToByteImage();
        }

        /// <summary>
        /// Сохранить изображение
        /// </summary>
        /// <param name="image">Изображение</param>
        /// <param name="path">Путь</param>
        public static void Save(ByteImage image, string path)
        {
            var bitmap = new Bitmap(image.Width, image.Height, image.PixelFormat);
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.WriteOnly, image.PixelFormat);
            var data = image.GetBitmapData(bitmapData.Stride);
            Marshal.Copy(data, 0, bitmapData.Scan0, data.Length);
            bitmap.UnlockBits(bitmapData);
            bitmap.SetResolution(image.DpiX, image.DpiY);
            if (image.ColorPalette != null && image.ColorPalette.Entries.Length != 0)
            {
                bitmap.Palette = image.ColorPalette;
            }
            string dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            if (image.ImageFormat != null)
            {
                bitmap.Save(path, image.ImageFormat);
            }
            else
            {
                bitmap.Save(path);
            }
        }
    }
}