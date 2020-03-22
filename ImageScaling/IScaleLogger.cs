namespace ImageScaling
{
    /// <summary>
    /// Логгер для масштабирования
    /// </summary>
    public interface IScaleLogger
    {
        /// <summary>
        /// Записать текст в лог
        /// </summary>
        /// <param name="text">Входная информация</param>
        void Log(string text);

        /// <summary>
        /// Записать текст в лог
        /// </summary>
        /// <param name="text">Входная информация</param>
        void Log(params string[] text);
    }
}