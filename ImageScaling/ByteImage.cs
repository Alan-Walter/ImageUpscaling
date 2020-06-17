using Numpy;

using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageScaling
{
    /// <summary>
    /// Класс, представляющий изображение как массив байтов
    /// </summary>
    public class ByteImage
    {
        /// <summary>
        /// Массив байтов
        /// </summary>
        private readonly byte[] data;

        /// <summary>
        /// Смещение по массиву
        /// </summary>
        public int Stride { get; }

        /// <summary>
        /// Ширина
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Высота
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// DPI по горизонтали
        /// </summary>
        public float DpiX { get; } = 96f;

        /// <summary>
        /// DPI по вертикали
        /// </summary>
        public float DpiY { get; } = 96f;

        /// <summary>
        /// Количество байт на пиксель
        /// </summary>
        public int BytePerPixel { get; }

        /// <summary>
        /// Формат пикселей
        /// </summary>
        public PixelFormat PixelFormat { get; }

        /// <summary>
        /// Массив цветов
        /// </summary>
        public ColorPalette ColorPalette { get; set; }

        /// <summary>
        /// Формат изображения
        /// </summary>
        public ImageFormat ImageFormat { get; }

        /// <summary>
        /// Сформировать изображение в виде массива байтов
        /// </summary>
        /// <param name="data">Массив байтов</param>
        /// <param name="width">Ширина</param>
        /// <param name="height">Высота</param>
        /// <param name="pixelFormat">Формат пикселей</param>
        /// <param name="stride">Смещение</param>
        /// <param name="imageFormat">Формат изображения</param>
        public ByteImage(byte[] data, int width, int height, PixelFormat pixelFormat, int stride, ImageFormat imageFormat)
        {
            BytePerPixel = Image.GetPixelFormatSize(pixelFormat) / 8;
            //  если изображение имеет дополнительные байты
            if (stride % (width * BytePerPixel) != 0)
            {
                //  вычисляем новое смещение
                Stride = width * BytePerPixel;
                this.data = new byte[Stride * height];
                //  копируем части массива
                for (int y = 0; y < height; ++y)
                {
                    Array.Copy(data, y * stride, this.data, y * Stride, Stride);
                }
            }
            else
            {
                this.data = data;
                Stride = stride;
            }
            Width = width;
            Height = height;
            PixelFormat = pixelFormat;
            ImageFormat = imageFormat;
        }

        /// <summary>
        /// Сформировать изображение в виде массива байтов
        /// </summary>
        /// <param name="data">Массив байтов</param>
        /// <param name="width">Ширина</param>
        /// <param name="height">Высота</param>
        /// <param name="pixelFormat">Формат пикселей</param>
        /// <param name="stride">Смещение</param>
        /// <param name="imageFormat">Формат изображения</param>
        /// <param name="dpiX">Разрешение по горизонтали</param>
        /// <param name="dpiY">Разрешение по вертикали</param>
        public ByteImage(byte[] data, int width, int height, PixelFormat pixelFormat, int stride, ImageFormat imageFormat, float dpiX, float dpiY)
            : this(data, width, height, pixelFormat, stride, imageFormat)
        {
            DpiX = dpiX;
            DpiY = dpiY;
        }

        /// <summary>
        /// Конструктор, принимающий исходное изображение в формате ByteImage и коэффициент масштабирования
        /// </summary>
        /// <param name="source">Исходное изображение</param>
        /// <param name="scale">Масштаб увеличения</param>
        public ByteImage(ByteImage source, double scale)
        {
            Width = (int)Math.Round(source.Width * scale);
            Height = (int)Math.Round(source.Height * scale);
            Stride = source.BytePerPixel * Width;
            PixelFormat = source.PixelFormat;
            BytePerPixel = source.BytePerPixel;
            data = new byte[Height * Stride];
            DpiX = source.DpiX;
            DpiY = source.DpiY;
            ImageFormat = source.ImageFormat;
            ColorPalette = source.ColorPalette;
        }

        /// <summary>
        /// Получение значение пикселя
        /// </summary>
        /// <param name="y">Координата Y</param>
        /// <param name="x">Координата X</param>
        /// <param name="channel">Номер канала</param>
        /// <returns>Значение цвета</returns>
        public byte this[int y, int x, int channel]
        {
            get
            {
                if (x >= Width)
                {
                    x = Width - 1;
                }
                else if (x < 0)
                {
                    x = 0;
                }

                if (y >= Height)
                {
                    y = Height - 1;
                }
                else if (y < 0)
                {
                    y = 0;
                }

                if (channel >= BytePerPixel)
                {
                    throw new ArgumentException("Wrong channel number");
                }

                return data[y * Stride + x * BytePerPixel + channel];
            }
            set
            {
                data[y * Stride + x * BytePerPixel + channel] = value;
            }
        }

        /// <summary>
        /// Получить или установить цвет в точке y, x
        /// </summary>
        /// <param name="y">Позиция по Y</param>
        /// <param name="x">Позиция по X</param>
        /// <returns>Цвет</returns>
        public ByteImageColor this[int y, int x]
        {
            get => new ByteImageColor(this, x, y);
            set
            {
                for (int i = 0; i < BytePerPixel; ++i)
                {
                    this[y, x, i] = value[i];
                }
            }
        }

        /// <summary>
        /// Получить массив байт bitmap
        /// </summary>
        /// <param name="bitmapStride">Смещение bitmap</param>
        /// <returns>Массив байт</returns>
        public byte[] GetBitmapData(int bitmapStride)
        {
            byte[] bitmapData;
            if (bitmapStride != Stride)
            {
                bitmapData = new byte[bitmapStride * Height];
                for (int y = 0; y < Height; ++y)
                {
                    Array.Copy(data, y * Stride, bitmapData, y * bitmapStride, Stride);
                }
            }
            else
            {
                bitmapData = data;
            }
            return bitmapData;
        }

        /// <summary>
        /// Преобразовать в NDarray
        /// </summary>
        /// <returns>Массив NDarray</returns>
        public NDarray ToNDarray()
        {
            var temp = new float[data.Length];
            for (int i = 0; i < data.Length; i += 3)
            {
                temp[i] = data[i + 2];
                temp[i + 1] = data[i + 1];
                temp[i + 2] = data[i];
            }
            var ndarray = np.array(temp).reshape(Height, Width, BytePerPixel);
            ndarray = ndarray / 127.5 - 1;
            return ndarray;
        }
    }
}