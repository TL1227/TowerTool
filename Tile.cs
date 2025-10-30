using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace TowerTool
{
    public class Tile 
    {
        public Rectangle Rect { get; set; }
        public char Character { get; set; }

        private char Default = '.';

        public Tile(int size)
        {
            Rect = new()
            {
                Width = size,
                Height = size,
                Fill = Brushes.White,
                Stroke = Brushes.LightGray,
                StrokeThickness = 0.5,
                Tag = this
            };

            Character = Default;
        }
    }
}
