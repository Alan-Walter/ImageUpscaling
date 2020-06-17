namespace ImageScaling.NeuralNetworks
{
    /// <summary>
    /// Параметры обучения
    /// </summary>
    public class TrainParams
    {
        /// <summary>
        /// Путь до файлов, на которых будет происходить обучение
        /// </summary>
        public string TrainFilesPath { get; set; }

        /// <summary>
        /// Количество эпох
        /// </summary>
        public int Epochs { get; set; } = 3000;

        /// <summary>
        /// Размер группы файлов
        /// </summary>
        public int BatchSize { get; set; } = 1;

        /// <summary>
        /// Директория для сохранения весов генератора, дискриминатора и модели
        /// </summary>
        public string SaveDirectory { get; set; }

        /// <summary>
        /// Промежуточный шаг, на котором будут генерироваться тестовые результаты и сохраняться бэкап
        /// </summary>
        public int IntermediateStep { get; set; } = 100;

        /// <summary>
        /// Путь для сохранения временных файлов
        /// </summary>
        public string TempPath { get; set; }

        /// <summary>
        /// Обрезать изображения до нужного размера
        /// </summary>
        public bool CropImage { get; set; }

        /// <summary>
        /// Случайное обрезание изображений
        /// </summary>
        public bool RandomCropImage { get; set; }
    }
}