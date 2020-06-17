namespace ImageScaling.Interpolation
{
    /// <summary>
    /// Метод ближайшего соседа
    /// </summary>
    public class NearestNeighbor : InterpolationAlgorithm
    {
        protected override ByteImage ScaleImage(ByteImage source, double scale)
        {
            ByteImage image = new ByteImage(source, scale);
            double coef = 1 / scale;

            for (int y = 0; y < image.Height; ++y)
            {
                for (int x = 0; x < image.Width; ++x)
                {
                    int tempX = (int)(x * coef);
                    int tempY = (int)(y * coef);
                    for (int i = 0; i < image.BytePerPixel; ++i)
                    {
                        image[y, x, i] = source[tempY, tempX, i];
                    }
                }
            }

            return image;
        }
    }
}