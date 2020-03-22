using ImageScaling.Helpers;

using Keras;
using Keras.Layers;
using Keras.Models;

using Numpy;

using Python.Runtime;

using System.Collections.Generic;

namespace ImageScaling.KerasNet
{
    /// <summary>
    /// Модель Keras с множественными входами и выходами
    /// </summary>
    public class CompileModel : BaseModel
    {
        /// <summary>
        /// Компилируемая модель с множественными выходами
        /// </summary>
        /// <param name="inputs">Входные слои</param>
        /// <param name="outputs">Выходные слои</param>
        public CompileModel(BaseLayer[] inputs, BaseLayer[] outputs)
        {
            List<PyObject> inputList = new List<PyObject>();
            List<PyObject> outputList = new List<PyObject>();

            foreach (var item in inputs)
            {
                inputList.Add(item.ToPython());
            }

            foreach (var item in outputs)
            {
                outputList.Add(item.ToPython());
            }

            this.SetPrivateFieldValue("PyInstance", (object)Instance.keras.models.Model(inputList, outputList));
        }

        /// <summary>
        /// Compile
        /// </summary>
        /// <param name="optimizer"></param>
        /// <param name="loss"></param>
        /// <param name="metrics"></param>
        /// <param name="loss_weights"></param>
        /// <param name="sample_weight_mode"></param>
        /// <param name="weighted_metrics"></param>
        /// <param name="target_tensors"></param>
        public void Compile(StringOrInstance optimizer, string[] loss, string[] metrics = null, float[] loss_weights = null,
                       string sample_weight_mode = null, string[] weighted_metrics = null, NDarray[] target_tensors = null)
        {
            var args = new Dictionary<string, object>
            {
                ["optimizer"] = optimizer,
                ["loss"] = loss,
                ["metrics"] = metrics,
                ["loss_weights"] = loss_weights,
                ["sample_weight_mode"] = sample_weight_mode,
                ["weighted_metrics"] = weighted_metrics,
                ["target_tensors"] = target_tensors
            };

            this.InvokeMethod("compile", args);
        }

        /// <summary>
        /// Runs a single gradient update on a single batch of data.
        /// </summary>
        /// <param name="x">Numpy array of training data, or list of Numpy arrays if the model has multiple inputs. If all inputs in the model are named, you can also pass a dictionary mapping input names to Numpy arrays.</param>
        /// <param name="y">Numpy array of target data, or list of Numpy arrays if the model has multiple outputs. If all outputs in the model are named, you can also pass a dictionary mapping output names to Numpy arrays.</param>
        /// <param name="sample_weight">Optional array of the same length as x, containing weights to apply to the model's loss for each sample. In the case of temporal data, you can pass a 2D array with shape (samples, sequence_length), to apply a different weight to every timestep of every sample. In this case you should make sure to specify sample_weight_mode="temporal" in compile().</param>
        /// <param name="class_weight">Optional dictionary mapping class indices (integers) to a weight (float) to apply to the model's loss for the samples from this class during training. This can be useful to tell the model to "pay more attention" to samples from an under-represented class.</param>
        /// <returns>Scalar training loss (if the model has a single output and no metrics) or list of scalars (if the model has multiple outputs and/or metrics). The attribute model.metrics_names will give you the display labels for the scalar outputs.</returns>

        public double[] TrainOnBatchEx(NDarray[] x, NDarray[] y, NDarray sample_weight = null, Dictionary<int, float> class_weight = null)
        {
            var args = new Dictionary<string, object>
            {
                ["x"] = x,
                ["y"] = y,
                ["sample_weight"] = sample_weight,
                ["class_weight"] = class_weight
            };
            var pyresult = Base.InvokeStaticMethod(this.ToPython(), "train_on_batch", args);
            if (pyresult == null) return default;
            double[] result;
            if (!pyresult.IsIterable())
                result = new double[] { pyresult.As<double>() };
            else
                result = pyresult.As<double[]>();
            pyresult.Dispose();
            return result;
        }
    }
}