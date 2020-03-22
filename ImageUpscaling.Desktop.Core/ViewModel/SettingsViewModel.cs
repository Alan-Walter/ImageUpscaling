using ImageUpscaling.Desktop.Core.Models;
using ImageUpscaling.Desktop.Core.Services;

using Ookii.Dialogs.Wpf;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace ImageUpscaling.Desktop.Core.ViewModel
{
    /// <summary>
    /// View Model настроек
    /// </summary>
    internal class SettingsViewModel : BaseViewModel
    {
        #region Поля

        private bool useCPU;
        private bool openAfterScale;
        private ObservableCollection<SRGANDirectoryViewModel> sRGANModelDirectories;
        private SRGANDirectoryViewModel selectedSRGANDirectoryViewModel;
        private string outputPath;
        private readonly AppSettings settings;
        private readonly Window currentWindow;
        private readonly Action saveAction;

        #endregion Поля

        #region Свойства

        /// <summary>
        /// Использовать CPU
        /// </summary>
        public bool UseCPU
        {
            get => useCPU;
            set
            {
                if (value == useCPU) return;
                useCPU = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Открыть папку после масштабирования
        /// </summary>
        public bool OpenAfterScale
        {
            get => openAfterScale;
            set
            {
                if (openAfterScale == value) return;
                openAfterScale = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Путь для сохранения
        /// </summary>
        public string OutputPath
        {
            get => outputPath;
            set
            {
                if (outputPath == value) return;
                outputPath = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Модели папок
        /// </summary>
        public ObservableCollection<SRGANDirectoryViewModel> SRGANModelDirectories
        {
            get => sRGANModelDirectories;
            set
            {
                if (sRGANModelDirectories == value) return;
                sRGANModelDirectories = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Выбранная папка
        /// </summary>
        public SRGANDirectoryViewModel SelectedSRGANDirectoryViewModel
        {
            get => selectedSRGANDirectoryViewModel;
            set
            {
                if (selectedSRGANDirectoryViewModel == value) return;
                selectedSRGANDirectoryViewModel = value;
                RaisePropertyChanged();
            }
        }

        #endregion Свойства

        #region Команды

        /// <summary>
        /// Команда сохранения
        /// </summary>
        public Command SaveCommand { get; }

        /// <summary>
        /// Команда отмены
        /// </summary>
        public Command CancelCommand { get; }

        /// <summary>
        /// Команда открыть путь
        /// </summary>
        public Command OpenPathCommand { get; }

        /// <summary>
        /// Команда добавить папку
        /// </summary>
        public Command AddDirectoryCommand { get; }

        /// <summary>
        /// Команда удалить папку
        /// </summary>
        public Command DeleteDirectoryCommand { get; }

        #endregion Команды

        public SettingsViewModel(Window current, Action saveAction)
        {
            settings = ConfigService.GetAppSettings();
            LoadSettings();

            currentWindow = current;
            this.saveAction = saveAction;

            SaveCommand = new Command(SaveSettings);
            CancelCommand = new Command(() =>
            {
                currentWindow.Close();
            });
            OpenPathCommand = new Command(OpenSavePath);
            AddDirectoryCommand = new Command(AddModelDirectory);
            DeleteDirectoryCommand = new Command(DeleteModelDirectory);
        }

        /// <summary>
        /// Сохранить настройки
        /// </summary>
        private void SaveSettings()
        {
            settings.OpenAfterScale = OpenAfterScale;
            settings.UseCPU = UseCPU;
            settings.OutputPath = OutputPath;
            settings.SRGANModelDirectories = SRGANModelDirectories.Select(i => i.Directory).ToList();
            ConfigService.SaveSettings();
            saveAction();
            currentWindow.Close();
        }

        /// <summary>
        /// Указать путь для сохранения
        /// </summary>
        private void OpenSavePath()
        {
            var path = GetDirectoryFolder();
            if (path != null)
            {
                OutputPath = path;
            }
        }

        /// <summary>
        /// Добавить директорию с моделями
        /// </summary>
        private void AddModelDirectory()
        {
            var path = GetDirectoryFolder();
            if (path != null)
            {
                SRGANModelDirectories.Add(new SRGANDirectoryViewModel(path));
            }
        }

        /// <summary>
        /// Удалить директорию с моделями
        /// </summary>
        private void DeleteModelDirectory()
        {
            SRGANModelDirectories.Remove(SelectedSRGANDirectoryViewModel);
            SelectedSRGANDirectoryViewModel = null;
        }

        /// <summary>
        /// Загрузить настройки
        /// </summary>
        private void LoadSettings()
        {
            OpenAfterScale = settings.OpenAfterScale;
            UseCPU = settings.UseCPU;
            OutputPath = settings.OutputPath;
            SRGANModelDirectories = new ObservableCollection<SRGANDirectoryViewModel>(settings.SRGANModelDirectories.Select(i => new SRGANDirectoryViewModel(i)));
        }

        /// <summary>
        /// Получить папку
        /// </summary>
        /// <returns>Строка с указанной папкой</returns>
        private string GetDirectoryFolder()
        {
            var vistaFolderBrowserDialog = new VistaFolderBrowserDialog();
            if (vistaFolderBrowserDialog.ShowDialog() == true)
            {
                return vistaFolderBrowserDialog.SelectedPath;
            }

            return null;
        }
    }
}