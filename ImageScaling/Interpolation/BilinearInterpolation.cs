using ImageScaling.Helpers;

using System;

namespace ImageScaling.Interpolation
{
    /// <summary>
    /// Билинейная интерполяция
    /// </summary>
    public sealed class BilinearInterpolation : InterpolationAlgorithm
    {
        protected override ByteImage ScaleImage(ByteImage source, double scale)
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
                        byte a = source[tempY, tempX, i];
                        byte b = source[tempY, tempX + 1, i];
                        byte c = source[tempY + 1, tempX, i];
                        byte d = source[tempY + 1, tempX + 1, i];

                        byte val = MathHelper.Clamp(a * (1 - yDiff) * (1 - xDiff)
                            + b * (1 - yDiff) * xDiff
                            + c * yDiff * (1 - xDiff)
                            + d * yDiff * xDiff);

                        image[y, x, i] = val;
                    }
                }
            }

            return image;
        }
    }
}