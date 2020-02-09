using System.Collections.Generic;

namespace ImageScaling
{
    /// <summary>
    /// Интерфейс фабрики масштабирования изображений
    /// </summary>
    public interface IScalingFactory
    {
        /// <summary>
        /// Получение объектов алгоритмов масштабирования
        /// </summary>
        /// <returns></returns>
        ICollection<IScaling> GetScaleObjects();
    }
}