namespace ImageUpscaling.Desktop.Core.ViewModel
{
    /// <summary>
    /// Папка с моделями SRGAN
    /// </summary>
    internal class SRGANDirectoryViewModel : BaseViewModel
    {
        /// <summary>
        /// Директория
        /// </summary>
        public string Directory { get; set; }

        /// <summary>
        /// Модель папки SRGAN
        /// </summary>
        /// <param name="directory">Путь</param>
        public SRGANDirectoryViewModel(string directory)
        {
            Directory = directory;
        }
    }
}