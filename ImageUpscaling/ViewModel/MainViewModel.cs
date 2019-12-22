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
using ImageUpscaling.Managers;
using System.Diagnostics;

namespace ImageUpscaling.ViewModel
{
    class MainViewModel : BaseViewModel
    {
        ScalableImageViewModel selectedScalableImage;

        ObservableCollection<ScalableImageViewModel> scalableImages;
        ObservableCollection<ScalingViewModel> scalingViewModels;
        private double scale = 1;
        private ScalingViewModel scalingViewModel;
        private TimeSpan? workTime;

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

        public string Time
        {
            get
            {
                if (!workTime.HasValue)
                {
                    return string.Empty;
                }
                else
                {
                    return $"Время: {workTime.Value}";
                }
            }
        }

        private TimeSpan? WorkTime
        {
            get
            {
                return workTime;
            }
            set
            {
                if (workTime == value) return;
                workTime = value;
                RaisePropertyChanged("Time");
            }
        }

        public MainViewModel()
        {
            ScalableImages = new ObservableCollection<ScalableImageViewModel>();

            ImageScalingFactory interpolationFactory = new ImageScalingFactory();
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
                    Image = ImageFileManager.Instance.Load(filePath),
                }));
                SelectedScalableImage = ScalableImages.Last();
            }
        }

        private void ScaleImage()
        {
            if (selectedScalableImage == null) return;
            if (scalingViewModel == null) return;
            WorkTime = null;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = scalingViewModel.Scaling.ScaleImage(selectedScalableImage.Image, scale);
            stopwatch.Stop();
            WorkTime = stopwatch.Elapsed;
            string path = Path.GetFullPath($"./output/[{scalingViewModel.ToString()} x{scale}] {DateTime.Now.ToString().Replace(':', '.')} " + selectedScalableImage.Name);
            ImageFileManager.Instance.Save(result, path);
            System.Diagnostics.Process.Start("explorer.exe", string.Format("/select,\"{0}\"", path));
        }
    }
}
