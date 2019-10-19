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
    class ImageFileManager
    {
        static ImageFileManager instance;

        public static ImageFileManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new ImageFileManager();
                return instance;
            }
        }

        public BitmapSource Load(string path)
        {
            return new BitmapImage(new Uri(path));
        }

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
