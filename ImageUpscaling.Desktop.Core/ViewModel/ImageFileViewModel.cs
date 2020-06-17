namespace ImageUpscaling.Desktop.Core.ViewModel
{
    /// <summary>
    /// ViewModel для файла изображения
    /// </summary>
    internal class ImageFileViewModel : BaseViewModel
    {
        /// <summary>
        /// Главный ViewModel
        /// </summary>
        public MainViewModel MainViewModel { get; }

        /// <summary>
        /// Имя файла
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Полный путь до файла
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// Размер изображения
        /// </summary>
        public string Size { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="mainViewModel">Главный ViewModel</param>
        public ImageFileViewModel(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
        }
    }
}