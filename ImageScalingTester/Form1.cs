using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using ImageUpscaling.Managers;
using ImageUpscaling.Scaling;

namespace ImageScalingTester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var path = GetPath();
            if (!string.IsNullOrEmpty(path))
                textBox1.Text = path;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var path = GetPath();
            if (!string.IsNullOrEmpty(path))
                textBox2.Text = path;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            ImageScalingFactory imageScalingFactory = new ImageScalingFactory();
            double scale = (double)numericUpDown1.Value;
            Dictionary<string, KeyValuePair<TimeSpan, double>> stats = new Dictionary<string, KeyValuePair<TimeSpan, double>>();
            foreach (var scaling in imageScalingFactory.GetScaleObjects())
            {
                var image = ImageFileManager.Instance.Load(textBox1.Text);
                stopwatch.Restart();
                var result = scaling.ScaleImage(image, scale);
                stopwatch.Stop();
                var workTime = stopwatch.Elapsed;
                string path = Path.GetFullPath($"./output/x{scale} [{scaling.Title}] {Path.GetFileName(textBox2.Text)}");
                ImageFileManager.Instance.Save(result, path);
                double psnr = GetPSNR(ImageFileManager.Instance.Load(textBox2.Text), result);
                stats.Add(scaling.Title, new KeyValuePair<TimeSpan, double>(workTime, psnr));
            }
            File.WriteAllText($"./output/x{scale} {Path.GetFileName(textBox2.Text)}.txt", string.Join("\n", stats.Select(i => $"{i.Key}\t{i.Value.Key}\t{i.Value.Value}")));
        }

        private string GetPath()
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog1.FileName;
            }
            return string.Empty;
        }

        static double GetPSNR(BitmapSource i, BitmapSource k)
        {
            ByteImage first = ByteImage.FromBitmapSource(i);
            ByteImage second = ByteImage.FromBitmapSource(k);
            double mse = MSE(first, second);
            double max = 65025;

            return 10 * Math.Log10(max / mse);
        }

        static double MSE(ByteImage first, ByteImage second)
        {
            if (first.BytePerPixel != second.BytePerPixel
                || first.Width != second.Width
                || first.Height != second.Height)
            {
                throw new ArgumentException();
            }

            long result = 0;
            for (int y = 0; y < first.Height; ++y)
            {
                for (int x = 0; x < first.Width; ++x)
                {
                    for (int b = 0; b < first.BytePerPixel - 1; ++b)
                    {
                        int f = Math.Abs(first[y, x, b] - second[y, x, b]);
                        result += f * f;
                    }
                }
            }
            return result / (double)(first.Width * first.Height * (first.BytePerPixel - 1));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var first = ImageFileManager.Instance.Load(textBox1.Text);
            var second = ImageFileManager.Instance.Load(textBox2.Text);
            label4.Text = GetPSNR(second, first).ToString();
        }
    }
}
