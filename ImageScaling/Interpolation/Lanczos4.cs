namespace ImageScaling.Interpolation
{
    /// <summary>
    /// Интерполяция Ланцоша с порядком 4
    /// </summary>
    public sealed class Lanczos4 : LanczosResampling
    {
        protected override int A => 4;
    }
}