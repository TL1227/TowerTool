using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TowerTool
{
    public partial class MainWindow : Window
    {
        public string CurrentFilePath { get; set; }
        public DirectoryInfo DataDir { get; set; }
        private FileSystemWatcher Watcher { get; set; }


        public MainWindow()
        {
            InitializeComponent();

            DataDir = new("..\\..\\..\\..\\TowerRpg\\data");

            PaletteControl.BrushChanged += c => MapGridControl.ChangeBrush(c);
            MapGridControl.EndPaint += SaveCurrentFile;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentFilePath == null)
            {
                var dialog = new SaveFileDialog()
                {
                    Title = "Save New Map File",
                    Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                    InitialDirectory = DataDir.FullName
                };

                if (dialog.ShowDialog() == true)
                {
                    try
                    {
                        CurrentFilePath = dialog.FileName;
                        SaveCurrentFile();
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                SaveCurrentFile();
                MessageBox.Show($"Saved to {CurrentFilePath}");
            }
        }

        private void LiveEdit_Click(object sender, RoutedEventArgs e)
        {
            if (LiveEditMenu.IsChecked)
            {
                MapGridControl.LiveEditing = false;
                LiveEditMenu.IsChecked = false;
                Watcher.Dispose();
            }
            else
            {
                Watcher = new(DataDir.FullName, "pos.txt");
                Watcher.NotifyFilter = NotifyFilters.LastWrite;
                Watcher.EnableRaisingEvents = true;

                Watcher.Changed += OnFileChange;

                MapGridControl.LiveEditing = true;
                LiveEditMenu.IsChecked = true;
            }
        }

        private void OnFileChange(object sender, FileSystemEventArgs e)
        {
            Dispatcher.Invoke(async () =>
            {
                for (int i = 0; i < 5; i++)
                {
                    try
                    {
                        //we invert the x,z because of how c# and c++ 2D arrays work
                        var positionFile = File.ReadAllText(e.FullPath);
                        int x = int.Parse(positionFile.Split(' ')[1]);
                        int z = int.Parse(positionFile.Split(' ')[0]);
                        MapGridControl.PaintPlayerLoc(x, z);
                        break;
                    }
                    catch (IOException)
                    {
                        await Task.Delay(10); // wait a few ms and retry
                    }
                }
            });
        }

        private void SaveCurrentFile()
        {
            File.WriteAllLines(CurrentFilePath, MapGridControl.GetMapLines());
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {

            var dialog = new OpenFileDialog
            {
                Title = "Open Map File",
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                InitialDirectory = DataDir.FullName
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    MapGridControl.LoadMap(dialog.FileName);
                    CurrentFilePath = dialog.FileName;
                }
                catch (IOException ex)
                {
                    MessageBox.Show($"Error reading file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
