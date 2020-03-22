namespace ImageScaling.Interpolation
{
    /// <summary>
    /// Интерполяция Ланцоша с порядком 3
    /// </summary>
    public sealed class Lanczos3 : LanczosResampling
    {
        protected override int KernelOrder => 3;
    }
}