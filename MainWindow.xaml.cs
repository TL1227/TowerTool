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
        public string MapFileDir { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            MapFileDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            CurrentFilePath = System.IO.Path.Combine(MapFileDir, "map.txt");

            PaletteControl.BrushChanged += c => MapGridControl.ChangeBrush(c);
            MapGridControl.EndPaint += SaveChanges;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllLines(CurrentFilePath, MapGridControl.GetMapLines());
            MessageBox.Show($"Map exported to {CurrentFilePath}");
        }

        private void LiveEdit_Click(object sender, RoutedEventArgs e)
        {
            if (LiveEditMenu.IsChecked)
            {
                MapGridControl.LiveEditing = false;
                LiveEditMenu.IsChecked = false;
            }
            else
            {
                MapGridControl.LiveEditing = true;
                LiveEditMenu.IsChecked = true;
            }
        }

        private void SaveChanges()
        {
            File.WriteAllLines(CurrentFilePath, MapGridControl.GetMapLines());
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Open Map File",
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                InitialDirectory = MapFileDir
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    MapGridControl.LoadMap(dialog.FileName);
                }
                catch (IOException ex)
                {
                    MessageBox.Show($"Error reading file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}