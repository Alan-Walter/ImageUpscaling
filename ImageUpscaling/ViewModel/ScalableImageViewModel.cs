using System.IO;
using System.Windows.Media.Imaging;

using ImageUpscaling.Model;

namespace ImageUpscaling.ViewModel
{
    /// <summary>
    /// ViewModel для масштабируемого изображения
    /// </summary>
    internal class ScalableImageViewModel : BaseViewModel
    {
        private readonly ScalableImage scalableImage;

        /// <summary>
        /// Сокращённое имя файла
        /// </summary>
        public string Name
        {
            get
            {
                if (scalableImage.Name.Length < 30)
                    return scalableImage.Name;
                return Path.GetFileName(scalableImage.Name).Substring(0, 30) + ".. " + Path.GetExtension(scalableImage.Name);
            }
        }

        /// <summary>
        /// Полное имя файла
        /// </summary>
        public string FullName => scalableImage.Name;

        /// <summary>
        /// Изображение
        /// </summary>
        public BitmapSource Image => scalableImage.Image;

        /// <summary>
        /// Ширина
        /// </summary>
        public int Width => scalableImage.Image.PixelWidth;

        /// <summary>
        /// Высота
        /// </summary>
        public int Height => scalableImage.Image.PixelHeight;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="scalableImage"></param>
        public ScalableImageViewModel(ScalableImage scalableImage)
        {
            this.scalableImage = scalableImage;
        }
    }
}