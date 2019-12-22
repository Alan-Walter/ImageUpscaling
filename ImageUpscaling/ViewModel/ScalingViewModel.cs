using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageUpscaling.Scaling;

namespace ImageUpscaling.ViewModel
{
    /// <summary>
    /// ViewModel для алгоритма масштабирования
    /// </summary>
    public class ScalingViewModel
    {
        /// <summary>
        /// Объект алгоритма масштабирования
        /// </summary>
        public IScaling Scaling { get; }

        /// <summary>
        /// Масштабирумость алгоритма
        /// </summary>
        public bool IsScalable => Scaling.IsScalable;

        public ScalingViewModel(IScaling scaling)
        {
            Scaling = scaling;
        }

        public override string ToString()
        {
            return Scaling.Title;
        }
    }
}
