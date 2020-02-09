using System;
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
        /// Формат изображения
        /// </summary>
        public ImageFormat ImageFormat { get; }

        public ByteImage(byte[] data, int width, int height, PixelFormat pixelFormat, int stride, ImageFormat imageFormat)
        {
            this.data = data;
            Width = width;
            Height = height;
            PixelFormat = pixelFormat;
            this.Stride = stride;
            BytePerPixel = stride / Width;
            ImageFormat = imageFormat;
        }

        public ByteImage(byte[] data, int width, int height, PixelFormat pixelFormat, int stride, ImageFormat imageFormat, float dpiX, float dpiY) 
            : this(data, width, height, pixelFormat, stride, imageFormat)
        {
            DpiX = dpiX;
            DpiY = dpiY;
        }

        /// <summary>
        /// Конструктор, принимающий исходное изображение в формате ByteImage и коэффициент масштабирования
        /// </summary>
        /// <param name="source"></param>
        /// <param name="scale"></param>
        public ByteImage(ByteImage source, double scale)
        {
            Width = (int)Math.Round(source.Width * scale);
            Height = (int)Math.Round(source.Height * scale);
            Stride = source.Stride / source.Width * Width;
            PixelFormat = source.PixelFormat;
            BytePerPixel = Stride / Width;
            data = new byte[Height * Stride];
            DpiX = source.DpiX;
            DpiY = source.DpiY;
            ImageFormat = source.ImageFormat;
        }

        /// <summary>
        /// Получение значение пикселя
        /// </summary>
        /// <param name="y">Координата Y</param>
        /// <param name="x">Координата X</param>
        /// <param name="channel">Номер канала</param>
        /// <returns></returns>
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

        public byte[] GetData()
        {
            return data;
        }
    }
}