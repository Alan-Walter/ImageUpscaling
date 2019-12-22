using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageUpscaling.Helpers
{
    /// <summary>
    /// Bitmap хелпер
    /// </summary>
    public static class BitmapHelper
    {
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
