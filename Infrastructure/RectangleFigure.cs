using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rectangles.Infrastructure
{
    /// <summary>
    /// Класс, описывающий фигуру треугольника с доп. параметрами
    /// </summary>
    public class RectangleFigure:IRectangleFigure
    {
        public Rectangle Rectangle { get; }
        public Color RecColor { get; }
        public int RemovalCycles { get; set; }
        public Random random { get; }

        public RectangleFigure(int x, int y, Color color, int seed)
        {
            random = new Random();
            RecColor = color;
            Rectangle = new Rectangle(x, y, random.Next(seed), random.Next(seed));
            RemovalCycles = -1;
        }

        public bool CheckIntersections(RectangleFigure rec)
        {
            if (Rectangle.IntersectsWith(rec.Rectangle))
            {
                return true;
            }
            return false;
        }
    }
}
