using System.Windows.Media.Imaging;

using ImageScaling;

using ImageUpscaling.Desktop.Core.Extensions;

namespace ImageUpscaling.Desktop.Core.Model
{
    /// <summary>
    /// Масштабируемое изображение
    /// </summary>
    internal class ScalableImage
    {
        private ByteImage byteImage;

        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Изображение
        /// </summary>
        public BitmapSource Image { get; private set; }

        /// <summary>
        /// Байтовое представление изображения
        /// </summary>
        public ByteImage ByteImage
        {
            get => byteImage;
            set
            {
                byteImage = value;
                Image = byteImage.ToBitmapSource();
            }
        }
    }
}