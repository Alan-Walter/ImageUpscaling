using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;

using ImageScaling;
using ImageScaling.IO;

using ImageUpscaling.Desktop.Core.Model;

using Microsoft.Win32;

namespace ImageUpscaling.Desktop.Core.ViewModel
{
    /// <summary>
    /// Класс главной ViewModel
    /// </summary>
    internal class MainViewModel : BaseViewModel
    {
        private ScalableImageViewModel selectedScalableImage;

        private ObservableCollection<ScalableImageViewModel> scalableImages;
        private ObservableCollection<ScalingViewModel> scalingViewModels;
        private double scale = 1;
        private ScalingViewModel scalingViewModel;
        private TimeSpan? workTime;

        /// <summary>
        /// Команда открытия файла
        /// </summary>
        public Command OpenFileCommand { get; }

        /// <summary>
        /// Команда масштабирования изображения
        /// </summary>
        public Command ScaleCommand { get; }

        /// <summary>
        /// Изображение для масштабирования
        /// </summary>
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

        /// <summary>
        /// Алгоритм масштабирования
        /// </summary>
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

        /// <summary>
        /// Открытые изображения для масштабирования
        /// </summary>
        public ObservableCollection<ScalableImageViewModel> ScalableImages
        {
            get => scalableImages;
            set
            {
                scalableImages = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Алгоритмы масштабирования
        /// </summary>
        public ObservableCollection<ScalingViewModel> ScalingAlgorithms
        {
            get => scalingViewModels;
            set
            {
                scalingViewModels = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Масштаб
        /// </summary>
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

        /// <summary>
        /// Размер
        /// </summary>
        public string Size
        {
            get
            {
                if (selectedScalableImage == null) return null;
                return $"{selectedScalableImage.Width}x{selectedScalableImage.Height}";
            }
        }

        /// <summary>
        /// Размер с масштабом
        /// </summary>
        public string ScaleSize
        {
            get
            {
                if (selectedScalableImage == null) return null;
                return $"{(int)Math.Round(selectedScalableImage.Width * scale)}x{(int)Math.Round(selectedScalableImage.Height * scale)}";
            }
        }

        /// <summary>
        /// Время работы
        /// </summary>
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

        /// <summary>
        /// Время работы
        /// </summary>
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

        /// <summary>
        /// Конструктор
        /// </summary>
        public MainViewModel()
        {
            ScalableImages = new ObservableCollection<ScalableImageViewModel>();

            ImageScalingFactory interpolationFactory = new ImageScalingFactory();
            ScalingAlgorithms = new ObservableCollection<ScalingViewModel>(interpolationFactory.GetScaleObjects().Select(i => new ScalingViewModel(i)));
            SelectedScalingAlgorithm = ScalingAlgorithms.FirstOrDefault();

            OpenFileCommand = new Command(OpenFile);
            ScaleCommand = new Command(ScaleImage);
        }

        /// <summary>
        /// Открытие файла изображения
        /// </summary>
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
                    ByteImage = ByteImageFileManager.ReadFile(filePath),
                }));
                SelectedScalableImage = ScalableImages.Last();
            }
        }

        /// <summary>
        /// Масштабировать изображение
        /// </summary>
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
            ByteImageFileManager.Save(result, path);
            System.Diagnostics.Process.Start("explorer.exe", string.Format("/select,\"{0}\"", path));
        }
    }
}