using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ImageUpscaling.Model
{
    /// <summary>
    /// Масштабируемое изображение
    /// </summary>
    class ScalableImage
    {
        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Изображение
        /// </summary>
        public BitmapSource Image { get; set; }
    }
}
