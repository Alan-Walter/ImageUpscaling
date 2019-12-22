using System.Collections.Generic;

namespace ImageUpscaling.Scaling
{
    /// <summary>
    /// Интерфейс фабрики масштабирования изображений
    /// </summary>
    /// <typeparam name="T"></typeparam>
    interface IScalingFactory<T>
    {
        /// <summary>
        /// Получение объектов алгоритмов масштабирования
        /// </summary>
        /// <returns></returns>
        ICollection<T> GetScaleObjects();
    }
}
