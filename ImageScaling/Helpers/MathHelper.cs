using System;

namespace ImageScaling.Helpers
{
    /// <summary>
    /// Математический хелпер
    /// </summary>
    internal static class MathHelper
    {
        /// <summary>
        /// Привести значение к byte от 0 до 255
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static byte Clamp(double val)
        {
            if (val > 255.0)
                val = (byte)255;
            else if (val < 0.0)
                val = (byte)0;
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

        /// <summary>
        /// Вычисление смещения
        /// </summary>
        /// <param name="width"></param>
        /// <param name="bitsPerPixel"></param>
        /// <returns></returns>
        public static int GetStride(int width, int bitsPerPixel)
        {
            return (width * bitsPerPixel + 31) / 32 * 4;
        }
    }
}