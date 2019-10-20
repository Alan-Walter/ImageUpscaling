using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ImageUpscaling.Scaling.Interpolation
{
    class BicubicInterpolation : IInterpolationScaling
    {
        public string Title => "Бикубическая интерполяция";

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
                        image[y, x, i] = Clamp(value);
                    }
                }
            }

            return image.ToBitmapSource();
        }

        double Interpolate(double A, double B, double C, double D, double t)
        {
            double a = -A / 2.0f + (3.0f * B) / 2.0f - (3.0f * C) / 2.0f + D / 2.0f;
            double b = A - (5.0f * B) / 2.0f + 2.0f * C - D / 2.0f;
            double c = -A / 2.0f + C / 2.0f;
            double d = B;

            return a * t * t * t + b * t * t + c * t + d;
        }

        static byte Clamp(double val)
        {
            if (val > 255)
                val = 255;
            else if (val < 0)
                val = 0;
            return (byte)val;
        }
    }
}
