using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
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

namespace TowerTool
{
    public partial class MapGrid : UserControl
    {
        bool IsPainting { get; set; }
        Brush PaintColor {  get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        int TileSize { get; set; }

        public Tile[,] Map {  get; set; }

        public MapGrid()
        {
            InitializeComponent();

            IsPainting = false;
            PaintColor = Brushes.DarkKhaki;
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
                tile.Rect.Fill = PaintColor;
                tile.Character = '#';
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
    }
}
