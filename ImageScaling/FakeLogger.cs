namespace ImageScaling
{
    /// <summary>
    /// Фэйковый логгер
    /// </summary>
    internal class FakeLogger : IScaleLogger
    {
        public void Log(string text)
        {
        }

        public void Log(params string[] text)
        {
        }
    }
}