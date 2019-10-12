using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using ImageUpscaling.Model;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using ImageUpscaling.Scaling;

namespace ImageUpscaling.ViewModel
{
    class MainViewModel : BaseViewModel
    {
        ScalableImageViewModel selectedScalableImage;

        ObservableCollection<ScalableImageViewModel> scalableImages;
        private double scale = 1;

        public Command OpenFileCommand { get; }
        public Command ScaleCommand { get; }

        public ScalableImageViewModel SelectedScalableImage
        {
            get => selectedScalableImage;
            set
            {
                selectedScalableImage = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<ScalableImageViewModel> ScalableImages
        {
            get => scalableImages;
            set
            {
                scalableImages = value;
                RaisePropertyChanged();
            }
        }

        public double Scale
        {
            get => scale;
            set
            {
                if (scale == value) return;
                scale = value;
                RaisePropertyChanged();
            }
        }

        public MainViewModel()
        {
            ScalableImages = new ObservableCollection<ScalableImageViewModel>();

            OpenFileCommand = new Command(OpenFile);
            ScaleCommand = new Command(ScaleImage);
        }

        private void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files|*.bmp;*.jpg;*.gif;*.png;*.tif|All files|*.*",
                FilterIndex = 1
            };
            if (openFileDialog.ShowDialog() == true)
            {
                var filePath = openFileDialog.FileName;
                ScalableImages.Add(new ScalableImageViewModel(new ScalableImage()
                {
                    Name = Path.GetFileName(filePath),
                    Image = new BitmapImage(new Uri(filePath)),
                }, true));
                SelectedScalableImage = ScalableImages.Last();
            }
        }

        private void ScaleImage()
        {
            NearestNeighbor nearestNeighbor = new NearestNeighbor();
            ScalableImages.Add(new ScalableImageViewModel(new ScalableImage()
            {
                Name = $"Scale x{scale:f2} - " + selectedScalableImage.FullName,
                Image = nearestNeighbor.ScaleImage(selectedScalableImage.Image, scale)
            }));
            SelectedScalableImage = ScalableImages.Last();
        }
    }
}
