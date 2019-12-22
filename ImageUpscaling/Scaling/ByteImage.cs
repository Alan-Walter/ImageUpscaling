using System;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ImageUpscaling.Extensions;
using ImageUpscaling.Helpers;

namespace ImageUpscaling.Scaling
{
    class ByteImage
    {
        readonly byte[] data;
        PixelFormat pixelFormat;
        private readonly int stride;

        public int Width { get; }
        public int Height { get; }
        public int BytePerPixel { get; }
        public ByteImage(byte[] data, int width, int height, PixelFormat pixelFormat)
        {
            this.data = data;
            Width = width;
            Height = height;
            stride = BitmapHelper.GetStride(width, pixelFormat.BitsPerPixel);
            this.pixelFormat = pixelFormat;
            BytePerPixel = stride / Width;
        }

        public ByteImage(ByteImage source, double scale)
        {
            Width = (int)Math.Round(source.Width * scale);
            Height = (int)Math.Round(source.Height * scale);
            stride = BitmapHelper.GetStride(Width, source.pixelFormat.BitsPerPixel);
            pixelFormat = source.pixelFormat;
            BytePerPixel = stride / Width;
            data = new byte[Height * stride];
        }

        public static ByteImage FromBitmapSource(BitmapSource bitmapSource)
        {
            return new ByteImage(
                bitmapSource.GetBuffer(),
                bitmapSource.PixelWidth,
                bitmapSource.PixelHeight,
                bitmapSource.Format
            );
        }

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


        public BitmapSource ToBitmapSource()
        {
            return BitmapSource.Create(Width, Height, 96, 96, pixelFormat, null, data, stride);
        }
    }
}
