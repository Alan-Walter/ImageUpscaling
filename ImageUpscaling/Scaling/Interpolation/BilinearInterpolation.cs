using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ImageUpscaling.Extensions;
using ImageUpscaling.Helpers;

namespace ImageUpscaling.Scaling.Interpolation
{
    /// <summary>
    /// Билинейная интерполяция
    /// </summary>
    class BilinearInterpolation : IScaling
    {
        public string Title => "Билинейная интерполяция";

        public bool IsScalable { get; } = true;

        public BitmapSource ScaleImage(BitmapSource source, double scale)
        {
            ByteImage sourceImage = ByteImage.FromBitmapSource(source);
            ByteImage image = new ByteImage(sourceImage, scale);
            double coef = 1 / scale;

            for (int y = 0; y < image.Height; ++y)
            {
                for (int x = 0; x < image.Width; ++x)
                {
                    int tempX = (int)(x * coef);
                    int tempY = (int)(y * coef);

                    double xDiff = x * coef - tempX;
                    double yDiff = y * coef - tempY;

                    for (int i = 0; i < image.BytePerPixel; ++i)
                    {
                        byte a = sourceImage[tempY, tempX, i];
                        byte b = sourceImage[tempY, tempX + 1, i];
                        byte c = sourceImage[tempY + 1, tempX, i];
                        byte d = sourceImage[tempY + 1, tempX + 1, i];

                        byte val = (byte)(a * (1 - yDiff) * (1 - xDiff)
                            + b * (1 - yDiff) * xDiff
                            + c * yDiff * (1 - xDiff)
                            + d * yDiff * xDiff);

                        image[y, x, i] = val;
                    }
                }
            }

            return image.ToBitmapSource();
        }
    }
}
