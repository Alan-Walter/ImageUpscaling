using System;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ImageUpscaling.Extensions;
using ImageUpscaling.Helpers;

namespace ImageUpscaling.Scaling
{
    /// <summary>
    /// Класс, представляющий изображение как массив байтов
    /// </summary>
    public class ByteImage
    {
        readonly double dpiX, dpiY;
        readonly byte[] data;
        PixelFormat pixelFormat;
        private readonly int stride;
        readonly BitmapPalette bitmapPalette;

        /// <summary>
        /// Ширина
        /// </summary>
        public int Width { get; }
        /// <summary>
        /// Высота
        /// </summary>
        public int Height { get; }
        /// <summary>
        /// Количество байт на пиксель
        /// </summary>
        public int BytePerPixel { get; }

        /// <summary>
        /// Конструктор, принимающий массив байтов, размеры и формат пикселя
        /// </summary>
        /// <param name="data"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="pixelFormat"></param>
        private ByteImage(byte[] data, int width, int height, PixelFormat pixelFormat, BitmapPalette bitmapPalette, double dpiX, double dpiY)
        {
            this.data = data;
            Width = width;
            Height = height;
            stride = BitmapHelper.GetStride(width, pixelFormat.BitsPerPixel);
            this.pixelFormat = pixelFormat;
            BytePerPixel = stride / Width;
            this.bitmapPalette = bitmapPalette;
            this.dpiX = dpiX;
            this.dpiY = dpiY;
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
            stride = BitmapHelper.GetStride(Width, source.pixelFormat.BitsPerPixel);
            pixelFormat = source.pixelFormat;
            BytePerPixel = stride / Width;
            data = new byte[Height * stride];
            this.bitmapPalette = source.bitmapPalette;
        }

        /// <summary>
        /// Получение ByteImage из BitmapSource
        /// </summary>
        /// <param name="bitmapSource"></param>
        /// <returns></returns>
        public static ByteImage FromBitmapSource(BitmapSource bitmapSource)
        {
            return new ByteImage(
                bitmapSource.GetBuffer(),
                bitmapSource.PixelWidth,
                bitmapSource.PixelHeight,
                bitmapSource.Format,
                bitmapSource.Palette,
                bitmapSource.DpiX,
                bitmapSource.DpiY
            );
        }

        /// <summary>
        /// Получение значение пикселя
        /// </summary>
        /// <param name="y">Координата Y</param>
        /// <param name="x">Координата X</param>
        /// <param name="index">Номер канала</param>
        /// <returns></returns>
        public byte this[int y, int x, int index]
        {
            get
            {
                if (x >= Width)
                    x = Width - 1;
                else if (x < 0)
                    x = 0;
                if (y >= Height)
                    y = Height - 1;
                else if (y < 0)
                    y = 0;
                

                return data[y * stride + x * BytePerPixel + index];
            }
            set
            {
                data[y * stride + x * BytePerPixel + index] = value;
            }
        }

        /// <summary>
        /// Получение значения пикселя по двум его координатам
        /// </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public PixelInfo this[int y, int x]
        {
            get
            {
                if (x >= Width)
                    x = Width - 1;
                else if (x < 0)
                    x = 0;
                if (y >= Height)
                    y = Height - 1;
                else if (y < 0)
                    y = 0;

                return new PixelInfo(data, y * stride + x * BytePerPixel, BytePerPixel);
            }
            set
            {
                if (value.Bytes != BytePerPixel)
                    throw new ArgumentException();

                for (int i = 0; i < BytePerPixel; ++i)
                {
                    data[y * stride + x * BytePerPixel + i] = value[i];
                }
            }
        }

        /// <summary>
        /// Преобразование ByteImage к BitmapSource
        /// </summary>
        /// <returns></returns>
        public BitmapSource ToBitmapSource()
        {
            return BitmapSource.Create(Width, Height, dpiX, dpiY, pixelFormat, bitmapPalette, data, stride);
        }
    }
}
