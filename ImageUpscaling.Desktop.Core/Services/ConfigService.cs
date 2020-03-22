using ImageUpscaling.Desktop.Core.Models;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;

namespace ImageUpscaling.Desktop.Core.Services
{
    /// <summary>
    /// Сервис настроек приложения
    /// </summary>
    public static class ConfigService
    {
        /// <summary>
        /// Имя приложения
        /// </summary>
        private const string AppName = "Image Upscaling";

        /// <summary>
        /// Имя файла настроек приложения
        /// </summary>
        private const string ConfigFileName = "config.json";

        /// <summary>
        /// Имя локальной папки для выхода
        /// </summary>
        private const string LocalOutputDirectoryName = "Output";

        /// <summary>
        /// Название папки с моделями SRGAN
        /// </summary>
        private const string LocalSRGANModels = "srganmodels";

        /// <summary>
        /// Паттерн расширения имени файла
        /// </summary>
        public const string FileExtPattern = "*.h5";

        /// <summary>
        /// Объект настроек приложения
        /// </summary>
        private static AppSettings appSettings;

        /// <summary>
        /// Получить настройки приложения
        /// </summary>
        /// <returns>Объект с настройками</returns>
        public static AppSettings GetAppSettings()
        {
            if (appSettings == null)
            {
                appSettings = LoadSettings();
            }

            return appSettings;
        }

        /// <summary>
        /// Сохранить настройки
        /// </summary>
        public static void SaveSettings()
        {
            var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppName);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            var file = Path.Combine(directory, ConfigFileName);
            var data = JsonConvert.SerializeObject(appSettings);
            File.WriteAllText(file, data);
        }

        /// <summary>
        /// Загрузить настройки
        /// </summary>
        /// <returns>Объект настроек</returns>
        private static AppSettings LoadSettings()
        {
            var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppName);
            var file = Path.Combine(directory, ConfigFileName);
            if (Directory.Exists(directory) && File.Exists(file))
            {
                var settings = JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText(file));
                return settings;
            }
            else
            {
                return new AppSettings()
                {
                    OpenAfterScale = false,
                    OutputPath = Path.Combine(Directory.GetCurrentDirectory(), LocalOutputDirectoryName),
                    SRGANModelDirectories = new List<string>
                    {
                        Path.Combine(Directory.GetCurrentDirectory(), LocalSRGANModels)
                    }
                };
            }
        }
    }
}