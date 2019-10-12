using System.IO;
using System.Windows.Media.Imaging;

using ImageUpscaling.Model;

namespace ImageUpscaling.ViewModel
{
    internal class ScalableImageViewModel : BaseViewModel
    {
        private readonly ScalableImage scalableImage;
        private bool isSaved;

        public string Name
        {
            get
            {
                if (scalableImage.Name.Length < 30)
                    return scalableImage.Name;
                return Path.GetFileName(scalableImage.Name).Substring(0, 30) + ".. " + Path.GetExtension(scalableImage.Name) + (isSaved ? "" : " *");
            }
        }

        public string FullName => scalableImage.Name;

        public BitmapSource Image => scalableImage.Image;

        public bool IsSaved
        {
            get => isSaved;
            set
            {
                if (isSaved == value) return;
                isSaved = true;
                RaisePropertyChanged();
                RaisePropertyChanged("Name");
            }
        }

        public ScalableImageViewModel(ScalableImage scalableImage, bool isSaved = false)
        {
            this.scalableImage = scalableImage;
            IsSaved = isSaved;
        }
    }
}