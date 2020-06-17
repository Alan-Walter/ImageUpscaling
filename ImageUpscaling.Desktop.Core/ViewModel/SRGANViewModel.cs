using System.IO;

namespace ImageUpscaling.Desktop.Core.ViewModel
{
    /// <summary>
    /// View Model модели SRGAN
    /// </summary>
    internal class SRGANViewModel : BaseViewModel
    {
        private string modelPath;

        /// <summary>
        /// Имя файла модели
        /// </summary>
        public string ModelName { get; private set; }

        /// <summary>
        /// Путь файла
        /// </summary>
        public string ModelPath
        {
            get => modelPath;
            set
            {
                if (value == null || value == modelPath) return;
                modelPath = value;
                ModelName = Path.GetFileNameWithoutExtension(modelPath);
                RaisePropertyChanged();
                RaisePropertyChanged("ModelName");
            }
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return ModelName;
        }

        public SRGANViewModel(string modelPath)
        {
            ModelPath = modelPath;
        }
    }
}