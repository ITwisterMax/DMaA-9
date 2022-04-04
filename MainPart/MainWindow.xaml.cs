using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Forms;
using ImageLoader;
using NeuralNetwork;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace MainPart
{
    /// <summary>
    ///     Main WPF class
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        ///     Image size
        /// </summary>
        private const int _imageSize = 30;

        /// <summary>
        ///     Bitmap
        /// </summary>
        private Bitmap _bitmap;

        /// <summary>
        ///     Network
        /// </summary>
        private NamedNeuralNetwork _network;

        /// <summary>
        ///     Default constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     On classification button click
        /// </summary>
        ///
        /// <param name="sender">Sender object</param>
        /// <param name="e">Arguments</param>
        private void ClassificationButton_Click(object sender, RoutedEventArgs e)
        {
            ClassificationResultLabel.Text = _network.GetAnswer(BitmapConverter.ToInt32List(_bitmap));
        }

        /// <summary>
        ///     On load image button click
        /// </summary>
        ///
        /// <param name="sender">Sender object</param>
        /// <param name="e">Arguments</param>
        private void LoadImageButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter ="Изображения (*.bmp, *.png, *.jpg, *.jpeg)|*.bmp;*.png;*.jpg;*.jpeg|Все файлы|*.*"
            };

            if (!(openFileDialog.ShowDialog() ?? false))
            {
                return;
            }
            
            _bitmap = BitmapConverter.Load(openFileDialog.FileName, _imageSize);
            CurrentImage.Source = BitmapConverter.ToBitmapImage(_bitmap);

            ClassificationButton.IsEnabled = true;
        }

        /// <summary>
        ///     On open network button click
        /// </summary>
        ///
        /// <param name="sender">Sender object</param>
        /// <param name="e">Arguments</param>
        private void OpenNetworkButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Сохранить сеть (*.snn)|*.snn"
            };

            if (openFileDialog.ShowDialog() ?? false)
            {
                using (var fileStream = new FileStream(openFileDialog.FileName, FileMode.Open))
                {
                    var serializer = new BinaryFormatter();
                    _network = (NamedNeuralNetwork)serializer.Deserialize(fileStream);
                }

                LoadImageButton.IsEnabled = true;
            }
        }

        /// <summary>
        ///     On teaching button click
        /// </summary>
        ///
        /// <param name="sender">Sender object</param>
        /// <param name="e">Arguments</param>
        private void TeachingButton_Click(object sender, RoutedEventArgs e)
        {
            var folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return;
            } 

            var directory = new DirectoryInfo(folderBrowserDialog.SelectedPath);
            var neuronsNames = GetNeuronsNames(directory);

            _network = new NamedNeuralNetwork(_imageSize * _imageSize, neuronsNames);

            while (! IsTeachingEnd(directory))
            {
                TeachFromDirectory(directory);
            }

            LoadImageButton.IsEnabled = true;
            SaveNetworkButton.IsEnabled = true;
        }

        /// <summary>
        ///     On save network button click
        /// </summary>
        ///
        /// <param name="sender">Sender object</param>
        /// <param name="e">Arguments</param>
        private void SaveNetworkButton_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Сохранить сеть (*.snn)|*.snn"
            };

            if (! (saveFileDialog.ShowDialog() ?? false))
            {
                return;
            } 

            using (var fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create))
            {
                var serializer = new BinaryFormatter();
                serializer.Serialize(fileStream, _network);
            }
        }

        /// <summary>
        ///     Get neurons names
        /// </summary>
        ///
        /// <param name="directory">Directory</param>
        ///
        /// <returns>List<string></returns>
        private static List<string> GetNeuronsNames(DirectoryInfo directory)
        {
            var neuronsNames = new List<string>();
            foreach (var file in directory.GetFiles())
            {
                var neuronNames = file.Name.Split('-')[0];
                if (! neuronsNames.Contains(neuronNames))
                {
                    neuronsNames.Add(neuronNames);
                }
            }

            return neuronsNames;
        }

        /// <summary>
        ///     Check if teaching is end
        /// </summary>
        ///
        /// <param name="directory">Directory</param>
        ///
        /// <returns>bool</returns>
        private bool IsTeachingEnd(DirectoryInfo directory)
        {
            return ! (from file in directory.GetFiles("*.png")
                     let neuronName = file.Name.Split('-')[0]
                     where _network.GetAnswer(GetElementFromPath(directory, file)) != neuronName
                     select file).Any();
        }

        /// <summary>
        ///     Teach from directory
        /// </summary>
        ///
        /// <param name="directory">Directory</param>
        private void TeachFromDirectory(DirectoryInfo directory)
        {
            foreach (var file in directory.GetFiles("*.png"))
            {
                var neuronName = file.Name.Split('-')[0];

                _network.Teaching(GetElementFromPath(directory, file), neuronName);
            }
        }

        /// <summary>
        ///     Fet element from path
        /// </summary>
        ///
        /// <param name="directory">Directory</param>
        /// <param name="file">File</param>
        ///
        /// <returns>List<int></returns>
        private static List<int> GetElementFromPath(DirectoryInfo directory, FileInfo file)
        {
            return BitmapConverter.ToInt32List(BitmapConverter.Load($"{directory}\\{file.Name}", _imageSize));
        }
    }
}