namespace ImageScaling.NeuralNetworks
{
    /// <summary>
    /// Интерфейс для алгоритмов нейронных сетей
    /// </summary>
    public interface INeuralNetworkImageScaling : IScaleImage
    {
        /// <summary>
        /// Логгер масштабирования
        /// </summary>
        IScaleLogger ScaleLogger { get; set; }

        /// <summary>
        /// Файл обученной модели
        /// </summary>
        string ModelPath { get; set; }

        /// <summary>
        /// Обучить нейронную сеть
        /// </summary>
        /// <param name="trainParams">Параметры обучения</param>
        void Train(TrainParams trainParams);
    }
}