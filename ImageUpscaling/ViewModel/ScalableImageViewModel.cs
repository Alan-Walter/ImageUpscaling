using System.IO;
using System.Windows.Media.Imaging;

using ImageUpscaling.Model;

namespace ImageUpscaling.ViewModel
{
    internal class ScalableImageViewModel : BaseViewModel
    {
        private readonly ScalableImage scalableImage;

        public string Name
        {
            get
            {
                if (scalableImage.Name.Length < 30)
                    return scalableImage.Name;
                return Path.GetFileName(scalableImage.Name).Substring(0, 30) + ".. " + Path.GetExtension(scalableImage.Name);
            }
        }

        public string FullName => scalableImage.Name;

        public BitmapSource Image => scalableImage.Image;

        public int Width => scalableImage.Image.PixelWidth;

        public int Height => scalableImage.Image.PixelHeight;

        public ScalableImageViewModel(ScalableImage scalableImage)
        {
            this.scalableImage = scalableImage;
        }
    }
}