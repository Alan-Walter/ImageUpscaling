using ImageUpscaling.Desktop.Core.ViewModel;

using System.Windows;

namespace ImageUpscaling.Desktop.Core.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel mainViewModel;

        public MainWindow()
        {
            InitializeComponent();
            mainViewModel = new MainViewModel();
            DataContext = mainViewModel;
            Keras.Keras.DisablePySysConsoleLog = true;
        }
    }
}