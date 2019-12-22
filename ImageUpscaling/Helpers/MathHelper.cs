using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageUpscaling.Helpers
{
    /// <summary>
    /// Математический хелпер
    /// </summary>
    public static class MathHelper
    {
        /// <summary>
        /// Привести значение к byte от 0 до 255
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Математическая функция Sinc
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double Sinc(double x)
        {
            if (x == 0)
                return 1;
            return Math.Sin(Math.PI * x) / (Math.PI * x);
        }
    }
}
