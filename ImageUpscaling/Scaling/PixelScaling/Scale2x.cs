using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ImageUpscaling.Scaling.PixelScaling
{
    /// <summary>
    /// Scale2x
    /// </summary>
    class Scale2x : IScaling
    {
        public string Title => "Scale 2x";

        public bool IsScalable { get; } = false;

        public BitmapSource ScaleImage(BitmapSource source, double scale)
        {
            ByteImage sourceImage = ByteImage.FromBitmapSource(source);
            ByteImage image = new ByteImage(sourceImage, 2);

            for (int y = 0; y < sourceImage.Height; ++y)
            {
                for (int x = 0; x < sourceImage.Width; ++x)
                {
                    var B = sourceImage[y - 1, x];
                    var H = sourceImage[y + 1, x];
                    var D = sourceImage[y, x - 1];
                    var F = sourceImage[y, x + 1];
                    var E = sourceImage[y, x];
                    if (B != H && D != F)
                    {
                        image[y * 2, x * 2] = D == B ? D : E;
                        image[y * 2, x * 2 + 1] = B == F ? F : E;
                        image[y * 2 + 1, x * 2] = D == H ? D : E;
                        image[y * 2 + 1, x * 2 + 1] = H == F ? F : E;
                    }
                    else
                    {
                        image[y * 2, x * 2] = E;
                        image[y * 2, x * 2 + 1] = E;
                        image[y * 2 + 1, x * 2] = E;
                        image[y * 2 + 1, x * 2 + 1] = E;
                    }
                }
            }

            return image.ToBitmapSource();
        }
    }
}
