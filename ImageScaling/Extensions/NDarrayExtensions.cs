using ImageScaling.Helpers;

using Numpy;

using System;
using System.Drawing.Imaging;
using System.Linq;

namespace ImageScaling.Extensions
{
    /// <summary>
    /// Расширения класса NDarray
    /// </summary>
    public static class NDarrayExtensions
    {
        /// <summary>
        /// Преобразовать в ByteImage
        /// </summary>
        /// <param name="nDarray">Массив NDarray</param>
        /// <returns>Объект ByteImage</returns>
        public static ByteImage ToByteImage(this NDarray nDarray)
        {
            var resultShape = nDarray.shape;
            var height = resultShape[0];
            var width = resultShape[1];
            var bytePerPixel = resultShape[2];
            var stride = bytePerPixel * width;
            var pixelFormat = Enum.GetValues(typeof(PixelFormat))
                .Cast<PixelFormat>()
                .FirstOrDefault(i => i.ToString().Contains((bytePerPixel * 8).ToString()));
            var arr = nDarray.reshape(height * width * bytePerPixel).GetData<float>();
            var data = new byte[arr.Length];
            for (int i = 0; i < arr.Length; i += 3)
            {
                data[i] = MathHelper.ConvertToByteFormat(arr[i + 2]);
                data[i + 1] = MathHelper.ConvertToByteFormat(arr[i + 1]);
                data[i + 2] = MathHelper.ConvertToByteFormat(arr[i]);
            }

            return new ByteImage(data, width, height, pixelFormat, stride, null);
        }
    }
}