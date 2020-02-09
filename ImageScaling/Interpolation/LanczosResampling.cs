using System;

using ImageScaling.Helpers;

namespace ImageScaling.Interpolation
{
    /// <summary>
    /// Масштабирование Ланцоша
    /// </summary>
    public abstract class LanczosResampling : IScaling
    {
        /// <summary>
        /// Порядок ядра
        /// </summary>
        protected abstract int A { get; }

        public ByteImage ScaleImage(ByteImage source, double scale)
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

                    for (int fY = tempY - A + 1; fY <= tempY + A; ++fY)
                    {
                        if (fY < 0 || fY >= source.Height) continue;
                        for (int fX = tempX - A + 1; fX <= tempX + A; ++fX)
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
            if (Math.Abs(x) < A)
                return MathHelper.Sinc(x) * MathHelper.Sinc(x / (double)A);
            return 0;
        }
    }
}