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
            if (double.IsNaN(val))
                return 0;
            if (val > 255)
                val = 255;
            else if (val < 0)
                val = 0;
            return (byte)val;
        }

        public static double Sinc(double x)
        {
            if (x == 0)
                return 1;
            return Math.Sin(Math.PI * x) / (Math.PI * x);
        }
    }
}
