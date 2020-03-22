namespace ImageScaling.PixelScaling
{
    /// <summary>
    /// Алгоритм Scale 3x
    /// </summary>
    public class Scale3x : PixelScaleAlgorithm
    {
        /// <summary>
        /// Масштаб увеличения
        /// </summary>
        protected override int Scale { get; } = 3;

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
                    var A = byteImage[y - 1, x - 1];
                    var B = byteImage[y - 1, x];
                    var C = byteImage[y - 1, x + 1];

                    var D = byteImage[y, x - 1];
                    var E = byteImage[y, x];
                    var F = byteImage[y, x + 1];

                    var G = byteImage[y + 1, x - 1];
                    var H = byteImage[y + 1, x];
                    var I = byteImage[y + 1, x + 1];

                    var xS = x * Scale;
                    var yS = y * Scale;

                    if (B != H && D != F)
                    {
                        image[yS, xS] = D == B ? D : E;
                        image[yS, xS + 1] = (D == B && E != C) || (B == F && E != A) ? B : E;
                        image[yS, xS + 2] = B == F ? F : E;
                        image[yS + 1, xS] = (D == B && E != G) || (D == H && E != A) ? D : E;
                        image[yS + 1, xS + 1] = E;
                        image[yS + 1, xS + 2] = (B == F && E != I) || (H == F && E != C) ? F : E;
                        image[yS + 2, xS] = D == H ? D : E;
                        image[yS + 2, xS + 1] = (D == H && E != I) || (H == F && E != G) ? H : E;
                        image[yS + 2, xS + 2] = H == F ? F : E;
                    }
                    else
                    {
                        for (int j = 0; j < Scale; ++j)
                        {
                            for (int i = 0; i < Scale; ++i)
                            {
                                image[yS + j, xS + i] = E;
                            }
                        }
                    }
                }
            }

            return image;
        }
    }
}