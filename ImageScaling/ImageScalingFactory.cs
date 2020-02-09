using System.Collections.Generic;

using ImageScaling.Helpers;

namespace ImageScaling
{
    /// <summary>
    /// Фабрика объектов для масштабирования изображения
    /// </summary>
    public class ImageScalingFactory : IScalingFactory
    {
        /// <summary>
        /// Получение объектов алгоритмов масштабирования
        /// </summary>
        /// <returns></returns>
        public ICollection<IScaling> GetScaleObjects()
        {
            return ReflectionHelper.GetInstance<IScaling>();
        }
    }
}