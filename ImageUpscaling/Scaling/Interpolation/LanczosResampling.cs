using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ImageUpscaling.Helpers;

namespace ImageUpscaling.Scaling.Interpolation
{
    class LanczosResampling : IInterpolationScaling
    {
        const int a = 3;

        public string Title => "Фильтр Ланцоша";

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

                    for (int b = 0; b < image.BytePerPixel; ++b)
                    {
                        double temp = 0;
                        double w = 0;
                        for (int i = -a + 1; i < a; ++i)
                        {
                            for (int j = -a + 1; j < a; ++j)
                            {
                                double wTemp = LanczosKernel(i + xDiff) * LanczosKernel(j + yDiff);
                                temp += sourceImage[tempY + j, tempX + i, b] * wTemp;
                                w += wTemp;
                            }
                        }

                        image[y, x, b] = MathHelper.Clamp(temp / w);
                    }
                }
            }

            return image.ToBitmapSource();
        }

        private static double LanczosKernel(double x)
        {
            if (Math.Abs(x) <= a)
                return MathHelper.Sinc(x) * MathHelper.Sinc(x / a);
            return 0;
        }
    }
}
