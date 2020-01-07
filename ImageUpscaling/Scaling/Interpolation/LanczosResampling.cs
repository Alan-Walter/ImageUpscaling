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
    abstract class LanczosResampling : IScaling
    {
        protected abstract int A { get; }

        public string Title => $"Фильтр Ланцоша {A}";

        public bool IsScalable { get; } = true;

        public BitmapSource ScaleImage(BitmapSource source, double scale)
        {
            ByteImage sourceImage = ByteImage.FromBitmapSource(source);
            ByteImage image = new ByteImage(sourceImage, scale);
            double coef = 1 / scale;

            for (int x = 0; x < image.Width; ++x)
            {
                for (int y = 0; y < image.Height; ++y)
                {
                    int tempX = (int)(x * coef);
                    int tempY = (int)(y * coef);

                    double[] channelData = new double[sourceImage.BytePerPixel];
                    double weight = 0;

                    for (int fY = tempY - A + 1; fY <= tempY + A; ++fY)
                    {
                        if (fY < 0 || fY >= sourceImage.Height) continue;
                        for (int fX = tempX - A + 1; fX <= tempX + A; ++fX)
                        {
                            if (fX < 0 || fX >= sourceImage.Width) continue;

                            double wTemp = LanczosKernel(x * coef - fX) * LanczosKernel(y * coef - fY);
                            weight += wTemp;
                            for (int b = 0; b < sourceImage.BytePerPixel; ++b)
                            {
                                channelData[b] += sourceImage[fY, fX, b] * wTemp;
                            }
                        }
                    }
                    for (int b = 0; b < sourceImage.BytePerPixel; ++b)
                    {
                        image[y, x, b] = MathHelper.Clamp(channelData[b] / weight);
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