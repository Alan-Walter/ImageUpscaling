using System;
using System.Drawing;

namespace ImageScaling.Helpers
{
    /// <summary>
    /// Хелпер изображений
    /// </summary>
    public static class ImageHelper
    {
        /// <summary>
        /// Генератор случайных чисел
        /// </summary>
        private static readonly Random random = new Random();

        /// <summary>
        /// Получить разрешение изображения
        /// </summary>
        /// <param name="imgPath">Путь до изображения</param>
        /// <returns>Разрешение изображения</returns>
        public static (int, int) GetResolution(string imgPath)
        {
            ExceptionHelper.ThrowIfNullOrEmpty(imgPath, "imgPath");

            var img = Image.FromFile(imgPath);

            return (img.Width, img.Height);
        }

        /// <summary>
        /// Получить случайную часть изображения нужного размера
        /// </summary>
        /// <param name="bitmap">Изображение</param>
        /// <param name="width">Ширина части</param>
        /// <param name="height">Высота части</param>
        /// <returns>Случайная часть изображения нужного размера</returns>
        public static Bitmap GetRandomImagePart(Bitmap bitmap, int width, int height)
        {
            ExceptionHelper.ThrowIfNull(bitmap, "bitmap");

            if (bitmap.Width < width || bitmap.Height < height)
            {
                return bitmap;
            }

            var x = random.Next(bitmap.Width - width);
            var y = random.Next(bitmap.Height - height);
            var part = bitmap.Clone(new Rectangle(x, y, width, height), bitmap.PixelFormat);
            return part;
        }

        /// <summary>
        /// Получить изображение нужного размера из центра картинки, если размеры картинки превышают указанные размеры
        /// </summary>
        /// <param name="bitmap">Изображение</param>
        /// <param name="width">Ширина</param>
        /// <param name="height">Высота</param>
        /// <returns>Середина изображения нужного размера</returns>
        public static Bitmap GetMiddleImagePart(Bitmap bitmap, int width, int height)
        {
            ExceptionHelper.ThrowIfNull(bitmap, "bitmap");

            if (bitmap.Width < width || bitmap.Height < height)
            {
                return bitmap;
            }

            var x = (bitmap.Width - width) / 2;
            var y = (bitmap.Height - height) / 2;

            var part = bitmap.Clone(new Rectangle(x, y, width, height), bitmap.PixelFormat);
            return part;
        }
    }
}