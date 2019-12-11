using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageUpscaling.Helpers
{
    public static class MathHelper
    {
        public static byte Clamp(double val)
        {
            if (val > 255)
                val = 255;
            else if (val < 0)
                val = 0;
            return (byte)val;
        }
    }
}
