using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageUpscaling.Helpers
{
    public static class BitmapHelper
    {
        public static int GetStride(int width, int bitsPerPixel)
        {
            return (width * bitsPerPixel + 31) / 32 * 4;
        }
    }
}
