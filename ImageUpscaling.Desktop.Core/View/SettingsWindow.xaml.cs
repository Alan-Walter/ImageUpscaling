using ImageUpscaling.Desktop.Core.ViewModel;

using System;
using System.Windows;

namespace ImageUpscaling.Desktop.Core.View
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private readonly SettingsViewModel settingsViewModel;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="saveAction">Действие при сохранении</param>
        public SettingsWindow(Action saveAction)
        {
            InitializeComponent();
            settingsViewModel = new SettingsViewModel(this, saveAction);
            DataContext = settingsViewModel;
        }
    }
}