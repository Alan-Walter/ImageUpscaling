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
        /// <param name="width">Ширина изображения</param>
        /// <param name="bytePerPixel">Количество байт на пиксель</param>
        /// <returns>Смещение (длина в байтах)</returns>
        public static int GetStride(int width, int bytePerPixel)
        {
            return (width * bytePerPixel) + ((width * bytePerPixel) % 4);
        }

        /// <summary>
        /// Конвертировать в формат byte
        /// </summary>
        /// <param name="val">Значение от -1 до 1</param>
        /// <returns>Значение в формате byte от 0 до 255</returns>
        public static byte ConvertToByteFormat(double val)
        {
            return Clamp((val + 1) * 127.5);
        }
    }
}