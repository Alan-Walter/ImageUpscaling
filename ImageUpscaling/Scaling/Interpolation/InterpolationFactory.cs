using System.Collections.Generic;
using ImageUpscaling.Helpers;

namespace ImageUpscaling.Scaling.Interpolation
{
    public class InterpolationFactory : IScalingFactory<IInterpolationScaling>
    {
        public ICollection<IInterpolationScaling> GetScaleObjects()
        {
            return ReflectionHelper.GetInstance<IInterpolationScaling>();
        }
    }
}
