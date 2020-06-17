namespace ImageScaling.PixelScaling
{
    /// <summary>
    /// Алгоритм Scale2x
    /// </summary>
    public class Scale2x : PixelScaleAlgorithm
    {
        /// <summary>
        /// Масштаб увеличения
        /// </summary
        protected override int Scale { get; } = 2;

        /// <summary>
        /// Масштабировать изображение
        /// </summary>
        /// <param name="byteImage">Входное изображение</param>
        /// <returns>Увеличенное изображение</returns>
        public override ByteImage ScaleImage(ByteImage byteImage)
        {
            var image = new ByteImage(byteImage, Scale);

            for (int y = 0; y < byteImage.Height; ++y)
            {
                for (int x = 0; x < byteImage.Width; ++x)
                {
                    var B = byteImage[y - 1, x];
                    var D = byteImage[y, x - 1];
                    var E = byteImage[y, x];
                    var F = byteImage[y, x + 1];
                    var H = byteImage[y + 1, x];
                    if (B != H && D != F)
                    {
                        image[y * Scale, x * Scale] = D == B ? D : E;
                        image[y * Scale, x * Scale + 1] = B == F ? F : E;
                        image[y * Scale + 1, x * Scale] = D == H ? D : E;
                        image[y * Scale + 1, x * Scale + 1] = H == F ? F : E;
                    }
                    else
                    {
                        image[y * Scale, x * Scale] = E;
                        image[y * Scale, x * Scale + 1] = E;
                        image[y * Scale + 1, x * Scale] = E;
                        image[y * Scale + 1, x * Scale + 1] = E;
                    }
                }
            }

            return image;
        }
    }
}