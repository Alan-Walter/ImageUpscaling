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
    /// Бикубическая интерполяция
    /// </summary>
    class BicubicInterpolation : IScaling
    {
        public string Title => "Бикубическая интерполяция";

        public bool IsScalable { get; } = true;

        public BitmapSource ScaleImage(BitmapSource source, double scale)
        {
            ByteImage sourceImage = ByteImage.FromBitmapSource(source);
            ByteImage image = new ByteImage(sourceImage, scale);
            double coef = (double)(sourceImage.Width - 1) / image.Width;

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
                        double first = Interpolate(sourceImage[tempY - 1, tempX - 1, i], 
                            sourceImage[tempY - 1, tempX, i], 
                            sourceImage[tempY - 1, tempX + 1, i], 
                            sourceImage[tempY - 1, tempX + 2, i], 
                            xDiff);
                        double second = Interpolate(sourceImage[tempY, tempX - 1, i], 
                            sourceImage[tempY, tempX, i], 
                            sourceImage[tempY, tempX + 1, i], 
                            sourceImage[tempY, tempX + 2, i], 
                            xDiff);
                        double third = Interpolate(sourceImage[tempY + 1, tempX - 1, i], 
                            sourceImage[tempY + 1, tempX, i], 
                            sourceImage[tempY + 1, tempX + 1, i], 
                            sourceImage[tempY + 1, tempX + 2, i], 
                            xDiff);
                        double fourth = Interpolate(sourceImage[tempY + 2, tempX - 1, i], 
                            sourceImage[tempY + 2, tempX, i], 
                            sourceImage[tempY + 2, tempX + 1, i], 
                            sourceImage[tempY + 2, tempX + 2, i], 
                            xDiff);

                        double value = Interpolate(first, second, third, fourth, yDiff);
                        image[y, x, i] = MathHelper.Clamp(value);
                    }
                }
            }

            return image.ToBitmapSource();
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
        static double Interpolate(double A, double B, double C, double D, double t)
        {
            double a = -A / 2.0d + (3.0d * B) / 2.0d - (3.0d * C) / 2.0d + D / 2.0d;
            double b = A - (5.0d * B) / 2.0d + 2.0d * C - D / 2.0d;
            double c = -A / 2.0d + C / 2.0d;
            double d = B;

            return a * t * t * t + b * t * t + c * t + d;
        }
    }
}
