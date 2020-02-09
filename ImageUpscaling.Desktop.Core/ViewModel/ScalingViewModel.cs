using ImageScaling;

namespace ImageUpscaling.Desktop.Core.ViewModel
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

        public ScalingViewModel(IScaling scaling)
        {
            Scaling = scaling;
        }

        public override string ToString()
        {
            return Scaling.GetType().Name;
        }
    }
}