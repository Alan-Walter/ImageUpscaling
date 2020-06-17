using ImageScaling.Helpers;

using System;

namespace ImageScaling.Interpolation
{
    /// <summary>
    /// Масштабирование Ланцоша
    /// </summary>
    public abstract class LanczosResampling : InterpolationAlgorithm
    {
        /// <summary>
        /// Порядок ядра
        /// </summary>
        protected abstract int KernelOrder { get; }

        protected override ByteImage ScaleImage(ByteImage source, double scale)
        {
            ByteImage image = new ByteImage(source, scale);
            double coef = (double)(source.Width) / image.Width;

            for (int x = 0; x < image.Width; ++x)
            {
                for (int y = 0; y < image.Height; ++y)
                {
                    double sX = x * coef - 0.5d;
                    double sY = y * coef - 0.5d;
                    int tempX = (int)Math.Floor(sX);
                    int tempY = (int)Math.Floor(sY);

                    double[] channelData = new double[source.BytePerPixel];
                    double weight = 0;

                    for (int fY = tempY - KernelOrder + 1; fY <= tempY + KernelOrder; ++fY)
                    {
                        if (fY < 0 || fY >= source.Height) continue;
                        for (int fX = tempX - KernelOrder + 1; fX <= tempX + KernelOrder; ++fX)
                        {
                            if (fX < 0 || fX >= source.Width) continue;

                            double wTemp = LanczosKernel(sX - fX) * LanczosKernel(sY - fY);
                            weight += wTemp;
                            for (int b = 0; b < source.BytePerPixel; ++b)
                            {
                                channelData[b] += source[fY, fX, b] * wTemp;
                            }
                        }
                    }
                    for (int b = 0; b < source.BytePerPixel; ++b)
                    {
                        image[y, x, b] = MathHelper.Clamp(channelData[b] / weight);
                    }
                }
            }

            return image;
        }

        /// <summary>
        /// Ядро Ланцоша
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private double LanczosKernel(double x)
        {
            if (Math.Abs(x) < KernelOrder)
                return MathHelper.Sinc(x) * MathHelper.Sinc(x / (double)KernelOrder);
            return 0;
        }
    }
}