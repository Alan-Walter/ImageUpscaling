using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageUpscaling.Scaling;

namespace ImageUpscaling.ViewModel
{
    public class ScalingViewModel
    {
        public IScaling Scaling { get; }

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
