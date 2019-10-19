using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using ImageUpscaling.Model;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using ImageUpscaling.Scaling;
using ImageUpscaling.Scaling.Interpolation;

namespace ImageUpscaling.ViewModel
{
    class MainViewModel : BaseViewModel
    {
        ScalableImageViewModel selectedScalableImage;

        ObservableCollection<ScalableImageViewModel> scalableImages;
        ObservableCollection<ScalingViewModel> scalingViewModels;
        private double scale = 1;
        private ScalingViewModel scalingViewModel;

        public Command OpenFileCommand { get; }
        public Command ScaleCommand { get; }

        public ScalableImageViewModel SelectedScalableImage
        {
            get => selectedScalableImage;
            set
            {
                selectedScalableImage = value;
                RaisePropertyChanged();
                RaisePropertyChanged("Size");
                RaisePropertyChanged("ScaleSize");
            }
        }

        public ScalingViewModel SelectedScalingAlgorithm 
        { 
            get => scalingViewModel;
            set
            {
                if (scalingViewModel == value) return;
                scalingViewModel = value;
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

        public ObservableCollection<ScalingViewModel> ScalingAlgorithms
        {
            get => scalingViewModels;
            set
            {
                scalingViewModels = value;
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
                RaisePropertyChanged("ScaleSize");
            }
        }

        public string Size
        {
            get
            {
                if (selectedScalableImage == null) return null;
                return $"{selectedScalableImage.Width}x{selectedScalableImage.Height}";
            }
        }

        public string ScaleSize
        {
            get
            {
                if (selectedScalableImage == null) return null;
                return $"{(int)Math.Round(selectedScalableImage.Width * scale)}x{(int)Math.Round(selectedScalableImage.Height * scale)}";
            }
        }

        public MainViewModel()
        {
            ScalableImages = new ObservableCollection<ScalableImageViewModel>();

            InterpolationFactory interpolationFactory = new InterpolationFactory();
            ScalingAlgorithms = new ObservableCollection<ScalingViewModel>(interpolationFactory.GetScaleObjects().Select(i => new ScalingViewModel(i)));
            SelectedScalingAlgorithm = ScalingAlgorithms.FirstOrDefault();

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
            if (selectedScalableImage == null) return;  //  добавить окно


            //IScaling nearestNeighbor = new BilinearInterpolation();
            //ScalableImages.Add(new ScalableImageViewModel(new ScalableImage()
            //{
            //    Name = $"Scale x{scale:f2} - " + selectedScalableImage.FullName,
            //    Image = nearestNeighbor.ScaleImage(selectedScalableImage.Image, scale)
            //}));
            //SelectedScalableImage = ScalableImages.Last();
        }
    }
}
