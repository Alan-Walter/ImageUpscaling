namespace ImageScaling
{
    /// <summary>
    /// Интерфейс для масштабирования изображения
    /// </summary>
    public interface IScaling
    {
        /// <summary>
        /// Масштабировать изображение
        /// </summary>
        /// <param name="source">Исходное изображение</param>
        /// <param name="scale">Масштаб</param>
        /// <returns>Увеличенное изображение</returns>
        ByteImage ScaleImage(ByteImage source, double scale);
    }
}