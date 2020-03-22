namespace ImageScaling.PixelScaling
{
    /// <summary>
    /// Алгоритм масштабирования Scale4x
    /// </summary>
    internal class Scale4x : PixelScaleAlgorithm
    {
        /// <summary>
        /// Масштаб увеличения
        /// </summary
        protected override int Scale { get; } = 4;

        /// <summary>
        /// Масштабировать изображение
        /// </summary>
        /// <param name="byteImage">Входное изображение</param>
        /// <returns>Увеличенное изображение</returns>
        public override ByteImage ScaleImage(ByteImage byteImage)
        {
            var scale2X = new Scale2x();
            var part1 = scale2X.ScaleImage(byteImage);
            var result = scale2X.ScaleImage(part1);
            return result;
        }
    }
}