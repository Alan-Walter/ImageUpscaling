using ImageScaling.Extensions;
using ImageScaling.Helpers;
using ImageScaling.KerasNet;

using Keras;
using Keras.Applications.VGG;
using Keras.Layers;
using Keras.Models;

using Numpy;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace ImageScaling.NeuralNetworks
{
    /// <summary>
    /// Алгоритм SRGAN
    /// </summary>
    public class SRGAN : INeuralNetworkImageScaling
    {
        /// <summary>
        /// Отступ
        /// </summary>
        private const string Padding = "same";

        /// <summary>
        /// Имя файла генератора
        /// </summary>
        private const string GeneratorFileName = "generator.h5";

        /// <summary>
        /// Имя файла дискриминатора
        /// </summary>
        private const string DiscriminatorFileName = "discriminator.h5";

        /// <summary>
        /// Имя файла модели
        /// </summary>
        private const string ModelFileName = "model.h5";

        /// <summary>
        /// Логгер
        /// </summary>
        private IScaleLogger scaleLogger;

        /// <summary>
        /// Действие с изображением
        /// </summary>
        private Func<Bitmap, int, int, Bitmap> bitmapActionFunc;

        /// <summary>
        /// Файл обученной модели
        /// </summary>
        public string ModelPath { get; set; }

        /// <summary>
        /// Логгер масштабирования
        /// </summary>
        public IScaleLogger ScaleLogger
        {
            get => scaleLogger;
            set
            {
                if (value == null || scaleLogger == value) return;
                scaleLogger = value;
            }
        }

        /// <summary>
        /// Конструктор SRGAN
        /// </summary>
        public SRGAN()
        {
            ScaleLogger = new FakeLogger();
        }

        /// <summary>
        /// Создать остаточный блок
        /// </summary>
        /// <param name="baseLayer">Базовый слой</param>
        /// <returns>Остаточный блок</returns>
        private BaseLayer CreateResidualBlock(BaseLayer baseLayer)
        {
            //  размер ядра остаточного блока
            var kernelSize = new Tuple<int, int>(3, 3);
            //  шаг свертки
            var strides = new Tuple<int, int>(1, 1);
            //  момент
            var momentum = 0.8f;
            //  создаём слой свёртки
            var residualBlock = new Conv2D(filters: 64, kernel_size: kernelSize, strides: strides, padding: Padding)
                .Set(baseLayer);
            //  создаем слой функции активации
            residualBlock = new Activation("relu")
                .Set(residualBlock);
            //  создаем слой пакетной нормализации
            residualBlock = new BatchNormalization(momentum: momentum)
                .Set(residualBlock);
            //  создаем второй слой свёртки
            residualBlock = new Conv2D(filters: 64, kernel_size: kernelSize, strides: strides, padding: Padding)
                .Set(residualBlock);
            //  создаем второй слой пакетной нормализации
            residualBlock = new BatchNormalization(momentum: momentum)
                .Set(residualBlock);
            //  создаем слой добавления
            residualBlock = new Add(residualBlock, baseLayer);
            return residualBlock;
        }

        /// <summary>
        /// Создать модель генератора
        /// </summary>
        /// <returns>Модель генератора</returns>
        private Model CreateGeneratorModel()
        {
            //  количество остаточных блоков
            var residualBlocks = 16;
            //  момент пакетной нормализации
            var momentum = 0.8f;
            //  размер входного изображения
            var inputShape = (64, 64, 3);
            //  шаги сверточной сети
            var strides = new Tuple<int, int>(1, 1);
            //  размер ядра
            var kernelSize = new Tuple<int, int>(3, 3);
            //  создаем слой входного изображения
            var inputLayer = new Input(shape: inputShape);
            //  создаем первый слой свёртки с функцией активации PReLU
            var conv1 = new Conv2D(filters: 64, kernel_size: new Tuple<int, int>(9, 9), strides: strides, padding: Padding, activation: "relu")
                .Set(inputLayer);
            //  создаем остаточные блоки
            BaseLayer residualBlock = conv1;
            for (int i = 0; i < residualBlocks; ++i)
            {
                residualBlock = CreateResidualBlock(residualBlock);
            }
            //  создаем второй слой свёртки
            var conv2 = new Conv2D(filters: 64, kernel_size: kernelSize, strides: strides, padding: Padding)
                .Set(residualBlock);
            //  создаем пакетную нормализацию
            conv2 = new BatchNormalization(momentum: momentum)
                .Set(conv2);
            //  создаем слой добавления, складывающий слои до остаточных блоков и после
            var addLayer = new Add(conv2, conv1);
            //  создаем первый слой увеличения
            var upSampling1 = new UpSampling2D(size: new Tuple<int, int>(2, 2))
                .Set(addLayer);
            upSampling1 = new Conv2D(filters: 256, kernel_size: kernelSize, strides: strides, padding: Padding)
                .Set(upSampling1);
            upSampling1 = new Activation("relu")
                .Set(upSampling1);
            //  создаем второй слой увеличения
            var upSampling2 = new UpSampling2D(size: new Tuple<int, int>(2, 2))
                .Set(upSampling1);
            upSampling2 = new Conv2D(filters: 256, kernel_size: kernelSize, strides: strides, padding: Padding)
                .Set(upSampling2);
            upSampling2 = new Activation("relu")
                .Set(upSampling2);
            //  создаем слой свёртки для построения изображения
            var conv3 = new Conv2D(filters: 3, kernel_size: new Tuple<int, int>(9, 9), strides: strides, padding: Padding)
                .Set(upSampling2);
            var output = new Activation("tanh")
                .Set(conv3);
            //  создаём модель генератора
            var model = new Model(inputs: new BaseLayer[] { inputLayer }, outputs: new BaseLayer[] { output });
            return model;
        }

        /// <summary>
        /// Создать модель дискриминатора
        /// </summary>
        /// <returns>Модель дискриминатора</returns>
        private Model CreateDiscriminatorModel()
        {
            //  альфа leakyrelu
            var leakyreluAlpha = 0.2f;
            //  момент пакетной нормализации
            var momentum = 0.8f;
            //  размеры входного изображения
            var inputShape = (256, 256, 3);
            //  размер ядра слоя свёртки
            var kernelSize = new Tuple<int, int>(3, 3);
            //  создаем входной слой
            var inputLayer = new Input(shape: inputShape);
            //  шаг размера 1
            var oneStride = new Tuple<int, int>(1, 1);
            //  шаг размера 2
            var twoStride = new Tuple<int, int>(2, 2);
            //  создаем первый слой свёртки
            var conv1 = new Conv2D(filters: 64, kernel_size: kernelSize, strides: oneStride, padding: Padding)
                .Set(inputLayer);
            conv1 = new LeakyReLU(alpha: leakyreluAlpha)
                .Set(conv1);
            //  создаём второй слой свёртки
            var conv2 = new Conv2D(filters: 64, kernel_size: kernelSize, strides: twoStride, padding: Padding)
                .Set(conv1);
            conv2 = new LeakyReLU(alpha: leakyreluAlpha)
                .Set(conv2);
            conv2 = new BatchNormalization(momentum: momentum)
                .Set(conv2);
            //  создаём третий слой свёртки
            var conv3 = new Conv2D(filters: 128, kernel_size: kernelSize, strides: oneStride, padding: Padding)
                .Set(conv2);
            conv3 = new LeakyReLU(alpha: leakyreluAlpha)
                .Set(conv3);
            conv3 = new BatchNormalization(momentum: momentum)
                .Set(conv3);
            //  создаём четвертый слой свёртки
            var conv4 = new Conv2D(filters: 128, kernel_size: kernelSize, strides: twoStride, padding: Padding)
                .Set(conv3);
            conv4 = new LeakyReLU(alpha: leakyreluAlpha)
                .Set(conv4);
            conv4 = new BatchNormalization(momentum: 0.8f)
                .Set(conv4);
            //  создаём пятый слой свёртки
            var conv5 = new Conv2D(256, kernel_size: kernelSize, strides: oneStride, padding: Padding)
                .Set(conv4);
            conv5 = new LeakyReLU(alpha: leakyreluAlpha)
                .Set(conv5);
            conv5 = new BatchNormalization(momentum: momentum)
                .Set(conv5);
            //  создаём шестой слой свёртки
            var conv6 = new Conv2D(filters: 256, kernel_size: kernelSize, strides: twoStride, padding: Padding)
                .Set(conv5);
            conv6 = new LeakyReLU(alpha: leakyreluAlpha)
                .Set(conv6);
            conv6 = new BatchNormalization(momentum: momentum)
                .Set(conv6);
            //  создаём седьмой слой свёртки
            var conv7 = new Conv2D(filters: 512, kernel_size: kernelSize, strides: oneStride, padding: Padding)
                .Set(conv6);
            conv7 = new LeakyReLU(alpha: leakyreluAlpha)
                .Set(conv7);
            conv7 = new BatchNormalization(momentum: momentum)
                .Set(conv7);
            //  создаём восьмой слой свёртки
            var conv8 = new Conv2D(filters: 512, kernel_size: kernelSize, strides: twoStride, padding: Padding)
                .Set(conv7);
            conv8 = new LeakyReLU(alpha: leakyreluAlpha)
                .Set(conv8);
            conv8 = new BatchNormalization(momentum: momentum)
                .Set(conv8);
            //  создаём девятый слой для определения принадлежности
            var conv9 = new Dense(units: 1024)
                .Set(conv8);
            conv9 = new LeakyReLU(alpha: 0.2f)
                .Set(conv9);
            //  создаём последний слой для вычисления принадлежности
            var output = new Dense(units: 1, activation: "sigmoid")
                .Set(conv9);
            //  создаем модель
            var model = new Model(inputs: new BaseLayer[] { inputLayer }, outputs: new BaseLayer[] { output });
            return model;
        }

        /// <summary>
        /// Создать модель VGG для получения признаков из изображений
        /// </summary>
        /// <returns>VGG Модель</returns>
        private Model CreateVGGModel()
        {
            //  размер входного изображения
            var inputShape = (256, 256, 3);
            //  загружаем натренерованную модель VGG19 на imagenet
            var vgg = new VGG19(weights: "imagenet");
            var dynamicVGG = vgg.ToPython() as dynamic;
            dynamicVGG.outputs = new object[] { dynamicVGG.layers[9].output };
            //  создаем вход для изображения
            var inputLayer = new Input(inputShape);
            //  получаем извлечение признаков
            var features = vgg.Call(inputLayer);
            //  создаем модель
            var model = new Model(new BaseLayer[] { inputLayer }, new BaseLayer[] { features });
            return model;
        }

        /// <summary>
        /// Выбрать случайные файлы из папки и вернуть изображения в виде массивов
        /// </summary>
        /// <param name="files">Файлы изображений</param>
        /// <param name="batchSize">Размер выборки</param>
        /// <param name="highResolutionShape">Размер больших изображений</param>
        /// <param name="lowResolutionShape">Размер малых изображений</param>
        /// <param name="cropImages">Случайно обрезать входное изображение до нужного размера</param>
        /// <returns>Случайные изображения большого и малого размера</returns>
        private (NDarray highResolutionImages, NDarray lowResolutionImages) GetRandomImages(
            string[] files,
            int batchSize,
            Shape highResolutionShape,
            Shape lowResolutionShape,
            bool cropImages)
        {
            //  выбираем случайные файлы
            var imagesBatch = np.random.choice(files.Length, size: new int[] { batchSize });
            //  создаем списки для изображений с малым размером и с большим
            var lowResImages = new List<NDarray>();
            var highResImages = new List<NDarray>();
            //  извлекаем случайные файлы
            var imgBatchData = imagesBatch.GetData<int>();
            //  проходим выбранные файлы
            foreach (var img in imgBatchData)
            {
                var bitmap = Bitmap.FromFile(files[img]) as Bitmap;
                if (cropImages)
                {
                    bitmap = bitmapActionFunc(bitmap, highResolutionShape[1], highResolutionShape[0]);
                }

                //  считываем файлы в нужном формате и с указанным размером
                var highResolution = bitmap
                    .Resize(highResolutionShape[0], highResolutionShape[1])
                    .ToRGBBitmap()
                    .ToByteImage()
                    .ToNDarray();
                var lowResolution = bitmap
                    .Resize(lowResolutionShape[0], lowResolutionShape[1])
                    .ToRGBBitmap()
                    .ToByteImage()
                    .ToNDarray();

                //  если рандом выпал < 0.5, то реверсим изображение
                if (np.random.rand() < 0.5)
                {
                    highResolution = np.fliplr(highResolution);
                    lowResolution = np.fliplr(lowResolution);
                }
                //  добавляем изображения
                highResImages.Add(highResolution);
                lowResImages.Add(lowResolution);
            }
            //  формируем массивы ndarray
            var (highResolutionImages, lowResolutionImages) = (np.array(highResImages), np.array(lowResImages));
            return (highResolutionImages, lowResolutionImages);
        }

        /// <summary>
        /// Масштабировать изображение
        /// </summary>
        /// <param name="imagePath">Путь до файла изображения</param>
        /// <param name="outputPath">Путь для сохранения</param>
        public void ScaleImage(string imagePath, string outputPath)
        {
            ExceptionHelper.ThrowIfFileNotExists(ModelPath);
            ExceptionHelper.ThrowIfFileNotExists(imagePath);

            //  загружаем модель
            var generator = BaseModel.LoadModel(ModelPath);
            //  загружаем изображение
            var bitmap = Bitmap.FromFile(imagePath) as Bitmap;
            var sizeShape = new Shape(bitmap.Height, bitmap.Width, 3);
            //  задаём размеры изображения
            var inputs = new Input(shape: sizeShape);
            //  формируем вход
            var outputs = generator.Call(inputs);
            //  создаём новую модель генератора на основе старой
            generator = new Model(inputs: new BaseLayer[] { inputs }, new BaseLayer[] { outputs });
            //  преобразуем изображение в массив float
            var lowResolutionImage = bitmap.ToRGBBitmap()
                                        .ToByteImage()
                                        .ToNDarray();
            //  пропускаем изображение через нейронную сеть
            var img = generator.PredictOnBatch(np.array(new List<NDarray> { lowResolutionImage }));
            //  получаем размеры увеличенного изображения
            var resultShape = img[0].shape;
            //  строим путь до файла изображения
            var path = new FilePathBuilder(imagePath)
                .SetPath(outputPath)
                .SetAlgorithmName("SRGAN")
                .SetScale(resultShape[0] / bitmap.Height)
                .Build();
            //  сохраняем изображение в файл
            KerasImage.SaveImg(path, img[0]);
        }

        /// <summary>
        /// Масштабировать изображение
        /// </summary>
        /// <param name="byteImage">Входное изображение</param>
        /// <returns>Увеличенное изображение</returns>
        public ByteImage ScaleImage(ByteImage byteImage)
        {
            ExceptionHelper.ThrowIfNull(byteImage, "byteImage");
            //  загружаем модель
            var generator = BaseModel.LoadModel(ModelPath);
            var sizeShape = new Shape(byteImage.Height, byteImage.Width, 3);
            //  задаём размеры изображения
            var inputs = new Input(shape: sizeShape);
            //  формируем вход
            var outputs = generator.Call(inputs);
            //  создаём новую модель генератора на основе старой
            generator = new Model(inputs: new BaseLayer[] { inputs }, new BaseLayer[] { outputs });
            //  преобразуем изображение в массив float
            var lowResolutionImage = byteImage.ToNDarray();
            //  пропускаем изображение через нейронную сеть
            var img = generator.PredictOnBatch(np.array(new List<NDarray> { lowResolutionImage }));
            //  возвращаем изображение
            return img[0].ToByteImage();
        }

        /// <summary>
        /// Обучить модель
        /// </summary>
        /// <param name="trainParams">Параметры обучения</param>
        public void Train(TrainParams trainParams)
        {
            ExceptionHelper.ThrowIfDirectoryNotExists(trainParams.TrainFilesPath);
            ExceptionHelper.ThrowIfNullOrEmpty(trainParams.TempPath, "trainParams.TempPath");
            ExceptionHelper.ThrowIfNullOrEmpty(trainParams.SaveDirectory, "trainParams.SaveDirectory");

            //  устанавливаем параметры
            SetParams(trainParams);

            //  формируем пути для выходных файлов
            var generatorPath = Path.Combine(trainParams.SaveDirectory, GeneratorFileName);
            var discriminatorPath = Path.Combine(trainParams.SaveDirectory, DiscriminatorFileName);
            var modelPath = Path.Combine(trainParams.SaveDirectory, ModelFileName);
            //  формируем путь для временных файлов
            var tempPath = Path.Combine(trainParams.TempPath, DateTime.Now.ToString().ToPathCorrect());
            Directory.CreateDirectory(tempPath);
            if (!Directory.Exists(trainParams.SaveDirectory))
            {
                Directory.CreateDirectory(trainParams.SaveDirectory);
            }

            //  указываем размеры изображений
            var lowResShape = (64, 64, 3);
            var highResShape = (256, 256, 3);
            //  создаем оптимизатор для всех нейронных сетей
            var commonOptimizer = new AdamTensorflow(0.0002f, 0.5f);
            //  создаем vgg модель
            var vgg = CreateVGGModel();
            (vgg.ToPython() as dynamic).trainable = false;
            vgg.Compile(loss: "mse", optimizer: commonOptimizer, metrics: new string[] { "accuracy" });
            //  создаем модель дискриминатора
            var discriminator = CreateDiscriminatorModel();
            discriminator.Compile(loss: "mse", optimizer: commonOptimizer, metrics: new string[] { "accuracy" });
            //  создаем генератор
            var generator = CreateGeneratorModel();
            //  создаем вход для изображений малого и большого размера
            var inputHighRes = new Input(shape: highResShape);
            var inputLowRes = new Input(shape: lowResShape);
            //  создаем слой генератора для создания изображений большого размера из изображений малого размера
            var generatedHighResolutionImages = generator.Call(inputLowRes);
            //  извлекаем слой признаков из сгенерированных изображений
            var features = vgg.Call(generatedHighResolutionImages);
            (discriminator.ToPython() as dynamic).trainable = false;
            //  извлекаем слой вероятности сгенерированного изображения
            var probs = discriminator.Call(generatedHighResolutionImages);
            //  загружаем веса, если файлы существуют
            if (File.Exists(generatorPath) && File.Exists(discriminatorPath))
            {
                generator.LoadWeight(generatorPath);
                discriminator.LoadWeight(discriminatorPath);
            }
            //  создаём модель состязательной сети
            var adversarialModel = new CompileModel(inputs: new BaseLayer[] { inputLowRes, inputHighRes }, outputs: new BaseLayer[] { probs, features });
            //  компилируем
            adversarialModel.Compile(loss: new string[] { "binary_crossentropy", "mse" }, loss_weights: new float[] { 1e-3f, 1 }, optimizer: commonOptimizer);

            //  считываем файлы из папки и запоминаем в массиве
            var images = Directory.GetFiles(trainParams.TrainFilesPath);

            //  обучаем в цикле
            for (int epoch = 1; epoch <= trainParams.Epochs; ++epoch)
            {
                ScaleLogger.Log($"Epoch: {epoch}/{trainParams.Epochs}");
                //  запоминаем время
                var time = DateTime.Now;
                //  обучение сети дискриминатора

                //  берем случайные изображения
                var (highResolutionImages, lowResolutionImages) = GetRandomImages(files: images,
                    batchSize: trainParams.BatchSize,
                    highResolutionShape: highResShape,
                    lowResolutionShape: lowResShape,
                    cropImages: trainParams.CropImage);
                //  генерируем изображения большого размера из изображений малого размера
                var generatedHighResolutionImagesPredict = generator.Predict(lowResolutionImages);
                //  создаем метки
                var realLabels = np.ones(trainParams.BatchSize, 16, 16, 1);
                var fakeLabels = np.zeros(trainParams.BatchSize, 16, 16, 1);
                //  обучаем дискриминатор на настоящий и сгенерированных изображениях
                var dLossReal = discriminator.TrainOnBatch(highResolutionImages, realLabels);
                var dLossFake = discriminator.TrainOnBatch(generatedHighResolutionImagesPredict, fakeLabels);
                //  вычисляем общую потерю дискриминатора
                var dLoss = 0.5 * np.add(dLossReal, dLossFake);
                ScaleLogger.Log("discriminator_loss:", dLoss.ToString());

                //  обучаем генератор

                //  берем случайные изображения
                (highResolutionImages, lowResolutionImages) = GetRandomImages(files: images,
                    batchSize: trainParams.BatchSize,
                    lowResolutionShape: lowResShape,
                    highResolutionShape: highResShape,
                    cropImages: trainParams.CropImage);

                //  извлекаем признаки из настоящих изображений с помощью VGG
                var imageFeatures = vgg.Predict(highResolutionImages);
                //  обучаем сеть генератора
                var gLoss = adversarialModel.TrainOnBatchEx(new NDarray[] { lowResolutionImages, highResolutionImages },
                    new NDarray[] { realLabels, imageFeatures });

                ScaleLogger.Log("generator_loss:", "[" + string.Join(' ', gLoss) + "]");

                //  если достигнут промежуточный шаг
                if (epoch % trainParams.IntermediateStep == 0)
                {
                    //  берём случайные изображения
                    (highResolutionImages, lowResolutionImages) = GetRandomImages(files: images,
                        batchSize: trainParams.BatchSize,
                        lowResolutionShape: lowResShape,
                        highResolutionShape: highResShape,
                        cropImages: trainParams.CropImage);

                    //  генерируем изображения
                    var generatedImages = generator.PredictOnBatch(lowResolutionImages);

                    //  создаем папку для эпохи
                    var epochPath = Path.Combine(tempPath, epoch.ToString());
                    Directory.CreateDirectory(epochPath);

                    //  сохраняем изображения
                    SaveImages(epochPath, lowResolutionImages, highResolutionImages, generatedImages);

                    //  делаем бэкап
                    MakeBackup(epochPath, generator, discriminator);

                    ScaleLogger.Log($"Epoch {epoch} saved");
                }

                ScaleLogger.Log($"Time: {DateTime.Now - time}");
            }
            //  сохраняем веса и модель
            generator.SaveWeight(generatorPath);
            discriminator.SaveWeight(discriminatorPath);
            generator.Save(modelPath);
        }

        /// <summary>
        /// Сохранить изображения на промежуточном шаге
        /// </summary>
        /// <param name="path">Путь для сохранения</param>
        /// <param name="lowResolutionImages">Изображения с низким размером</param>
        /// <param name="highResolutionImages">Оригинальные изображения</param>
        /// <param name="generatedImages">Сгенерированные изображения</param>
        private void SaveImages(string path, NDarray lowResolutionImages, NDarray highResolutionImages, NDarray generatedImages)
        {
            for (int index = 0; index < generatedImages.len; ++index)
            {
                KerasImage.SaveImg(Path.Combine(path, $"{index} low.png"), lowResolutionImages[index]);
                KerasImage.SaveImg(Path.Combine(path, $"{index} original.png"), highResolutionImages[index]);
                KerasImage.SaveImg(Path.Combine(path, $"{index} generated.png"), generatedImages[index]);
            }
        }

        /// <summary>
        /// Сделать бэкап генератора и дискриминатора
        /// </summary>
        /// <param name="path">Путь для сохранения</param>
        /// <param name="generator">Генератор</param>
        /// <param name="discriminator">Дискриминатор</param>
        private void MakeBackup(string path, Model generator, Model discriminator)
        {
            generator.SaveWeight(Path.Combine(path, GeneratorFileName));
            discriminator.SaveWeight(Path.Combine(path, DiscriminatorFileName));
        }

        /// <summary>
        /// Установить настройки обучения в зависимости от конфига
        /// </summary>
        /// <param name="trainParams">Настройки обучения</param>
        private void SetParams(TrainParams trainParams)
        {
            if (trainParams.CropImage)
            {
                if (trainParams.RandomCropImage)
                {
                    bitmapActionFunc = ImageHelper.GetRandomImagePart;
                }
                else
                {
                    bitmapActionFunc = ImageHelper.GetMiddleImagePart;
                }
            }
        }
    }
}