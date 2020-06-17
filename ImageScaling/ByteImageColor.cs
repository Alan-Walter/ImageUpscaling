using System;
using System.Collections.Generic;

namespace ImageScaling
{
    /// <summary>
    /// Цвет в ByteImage
    /// </summary>
    public struct ByteImageColor : IEquatable<ByteImageColor>
    {
        private readonly ByteImage byteImage;
        private readonly int x;
        private readonly int y;

        /// <summary>
        /// Создать цвет ByteImage
        /// </summary>
        /// <param name="byteImage">ByteImage</param>
        /// <param name="x">Позиция по X</param>
        /// <param name="y">Позиция по Y</param>
        public ByteImageColor(ByteImage byteImage, int x, int y)
        {
            this.byteImage = byteImage;
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Оператор эквивалентности
        /// </summary>
        /// <param name="first">Первый цвет</param>
        /// <param name="second">Второй цвет</param>
        /// <returns>Эквивалентность цветов</returns>
        public static bool operator ==(ByteImageColor first, ByteImageColor second)
        {
            if (first.byteImage != second.byteImage) return false;
            for (int i = 0; i < first.byteImage.BytePerPixel; ++i)
            {
                if (first[i] != second[i]) return false;
            }

            return true;
        }

        /// <summary>
        /// Оператор различия цветов
        /// </summary>
        /// <param name="first">Первый цвет</param>
        /// <param name="second">Второй цвет</param>
        /// <returns>Эквивалентность цветов</returns>
        public static bool operator !=(ByteImageColor first, ByteImageColor second)
        {
            return !(first == second);
        }

        /// <summary>
        /// Получить значение яркости по номеру канала
        /// </summary>
        /// <param name="channel">Номер канала</param>
        /// <returns>Яркость (в байтах)</returns>
        public byte this[int channel] => byteImage[y, x, channel];

        /// <summary>
        /// Эквивалентность объектов
        /// </summary>
        /// <param name="obj">Объект для сравнения</param>
        /// <returns>True, если объекты равны</returns>
        public override bool Equals(object obj)
        {
            return obj is ByteImageColor color && Equals(color);
        }

        /// <summary>
        /// Эквивалентность объектов
        /// </summary>
        /// <param name="other">Объект для сравнения</param>
        /// <returns>True, если объекты равны</returns>
        public bool Equals(ByteImageColor other)
        {
            return EqualityComparer<ByteImage>.Default.Equals(byteImage, other.byteImage) &&
                   x == other.x &&
                   y == other.y;
        }

        /// <summary>
        /// Вычислить Hash Code
        /// </summary>
        /// <returns>Хэш-код</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(byteImage, x, y);
        }
    }
}