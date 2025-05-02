using System;
using System.Collections.Generic;

namespace GraphicsAlgorithmsApp.Models.Algorithms
{
    public class BresenhamLineAlgorithm : ILineAlgorithm, IGenericDrawingAlgorithm
    {
        public List<Point> CalculatePoints(Point start, Point end)
        {
            List<Point> points = new List<Point>();

            int x1 = (int)Math.Round(start.X);
            int y1 = (int)Math.Round(start.Y);
            int x2 = (int)Math.Round(end.X);
            int y2 = (int)Math.Round(end.Y);

            int dx = Math.Abs(x2 - x1);
            int dy = Math.Abs(y2 - y1);
            int sx = x1 < x2 ? 1 : -1;
            int sy = y1 < y2 ? 1 : -1;

            int err = dx - dy;

            while (true)
            {
                points.Add(new Point(x1, y1));
                if (x1 == x2 && y1 == y2)
                    break;

                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x1 += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y1 += sy;
                }
            }

            return points;
        }

        public IEnumerable<Point> Execute(Point start, Point end)
        {
            return CalculatePoints(start, end);
        }
    }
}