using System;
using System.IO;

namespace ImageScaling
{
    /// <summary>
    /// Хэлпер исключений
    /// </summary>
    internal static class ExceptionHelper
    {
        /// <summary>
        /// Создать исключение, если аргумент пустой или равен null
        /// </summary>
        /// <param name="arg">Аргумент</param>
        /// <param name="argName">Имя аргумента</param>
        public static void ThrowIfNullOrEmpty(string arg, string argName)
        {
            if (string.IsNullOrEmpty(arg))
            {
                throw new ArgumentException(argName);
            }
        }

        /// <summary>
        /// Создать исключение, если файл не существует
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        public static void ThrowIfFileNotExists(string fileName)
        {
            ThrowIfNullOrEmpty(fileName, "fileName");
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException(fileName);
            }
        }

        /// <summary>
        /// Создать исключение, если папка не существует
        /// </summary>
        /// <param name="directory">Директория</param>
        public static void ThrowIfDirectoryNotExists(string directory)
        {
            ThrowIfNullOrEmpty(directory, "directory");
            if (!Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException(directory);
            }
        }

        /// <summary>
        /// Создать исключение, если аргумент равен null
        /// </summary>
        /// <param name="arg">Объект</param>
        /// <param name="argName">Имя аргумента</param>
        public static void ThrowIfNull(object arg, string argName)
        {
            if (arg == null)
            {
                throw new ArgumentNullException(argName);
            }
        }
    }
}