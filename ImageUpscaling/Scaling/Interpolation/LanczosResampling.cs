using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

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

                    for (int i = 0; i < image.BytePerPixel; ++i)
                    {
                        image[y, x, i] = 
                    }
                }
            }

            return image.ToBitmapSource();
        }

        private static double LanczosKernel(double x)
        {
            if (x == 0)
            {
                return 1;
            } else if (-a <= x && x < a)
            {
                return (a * Math.Sin(Math.PI * x) * Math.Sin(Math.PI * x / a)) / (Math.PI * Math.PI * x * x);
            }
            else
            {
                return 0;
            }
        }

        private static double TwoDimensionalLanczos(double x, double y)
        {
            return LanczosKernel(x) * LanczosKernel(y);
        }
    }
}
