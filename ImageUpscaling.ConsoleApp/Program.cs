using ImageScaling;
using ImageScaling.NeuralNetworks;

using Newtonsoft.Json;

using System;
using System.IO;

namespace ImageUpscaling.ConsoleApp
{
    internal class Program
    {
        /// <summary>
        /// Имя файла с параметрами
        /// </summary>
        private const string ConfigPath = "trainparams.json";

        /// <summary>
        /// Точка входа в приложение
        /// </summary>
        /// <param name="args">Параметры командной строки</param>
        private static void Main(string[] args)
        {
            var scalingFactory = new ScalingFactory<INeuralNetworkImageScaling>();
            var neuralNetwork = scalingFactory.GetScaleObject(typeof(SRGAN));

            neuralNetwork.ScaleLogger = new ConsoleScaleLogger();

            TrainParams trainParams;

            if (!File.Exists(ConfigPath))
            {
                trainParams = new TrainParams();
                File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(trainParams));
            }
            else
            {
                var data = File.ReadAllText(ConfigPath);
                trainParams = JsonConvert.DeserializeObject<TrainParams>(data);
            }

            neuralNetwork.Train(trainParams);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}