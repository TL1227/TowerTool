using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;

using TowerTool.Dictionaries;

namespace TowerTool
{
    public partial class MapGrid : UserControl
    {
        bool IsPainting { get; set; }
        Brush CurrentBrush {  get; set; }
        char CurrentChar {  get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public bool LiveEditing { get; set; }
        int TileSize { get; set; }
        public Tile[,] Map {  get; set; }

        public event Action EndPaint;

        public MapGrid()
        {
            InitializeComponent();

            IsPainting = false;
            CurrentBrush = CharToBrush.GetBrush('#');
            CurrentChar = '#';
            Rows = 20;
            Columns = 50;
            TileSize = 20;
            Map = new Tile[Rows, Columns];

            for (int i = 0; i < Rows; i++)
            {
                StackPanel stack = new() { Orientation = Orientation.Horizontal };

                for (int j = 0; j < Columns; j++)
                {
                    Tile tile = new(TileSize);
                    Map[i, j] = tile;
                    stack.Children.Add(tile.Rect);
                }

                MapGridMain.Children.Add(stack);
            }

            MapGridMain.MouseLeftButtonDown += MapGrid_MouseLeftButtonDown;
            MapGridMain.MouseLeftButtonUp += MapGrid_MouseLeftButtonUp;
            MapGridMain.MouseMove += MapGrid_MouseMove;
        }

        public void ChangeBrush(char c)
        {
            CurrentBrush = CharToBrush.GetBrush(c);
            CurrentChar = c;
        }

        private void MapGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsPainting = true;
            MapGridMain.CaptureMouse();
            PaintAtMouse(e.GetPosition(MapGridMain));
        }

        private void MapGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsPainting = false;
            MapGridMain.ReleaseMouseCapture();

            if (LiveEditing)
            {
                EndPaint.Invoke();
                SwitchToWindow("TowerRpg");
            }
        }

        private void MapGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsPainting)
            {
                PaintAtMouse(e.GetPosition(MapGridMain));
            }
        }

        private void PaintAtMouse(Point position)
        {
            int col = (int)(position.X / TileSize);
            int row = (int)(position.Y / TileSize);

            if (row >= 0 && row < Rows && col >= 0 && col < Columns)
            {
                Tile tile = Map[row, col];
                tile.Rect.Fill = CurrentBrush;
                tile.Character = CurrentChar;
            }
        }

        public List<string> GetMapLines()
        {
            var lines = new List<string>();

            for (int row = 0; row < Rows; row++)
            {
                char[] line = new char[Columns];

                for (int col = 0; col < Columns; col++)
                {
                    line[col] = Map[row, col].Character;
                }

                lines.Add(new string(line));
            }

            return lines;
        }

        public void LoadMap(string path)
        {
            var lines = File.ReadAllLines(path);

            int rows = Math.Min(lines.Length, Rows);

            for (int row = 0; row < rows; row++)
            {
                var line = lines[row];
                int cols = Math.Min(line.Length, Columns);

                for (int col = 0; col < cols; col++)
                {
                    char c = line[col];
                    Tile tile = Map[row, col];
                    tile.Character = c;
                    tile.Rect.Fill = CharToBrush.GetBrush(c);
                }
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public void SwitchToWindow(string processName)
        {
            var proc = Process.GetProcessesByName("TowerRpg");
            var hndl = proc[0].MainWindowHandle;
            SetForegroundWindow(hndl);
        }
    }
}
