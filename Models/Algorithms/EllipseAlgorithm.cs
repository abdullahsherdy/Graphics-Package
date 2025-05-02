using System;
using System.Collections.Generic;

namespace GraphicsAlgorithmsApp.Models.Algorithms
{
    public class EllipseAlgorithm : IGenericDrawingAlgorithm
    {
        public List<Point> CalculatePoints(Point center, double radiusX, double radiusY)
        {
            List<Point> points = new List<Point>();
            
            double rx2 = radiusX * radiusX;
            double ry2 = radiusY * radiusY;
            double twoRx2 = 2 * rx2;
            double twoRy2 = 2 * ry2;
            double p;
            double x = 0;
            double y = radiusY;
            double px = 0;
            double py = twoRx2 * y;
            
            p = (ry2 - (rx2 * radiusY) + (0.25 * rx2));
            
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
            
            p = (ry2 * (x + 0.5) * (x + 0.5) + rx2 * (y - 1) * (y - 1) - rx2 * ry2);
            
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
        
        private void AddEllipsePoints(List<Point> points, double centerX, double centerY, double x, double y)
        {
            points.Add(new Point(centerX + x, centerY + y));
            points.Add(new Point(centerX - x, centerY + y));
            points.Add(new Point(centerX + x, centerY - y));
            points.Add(new Point(centerX - x, centerY - y));
        }
        
        public IEnumerable<Point> Execute(Point start, Point end)
        {
            double radiusX = Math.Abs(end.X - start.X);
            double radiusY = Math.Abs(end.Y - start.Y);
            return CalculatePoints(start, radiusX, radiusY);
        }
    }
}