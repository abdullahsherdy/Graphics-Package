using System;
using System.Collections.Generic;

namespace GraphicsAlgorithmsApp.Models.Algorithms
{
    public class BresenhamCircleAlgorithm : ICircleAlgorithm, IGenericDrawingAlgorithm
    {
        public List<Point> CalculatePoints(Point center, int radius)
        {
            List<Point> points = new List<Point>();
            
            int x = 0;
            int y = radius;
            int d = 3 - 2 * radius;
            
            DrawCirclePoints(points, center.X, center.Y, x, y);
            
            while (y >= x)
            {
                x++;
                
                if (d > 0)
                {
                    y--;
                    d = d + 4 * (x - y) + 10;
                }
                else
                {
                    d = d + 4 * x + 6;
                }
                
                DrawCirclePoints(points, center.X, center.Y, x, y);
            }
            
            return points;
        }
        
        private void DrawCirclePoints(List<Point> points, int centerX, int centerY, int x, int y)
        {
            points.Add(new Point(centerX + x, centerY + y));
            points.Add(new Point(centerX - x, centerY + y));
            points.Add(new Point(centerX + x, centerY - y));
            points.Add(new Point(centerX - x, centerY - y));
            points.Add(new Point(centerX + y, centerY + x));
            points.Add(new Point(centerX - y, centerY + x));
            points.Add(new Point(centerX + y, centerY - x));
            points.Add(new Point(centerX - y, centerY - x));
        }
        
        public IEnumerable<Point> Execute(Point start, Point end)
        {
            int radius = (int)Math.Sqrt(Math.Pow(end.X - start.X, 2) + Math.Pow(end.Y - start.Y, 2));
            return CalculatePoints(start, radius);
        }
    }
}