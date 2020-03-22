using ImageScaling;

using System;
using System.Text;

namespace ImageUpscaling.Desktop.Core.ViewModel
{
    /// <summary>
    /// ViewModel для алгоритма масштабирования
    /// </summary>
    internal class ScaleImageViewModel : BaseViewModel
    {
        /// <summary>
        /// Название алгоритма
        /// </summary>
        private readonly string title;

        /// <summary>
        /// Тип алгоритма масштабирования
        /// </summary>
        public Type ScaleImageType { get; }

        /// <summary>
        /// Масштабируемость алгоритма изображения
        /// </summary>
        public bool IsScalable
        {
            get => typeof(IScalable).IsAssignableFrom(ScaleImageType);
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="scaleImageType">Тип алгоритма</param>
        public ScaleImageViewModel(Type scaleImageType)
        {
            ScaleImageType = scaleImageType;
            title = SplitName(ScaleImageType.Name);
        }

        public override string ToString()
        {
            return title;
        }

        /// <summary>
        /// Разбить имя класса для названия
        /// </summary>
        /// <param name="name">Имя класса</param>
        /// <returns>Разбитая через пробел строка</returns>
        private static string SplitName(string name)
        {
            StringBuilder stringBuilder = new StringBuilder(name.Length);
            bool lastUpper = true;
            foreach (var @char in name)
            {
                if ((char.IsUpper(@char) || char.IsDigit(@char)) && !lastUpper)
                {
                    stringBuilder.Append(' ');
                }

                stringBuilder.Append(@char);
                lastUpper = char.IsUpper(@char);
            }

            return stringBuilder.ToString();
        }
    }
}