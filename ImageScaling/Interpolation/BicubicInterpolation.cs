using System;

using ImageScaling.Helpers;

namespace ImageScaling.Interpolation
{
    /// <summary>
    /// Бикубическая интерполяция
    /// </summary>
    public sealed class BicubicInterpolation : IScaling
    {
        public ByteImage ScaleImage(ByteImage source, double scale)
        {
            ByteImage image = new ByteImage(source, scale);
            double coef = (double)(source.Width) / image.Width;

            for (int y = 0; y < image.Height; ++y)
            {
                for (int x = 0; x < image.Width; ++x)
                {
                    double sX = x * coef - 0.5d;
                    double sY = y * coef - 0.5d;
                    int tempX = (int)Math.Floor(sX);
                    int tempY = (int)Math.Floor(sY);

                    double xDiff = sX - Math.Floor(sX);
                    double yDiff = sY - Math.Floor(sY);

                    for (int i = 0; i < image.BytePerPixel; ++i)
                    {
                        double first = Interpolate(source[tempY - 1, tempX - 1, i],
                            source[tempY - 1, tempX, i],
                            source[tempY - 1, tempX + 1, i],
                            source[tempY - 1, tempX + 2, i],
                            xDiff);
                        double second = Interpolate(source[tempY, tempX - 1, i],
                            source[tempY, tempX, i],
                            source[tempY, tempX + 1, i],
                            source[tempY, tempX + 2, i],
                            xDiff);
                        double third = Interpolate(source[tempY + 1, tempX - 1, i],
                            source[tempY + 1, tempX, i],
                            source[tempY + 1, tempX + 1, i],
                            source[tempY + 1, tempX + 2, i],
                            xDiff);
                        double fourth = Interpolate(source[tempY + 2, tempX - 1, i],
                            source[tempY + 2, tempX, i],
                            source[tempY + 2, tempX + 1, i],
                            source[tempY + 2, tempX + 2, i],
                            xDiff);

                        double value = Interpolate(first, second, third, fourth, yDiff);
                        image[y, x, i] = MathHelper.Clamp(value);
                    }
                }
            }

            return image;
        }

        /// <summary>
        /// Функция интерполяции
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <param name="D"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        private static double Interpolate(double A, double B, double C, double D, double t)
        {
            double a = -A / 2.0d + (3.0d * B) / 2.0d - (3.0d * C) / 2.0d + D / 2.0d;
            double b = A - (5.0d * B) / 2.0d + 2.0d * C - D / 2.0d;
            double c = -A / 2.0d + C / 2.0d;
            double d = B;

            return a * t * t * t + b * t * t + c * t + d;
        }
    }
}