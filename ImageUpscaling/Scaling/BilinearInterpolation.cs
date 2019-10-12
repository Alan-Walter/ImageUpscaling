using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ImageUpscaling.Extensions;
using ImageUpscaling.Helpers;

namespace ImageUpscaling.Scaling
{
    class BilinearInterpolation : BaseScaling
    {
        public override BitmapSource ScaleImage(BitmapSource source, double scale)
        {
            throw new NotImplementedException();
        }
    }
}
