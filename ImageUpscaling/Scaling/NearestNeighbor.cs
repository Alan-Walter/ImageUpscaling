using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ImageUpscaling.Extensions;
using ImageUpscaling.Helpers;

namespace ImageUpscaling.Scaling
{
    class NearestNeighbor : BaseScaling
    {
        public override BitmapSource ScaleImage(BitmapSource source, double scale)
        {
            int width = (int)Math.Round(source.PixelWidth * scale);
            int height = (int)Math.Round(source.PixelHeight * scale);

            double coef = 1 / scale;
            int stride = BitmapHelper.GetStride(width, source.Format.BitsPerPixel);
            int oldStride = BitmapHelper.GetStride(source.PixelWidth, source.Format.BitsPerPixel);
            int bytes = stride / width;

            byte[] buffer = source.GetBuffer();
            byte[] arr = new byte[height * stride];

            for(int y = 0; y < height; ++y)
            {
                for(int x = 0; x < width; ++x)
                {
                    int index = GetPosition(x, y, stride, bytes);
                    int oldIndex = (int)(y * coef) * oldStride + (int)(x * coef) * bytes;
                    for(int i = 0; i < bytes; ++i)
                    {
                        arr[index + i] = buffer[oldIndex + i];
                    }
                }
            }

            return BitmapSource.Create(width, height, 96, 96, source.Format, null, arr, stride);
        }
    }
}
