using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageUpscaling.Helpers;

namespace ImageUpscaling.Scaling
{
    /// <summary>
    /// Фабрика объектов для масштабирования изображения
    /// </summary>
    public class ImageScalingFactory : IScalingFactory<IScaling>
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
