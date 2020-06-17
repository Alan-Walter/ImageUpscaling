namespace ImageScaling
{
    /// <summary>
    /// Интерфейс алгоритма масштабирования с возможностью задать коэффициент масштабирования
    /// </summary>
    public interface IScalable
    {
        /// <summary>
        /// Масштаб увеличения
        /// </summary>
        double Scale { get; set; }
    }
}