using ImageScaling;
using ImageScaling.Helpers;
using ImageScaling.KerasNet;
using ImageScaling.NeuralNetworks;

using ImageUpscaling.Desktop.Core.Models;
using ImageUpscaling.Desktop.Core.Services;
using ImageUpscaling.Desktop.Core.View;

using Microsoft.Win32;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace ImageUpscaling.Desktop.Core.ViewModel
{
    /// <summary>
    /// Класс главной ViewModel
    /// </summary>
    internal class MainViewModel : BaseViewModel
    {
        #region Приватные поля

        private ObservableCollection<ImageFileViewModel> imageFileViewModels;
        private ObservableCollection<ScaleImageViewModel> scaleImageViewModels;
        private ObservableCollection<SRGANViewModel> srganViewModels;
        private ScaleImageViewModel selectedScaleImageViewModel;
        private SRGANViewModel selectedSRGANViewModel;
        private double scale = 1;
        private string errorMessage;
        private bool canStarted = true;
        private string userMessage = "Ожидание действий";
        private readonly AppSettings settings;
        private readonly ScalingFactory<IScaleImage> scalingFactory;

        #endregion Приватные поля

        #region Публичные свойства

        /// <summary>
        /// Коллекция ViewModel файлов изображений
        /// </summary>
        public ObservableCollection<ImageFileViewModel> ImageFileViewModels
        {
            get => imageFileViewModels;
            set
            {
                if (imageFileViewModels == value) return;
                imageFileViewModels = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Коллекция ViewModel алгоритмов масштабирования
        /// </summary>
        public ObservableCollection<ScaleImageViewModel> ScaleImageViewModels
        {
            get => scaleImageViewModels;
            set
            {
                if (scaleImageViewModels == value) return;
                scaleImageViewModels = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Коллекция view model моделей SRGAN
        /// </summary>
        public ObservableCollection<SRGANViewModel> SRGANViewModels
        {
            get => srganViewModels;
            set
            {
                if (value == srganViewModels) return;
                srganViewModels = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Выбранный алгоритм масштабирования изображения
        /// </summary>
        public ScaleImageViewModel SelectedScaleImageViewModel
        {
            get => selectedScaleImageViewModel;
            set
            {
                if (selectedScaleImageViewModel == value) return;
                selectedScaleImageViewModel = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsScalable");
                RaisePropertyChanged("IsSRGAN");
            }
        }

        /// <summary>
        /// Выбранная модель SRGAN
        /// </summary>
        public SRGANViewModel SelectedSRGANViewModel
        {
            get => selectedSRGANViewModel;
            set
            {
                if (value == selectedSRGANViewModel) return;
                selectedSRGANViewModel = value;
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
            }
        }

        /// <summary>
        /// Масштабируемость алгоритма
        /// </summary>
        public Visibility IsScalable
        {
            get
            {
                return SelectedScaleImageViewModel?.IsScalable == true ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        /// <summary>
        /// SRGAN алгоритм масштабирования
        /// </summary>
        public Visibility IsSRGAN
        {
            get
            {
                return SelectedScaleImageViewModel.ScaleImageType == typeof(SRGAN) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Текст ошибки
        /// </summary>
        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                if (errorMessage == value) return;
                errorMessage = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Можно ли запустить
        /// </summary>
        public bool CanStarted
        {
            get => canStarted;
            set
            {
                if (canStarted == value) return;
                canStarted = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Сообщение пользователю
        /// </summary>
        public string UserMessage
        {
            get => userMessage;
            set
            {
                if (userMessage == value) return;
                userMessage = value;
                RaisePropertyChanged();
            }
        }

        #endregion Публичные свойства

        #region Команды

        /// <summary>
        /// Удалить файл изображения
        /// </summary>
        public ICommand DeleteImageFileCommand { get; }

        /// <summary>
        /// Команда открытия файла
        /// </summary>
        public ICommand OpenFileCommand { get; }

        /// <summary>
        /// Команда масштабирования изображения
        /// </summary>
        public ICommand ScaleCommand { get; }

        /// <summary>
        /// Команда очистки списка
        /// </summary>
        public ICommand ClearCommand { get; }

        /// <summary>
        /// Команда настроек
        /// </summary>
        public ICommand SettingsCommand { get; }

        #endregion Команды

        public MainViewModel()
        {
            scalingFactory = new ScalingFactory<IScaleImage>();
            settings = ConfigService.GetAppSettings();

            DeleteImageFileCommand = new Command((i) => DeleteImageFile(i));
            OpenFileCommand = new Command(OpenFile);
            ScaleCommand = new Command(ScaleImage, () => CanStarted);
            ClearCommand = new Command(i =>
            {
                ImageFileViewModels.Clear();
            });
            SettingsCommand = new Command(() =>
            {
                var window = new SettingsWindow(() =>
                {
                    ReloadSRGANModels();
                });
                window.ShowDialog();
            });

            ImageFileViewModels = new ObservableCollection<ImageFileViewModel>();
            ScaleImageViewModels = new ObservableCollection<ScaleImageViewModel>(scalingFactory.GetImplTypes().Select(i =>
            {
                return new ScaleImageViewModel(i);
            }));
            ReloadSRGANModels();

            SelectedScaleImageViewModel = ScaleImageViewModels.FirstOrDefault();
            SelectedSRGANViewModel = SRGANViewModels.FirstOrDefault();
        }

        /// <summary>
        /// Удалить файл изображения из view model
        /// </summary>
        /// <param name="imageFile">Объект View Model файла изображения</param>
        private void DeleteImageFile(object imageFile)
        {
            if (imageFile is ImageFileViewModel imageFileViewModel)
            {
                ImageFileViewModels.Remove(imageFileViewModel);
            }
        }

        /// <summary>
        /// Открытие файла изображения
        /// </summary>
        private void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files|*.bmp;*.jpg;*.gif;*.png;*.tif|All files|*.*",
                FilterIndex = 1,
                Multiselect = true
            };
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (var filePath in openFileDialog.FileNames)
                {
                    var (width, height) = ImageHelper.GetResolution(filePath);
                    ImageFileViewModels.Add(new ImageFileViewModel(this)
                    {
                        FileName = Path.GetFileName(filePath),
                        FullPath = filePath,
                        Size = $"{width} x {height}"
                    });
                }
            }
        }

        /// <summary>
        /// Масштабировать изображение
        /// </summary>
        private void ScaleImage()
        {
            if (SelectedScaleImageViewModel == null)
            {
                ErrorMessage = "Алгоритм масштабирования не выбран";
                return;
            }

            if (!ImageFileViewModels.Any())
            {
                ErrorMessage = "Отсутствуют изображения для масштабирования";
                return;
            }

            var scale = scalingFactory.GetScaleObject(SelectedScaleImageViewModel.ScaleImageType);
            if (scale is IScalable scalable)
            {
                scalable.Scale = Scale;
            }

            if (scale is SRGAN srgan)
            {
                if (SelectedSRGANViewModel == null)
                {
                    ErrorMessage = "Не выбрана модель нейронной сети";
                    return;
                }

                srgan.ModelPath = SelectedSRGANViewModel.ModelPath;

                if (settings.UseCPU)
                {
                    KerasHelper.HideCuda();
                }
            }

            if (!Directory.Exists(settings.OutputPath))
            {
                Directory.CreateDirectory(settings.OutputPath);
            }

            CanStarted = false;
            UserMessage = "Выполнение масштабирования";

            Dispatcher.CurrentDispatcher.BeginInvoke(() =>
            {
                var timer = new Stopwatch();
                timer.Start();
                try
                {
                    foreach (var image in ImageFileViewModels)
                    {
                        scale.ScaleImage(image.FullPath, settings.OutputPath);
                    }

                    timer.Stop();

                    UserMessage = $"Масштабирование завершено\nВремя: {timer.Elapsed}";

                    if (settings.OpenAfterScale)
                    {
                        Process.Start("explorer.exe", settings.OutputPath);
                    }
                }
                catch (Exception e)
                {
                    UserMessage = "Произошла ошибка";
                    ErrorMessage = "Текст ошибки: " + e.Message;
                }
                finally
                {
                    CanStarted = true;
                }
            }, DispatcherPriority.Background);
        }

        /// <summary>
        /// Получить модели SRGAN
        /// </summary>
        /// <returns>Модели SRGAN</returns>
        private IEnumerable<SRGANViewModel> GetSRGANModels()
        {
            var viewModels = new List<SRGANViewModel>();
            foreach (var dir in settings.SRGANModelDirectories)
            {
                if (Directory.Exists(dir))
                {
                    var files = Directory.GetFiles(dir, ConfigService.FileExtPattern);
                    viewModels.AddRange(files.Select(i => new SRGANViewModel(i)));
                }
            }
            return viewModels;
        }

        /// <summary>
        /// Перезагрузить модели SRGAN
        /// </summary>
        private void ReloadSRGANModels()
        {
            SRGANViewModels = new ObservableCollection<SRGANViewModel>(GetSRGANModels());
        }
    }
}