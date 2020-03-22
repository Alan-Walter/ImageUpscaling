using ImageScaling.IO;

namespace ImageScaling.Interpolation
{
    /// <summary>
    /// Абстрактный класс алгоритма интерполяции
    /// </summary>
    public abstract class InterpolationAlgorithm : IInterpolationScaleImage
    {
        public double Scale { get; set; }

        public void ScaleImage(string imagePath, string outputPath)
        {
            var byteImage = ByteImageFileManager.ReadFile(imagePath);

            var result = ScaleImage(byteImage, Scale);

            var path = new FilePathBuilder(imagePath)
                .SetPath(outputPath)
                .SetAlgorithmName(GetType().Name)
                .SetScale(Scale)
                .Build();

            ByteImageFileManager.Save(result, path);
        }

        /// <summary>
        /// Масштабировать изображение
        /// </summary>
        /// <param name="byteImage">Входное изображение</param>
        /// <returns>Увеличенное изображение</returns>
        public ByteImage ScaleImage(ByteImage byteImage)
        {
            return ScaleImage(byteImage, Scale);
        }

        /// <summary>
        /// Масштабировать ByteImage изображение
        /// </summary>
        /// <param name="source">Исходное изображение</param>
        /// <param name="scale">Масштаб</param>
        /// <returns>Увеличенное изображение</returns>
        protected abstract ByteImage ScaleImage(ByteImage source, double scale);
    }
}