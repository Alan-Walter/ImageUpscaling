using Keras;
using Keras.PreProcessing.Image;

using Numpy;

using System.Collections.Generic;

namespace ImageScaling.KerasNet
{
    /// <summary>
    /// Работа с изображениями в Keras
    /// </summary>
    public class KerasImage : Base
    {
        private static readonly dynamic caller = Instance.keras.preprocessing.image;

        /// <summary>
        /// Сохранить изображение
        /// </summary>
        /// <param name="path">Путь</param>
        /// <param name="x">Изображение</param>
        /// <param name="dataFormat">Формат данных</param>
        /// <param name="fileFormat">Формат файла</param>
        /// <param name="scale">Масштаб</param>
        public static void SaveImg(string path, NDarray x, string dataFormat = null, string fileFormat = null, bool scale = true)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                ["path"] = path,
                ["x"] = x,
                ["data_format"] = dataFormat,
                ["file_format"] = fileFormat,
                ["scale"] = scale
            };

            InvokeStaticMethod(caller, "save_img", parameters);
        }

        /// <summary>
        /// Загрузить изображение как Numpy Array
        /// </summary>
        /// <param name="path">Путь</param>
        /// <param name="colorMode">Формат цвета</param>
        /// <param name="dtype">Формат данных массива</param>
        /// <param name="targetSize">Размер массива изображения</param>
        /// <returns>Массив с изображением</returns>
        public static NDarray LoadImgAsArray(string path, string colorMode, string dtype, Shape targetSize)
        {
            var img1 = ImageUtil.LoadImg(path, color_mode: colorMode, target_size: targetSize, interpolation: "bicubic");
            var array = ImageUtil.ImageToArray(img1, dtype: dtype);
            return array;
        }
    }
}