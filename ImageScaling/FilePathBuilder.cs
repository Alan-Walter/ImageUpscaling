using System.IO;

namespace ImageScaling
{
    /// <summary>
    /// Билдер пути файла
    /// </summary>
    internal class FilePathBuilder
    {
        private string fileName;
        private string extension;
        private string path;
        private string algorithm;
        private double scale = 1;

        /// <summary>
        /// Конструктор билдера пути
        /// </summary>
        public FilePathBuilder()
        {
        }

        /// <summary>
        /// Конструктор билдера пути
        /// </summary>
        /// <param name="fileName">Имя файла, которое будет браться за основу</param>
        public FilePathBuilder(string fileName)
        {
            this.fileName = Path.GetFileNameWithoutExtension(fileName);
            extension = Path.GetExtension(fileName);
        }

        /// <summary>
        /// Установить путь
        /// </summary>
        /// <param name="path">Путь</param>
        /// <returns>Билдер</returns>
        public FilePathBuilder SetPath(string path)
        {
            this.path = path;
            return this;
        }

        /// <summary>
        /// Установить имя алгоритма
        /// </summary>
        /// <param name="name">Имя алгоритма</param>
        /// <returns>Билдер</returns>
        public FilePathBuilder SetAlgorithmName(string name)
        {
            algorithm = name;
            return this;
        }

        /// <summary>
        /// Установить масштаб
        /// </summary>
        /// <param name="scale">Масштаб</param>
        /// <returns>Билдер</returns>
        public FilePathBuilder SetScale(double scale)
        {
            this.scale = scale;
            return this;
        }

        /// <summary>
        /// Установить имя файла
        /// </summary>
        /// <param name="fileName">Новое имя файла (без расширения)</param>
        /// <returns>Билдер</returns>
        public FilePathBuilder SetFileName(string fileName)
        {
            this.fileName = fileName;
            return this;
        }

        /// <summary>
        /// Установить расширение
        /// </summary>
        /// <param name="ext">Расширение файла</param>
        /// <returns>Билдер</returns>
        public FilePathBuilder SetExtension(string ext)
        {
            if (!ext.StartsWith('.'))
            {
                this.extension = "." + ext;
            }
            else
            {
                this.extension = ext;
            }
            return this;
        }

        /// <summary>
        /// Построить путь для файла
        /// </summary>
        /// <returns>Полный путь до файла</returns>
        public string Build()
        {
            return Path.Combine(path, string.Join(' ', fileName, $"x{scale}", algorithm) + extension);
        }
    }
}