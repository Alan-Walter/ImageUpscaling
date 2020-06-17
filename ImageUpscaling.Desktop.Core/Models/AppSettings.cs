using System.Collections.Generic;

namespace ImageUpscaling.Desktop.Core.Models
{
    /// <summary>
    /// Модель настроек приложения
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// Список папок с моделями SRGAN
        /// </summary>
        public IEnumerable<string> SRGANModelDirectories { get; set; }

        /// <summary>
        /// Путь для сохранения файлов
        /// </summary>
        public string OutputPath { get; set; }

        /// <summary>
        /// Открыть после масштабирования
        /// </summary>
        public bool OpenAfterScale { get; set; } = false;

        /// <summary>
        /// Использовать CPU вместо GPU
        /// </summary>
        public bool UseCPU { get; set; } = false;
    }
}