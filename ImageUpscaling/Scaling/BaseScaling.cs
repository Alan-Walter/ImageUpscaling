using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ImageUpscaling.Scaling
{
    abstract class BaseScaling : IScaling
    {
        public abstract BitmapSource ScaleImage(BitmapSource source, double scale);

        protected static int GetPosition(int x, int y, int stride, int bytes)
        {
            return y * stride + x * bytes;
        }

        protected static int GetPosition(int x, int y, int stride, int bytes, double scale)
        {
            return (int)(y * scale) * stride + (int)(x * scale) * bytes;
        }
    }
}
