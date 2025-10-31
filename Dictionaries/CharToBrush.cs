using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TowerTool.Dictionaries
{
    public static class CharToBrush
    {
        public static readonly Dictionary<char, Brush> MapBrushes = new()
        {
            { '#', Brushes.DarkSlateGray }, // wall
            { 'c', Brushes.Goldenrod },     // chest
            { '.', Brushes.White },         // empty
            { ' ', Brushes.LightGray },     // floor
            { 's', Brushes.SteelBlue },     // player start
        };

        public static Brush GetBrush(char c)
        {
            return MapBrushes.TryGetValue(c, out var brush) ? brush : Brushes.Magenta; // Magenta = debug unknown char
        }
    }
}
