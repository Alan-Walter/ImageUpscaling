using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageUpscaling.Scaling
{
    /// <summary>
    /// Структура с информацией о пикселе
    /// </summary>
    struct PixelInfo : IEquatable<PixelInfo>
    {
        private byte[] source;

        int offset;

        /// <summary>
        /// Число байтов на пиксель
        /// </summary>
        public int Bytes { get; private set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="source">Исходный массив</param>
        /// <param name="offset">Смещение</param>
        /// <param name="bytes">Число байтов</param>
        public PixelInfo(byte[] source, int offset, int bytes)
        {
            this.source = source;
            this.offset = offset;
            Bytes = bytes;
        }

        /// <summary>
        /// Получение байта по его номеру
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public byte this[int index]
        {
            get
            {
                if (index < 0 || index >= Bytes)
                    throw new IndexOutOfRangeException();

                return source[offset + index];
            }
        }

        public static bool operator ==(PixelInfo first, PixelInfo second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(PixelInfo first, PixelInfo second)
        {
            return !first.Equals(second);
        }

        public override bool Equals(object obj)
        {
            return obj is PixelInfo info && Equals(info);
        }

        public bool Equals(PixelInfo other)
        {
            if (this.Bytes != other.Bytes) return false;
            for (int i = 0; i < Bytes; ++i)
                if (this[i] != other[i]) return false;

            return true;
        }

        public override int GetHashCode()
        {
            var hashCode = -1103474690;
            hashCode = hashCode * -1521134295 + offset.GetHashCode();
            hashCode = hashCode * -1521134295 + Bytes.GetHashCode();
            return hashCode;
        }
    }
}
