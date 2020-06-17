using Keras.Layers;
using Keras.Models;

using Python.Runtime;

using System.Collections.Generic;

namespace ImageScaling.KerasNet
{
    /// <summary>
    /// Хелпер Keras
    /// </summary>
    public static class KerasHelper
    {
        /// <summary>
        /// Вызвать __call__
        /// </summary>
        /// <param name="model">Модель</param>
        /// <param name="baseLayer">Базовый слой</param>
        /// <returns>Слой</returns>
        public static BaseLayer Call(this BaseModel model, BaseLayer baseLayer)
        {
            Dictionary<string, object> d = new Dictionary<string, object>
            {
                { "inputs", baseLayer.ToPython() }
            };
            return new BaseLayer(model.InvokeMethod("__call__", d));
        }

        /// <summary>
        /// Скрыть Cuda для запуска на CPU
        /// </summary>
        public static void HideCuda()
        {
            using (Py.GIL())
            {
                PythonEngine.RunSimpleString(@"import os
os.environ['CUDA_VISIBLE_DEVICES'] = '-1'");
            }
        }
    }
}