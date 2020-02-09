using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace ImageScaling.IO
{
    public static class ByteImageFileManager
    {
        public static ByteImage ReadFile(string path)
        {
            var bitmap = Bitmap.FromFile(path) as Bitmap;
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
            var data = new byte[bitmapData.Stride * bitmapData.Height];
            Marshal.Copy(bitmapData.Scan0, data, 0, bitmapData.Stride * bitmapData.Height);
            bitmap.UnlockBits(bitmapData);
            return new ByteImage(data, bitmap.Width, bitmap.Height, bitmap.PixelFormat, bitmapData.Stride, bitmap.RawFormat, bitmap.HorizontalResolution, bitmap.VerticalResolution);
        }

        public static void Save(ByteImage image, string path)
        {
            Bitmap bitmap = new Bitmap(image.Width, image.Height, image.Stride, image.PixelFormat, Marshal.UnsafeAddrOfPinnedArrayElement(image.GetData(), 0));
            bitmap.SetResolution(image.DpiX, image.DpiY);
            string dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            bitmap.Save(path, image.ImageFormat);
        }
    }
}