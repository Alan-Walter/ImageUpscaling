using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ImageUpscaling.Helpers;

namespace ImageUpscaling.Managers
{
    /// <summary>
    /// Файловый менеджер изображений
    /// </summary>
    public class ImageFileManager
    {
        static ImageFileManager instance;

        /// <summary>
        /// Синглтон
        /// </summary>
        public static ImageFileManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new ImageFileManager();
                return instance;
            }
        }

        /// <summary>
        /// Загрузка изображения из пути
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public BitmapSource Load(string path)
        {
            BitmapImage image = new BitmapImage();
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                image.BeginInit();
                image.StreamSource = stream;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
            }
            return image;
        }

        /// <summary>
        /// Сохранение изображения в файл по указанному пути
        /// </summary>
        /// <param name="image"></param>
        /// <param name="filePath"></param>
        public void Save(BitmapSource image, string filePath)
        {
            var extension = string.Concat(Path.GetExtension(filePath).Skip(1));
            if (extension.ToLower() == "jpg")
                extension = "jpeg";
            var types = ReflectionHelper.GetImplimentationTypes(typeof(BitmapEncoder));
            var implType = types
                .Where(i => i.Name.StartsWith(extension, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();
            if (implType == null)
                throw new ArgumentException("Не удалось найти encoder для указанного типа файла");
            string dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                BitmapEncoder encoder = (BitmapEncoder)Activator.CreateInstance(implType);
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(fileStream);
            }
        }
    }
}
