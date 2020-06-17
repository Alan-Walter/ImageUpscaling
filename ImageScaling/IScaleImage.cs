namespace ImageScaling
{
    /// <summary>
    /// Интерфейс алгоритма масштабирования изображения
    /// </summary>
    public interface IScaleImage
    {
        /// <summary>
        /// Масштабировать изображение
        /// </summary>
        /// <param name="imagePath">Путь для входного файла изображения</param>
        /// <param name="outputPath">Путь выходного файла для изображения</param>
        void ScaleImage(string imagePath, string outputPath);

        /// <summary>
        /// Масштабировать изображение
        /// </summary>
        /// <param name="byteImage">Входное изображение</param>
        /// <returns>Увеличенное изображение</returns>
        ByteImage ScaleImage(ByteImage byteImage);
    }
}