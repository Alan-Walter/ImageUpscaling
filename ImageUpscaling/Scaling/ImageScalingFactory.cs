using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageUpscaling.Helpers;

namespace ImageUpscaling.Scaling
{
    public class ImageScalingFactory : IScalingFactory<IScaling>
    {
        public ICollection<IScaling> GetScaleObjects()
        {
            return ReflectionHelper.GetInstance<IScaling>();
        }
    }
}
