using System.Windows.Media.Imaging;

namespace ImageUpscaling.Scaling
{
    class NearestNeighbor : IScaling
    {
        public BitmapSource ScaleImage(BitmapSource source, double scale)
        {
            ByteImage sourceImage = ByteImage.FromBitmapSource(source);
            ByteImage image = new ByteImage(sourceImage, scale);
            double coef = 1 / scale;

            for(int y = 0; y < image.Height; ++y)
            {
                for(int x = 0; x < image.Width; ++x)
                {
                    int tempX = (int)(x * coef);
                    int tempY = (int)(y * coef);
                    for(int i = 0; i < image.BytePerPixel; ++i)
                    {
                        image[y, x, i] = sourceImage[tempY, tempX, i];
                    }
                }
            }

            return image.ToBitmapSource();
        }
    }
}
