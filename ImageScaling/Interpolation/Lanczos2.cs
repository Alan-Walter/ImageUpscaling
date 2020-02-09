namespace ImageScaling.Interpolation
{
    /// <summary>
    /// Интерполяция Ланцоша с порядком 2
    /// </summary>
    public sealed class Lanczos2 : LanczosResampling
    {
        protected override int A => 2;
    }
}