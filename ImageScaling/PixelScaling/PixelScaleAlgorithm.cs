using ImageScaling.IO;

namespace ImageScaling.PixelScaling
{
    /// <summary>
    /// Алгоритм масштабирования пиксельных изображений
    /// </summary>
    public abstract class PixelScaleAlgorithm : IScaleImage
    {
        /// <summary>
        /// Масштаб увеличения
        /// </summary>
        protected abstract int Scale { get; }

        /// <summary>
        /// Масштабировать изображение
        /// </summary>
        /// <param name="imagePath">Входное изображение</param>
        /// <param name="outputPath">Увеличенное изображение</param>
        public void ScaleImage(string imagePath, string outputPath)
        {
            var byteImage = ByteImageFileManager.ReadFile(imagePath);

            var result = ScaleImage(byteImage);

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
        public abstract ByteImage ScaleImage(ByteImage byteImage);
    }
}