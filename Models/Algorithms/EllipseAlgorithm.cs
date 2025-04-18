using System;
using System.Collections.Generic;

namespace GraphicsAlgorithmsApp.Models.Algorithms
{
    public class EllipseAlgorithm : IGenericDrawingAlgorithm
    {
        public List<Point> CalculatePoints(Point center, int radiusX, int radiusY)
        {
            List<Point> points = new List<Point>();
            
            int rx2 = radiusX * radiusX;
            int ry2 = radiusY * radiusY;
            int twoRx2 = 2 * rx2;
            int twoRy2 = 2 * ry2;
            int p;
            int x = 0;
            int y = radiusY;
            int px = 0;
            int py = twoRx2 * y;
            
            p = (int)(ry2 - (rx2 * radiusY) + (0.25 * rx2));
            
            AddEllipsePoints(points, center.X, center.Y, x, y);
            
            while (px < py)
            {
                x++;
                px += twoRy2;
                
                if (p < 0)
                    p += ry2 + px;
                else
                {
                    y--;
                    py -= twoRx2;
                    p += ry2 + px - py;
                }
                
                AddEllipsePoints(points, center.X, center.Y, x, y);
            }
            
            p = (int)(ry2 * (x + 0.5) * (x + 0.5) + rx2 * (y - 1) * (y - 1) - rx2 * ry2);
            
            while (y > 0)
            {
                y--;
                py -= twoRx2;
                
                if (p > 0)
                    p += rx2 - py;
                else
                {
                    x++;
                    px += twoRy2;
                    p += rx2 - py + px;
                }
                
                AddEllipsePoints(points, center.X, center.Y, x, y);
            }
            
            return points;
        }
        
        private void AddEllipsePoints(List<Point> points, int centerX, int centerY, int x, int y)
        {
            points.Add(new Point(centerX + x, centerY + y));
            points.Add(new Point(centerX - x, centerY + y));
            points.Add(new Point(centerX + x, centerY - y));
            points.Add(new Point(centerX - x, centerY - y));
        }
        
        public IEnumerable<Point> Execute(Point start, Point end)
        {
            int radiusX = Math.Abs(end.X - start.X);
            int radiusY = Math.Abs(end.Y - start.Y);
            return CalculatePoints(start, radiusX, radiusY);
        }
    }
}