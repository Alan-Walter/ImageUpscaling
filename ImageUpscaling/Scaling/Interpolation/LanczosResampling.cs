using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ImageUpscaling.Helpers;

namespace ImageUpscaling.Scaling.Interpolation
{
    /// <summary>
    /// Масштабирование Ланцоша
    /// </summary>
    class LanczosResampling : IScaling
    {
        int A { get; } = 3;

        public string Title => $"Фильтр Ланцоша {A}";

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

                    for (int b = 0; b < image.BytePerPixel; ++b)
                    {
                        double temp = 0;
                        double w = 0;
                        for (int j = -A + 1; j <= A; ++j)
                        {
                            if (tempY + j < 0 || tempY + j >= sourceImage.Height) continue;
                            for (int i = -A + 1; i <= A; ++i)
                            {
                                if (tempX + i < 0 || tempX + i >= sourceImage.Width) continue;
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

        private double LanczosKernel(double x)
        {
            if (Math.Abs(x) < A)
                return MathHelper.Sinc(x) * MathHelper.Sinc(x / (double)A);
            return 0;
        }
    }
}
