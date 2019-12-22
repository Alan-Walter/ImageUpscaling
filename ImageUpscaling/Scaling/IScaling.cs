using System.Windows.Media.Imaging;

namespace ImageUpscaling.Scaling
{
    /// <summary>
    /// Интерфейс для масштабирования изображения
    /// </summary>
    public interface IScaling
    {
        /// <summary>
        /// Название метода
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Масштабируемость
        /// </summary>
        bool IsScalable { get; }

        /// <summary>
        /// Масштабировать изображение
        /// </summary>
        /// <param name="source">Исходное изображение</param>
        /// <param name="scale">Масштаб</param>
        /// <returns></returns>
        BitmapSource ScaleImage(BitmapSource source, double scale);
    }
}
