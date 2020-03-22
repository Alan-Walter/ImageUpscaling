using ImageScaling;

using System;

namespace ImageUpscaling.ConsoleApp
{
    /// <summary>
    /// Логгер записей в консоль
    /// </summary>
    internal class ConsoleScaleLogger : IScaleLogger
    {
        public void Log(string text)
        {
            Console.WriteLine(text);
        }

        public void Log(params string[] text)
        {
            Console.WriteLine(string.Join(' ', text));
        }
    }
}