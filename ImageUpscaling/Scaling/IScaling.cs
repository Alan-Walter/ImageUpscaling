using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ImageUpscaling.Scaling
{
    interface IScaling
    {
        BitmapSource ScaleImage(BitmapSource source, double scale);
    }
}
