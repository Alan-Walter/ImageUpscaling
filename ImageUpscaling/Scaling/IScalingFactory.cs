using System.Collections.Generic;

namespace ImageUpscaling.Scaling
{
    interface IScalingFactory<T>
    {
        ICollection<T> GetScaleObjects();
    }
}
