using System;
using System.Collections.Generic;

namespace GraphicsAlgorithmsApp.Models.Algorithms
{
    public class BresenhamCircleAlgorithm : ICircleAlgorithm, IGenericDrawingAlgorithm
    {
        public List<Point> CalculatePoints(Point center, double radius)
        {
            List<Point> points = new List<Point>();
            
            double x = 0;
            double y = radius;
            double d = 3 - 2 * radius;
            
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
        
        private void DrawCirclePoints(List<Point> points, double centerX, double centerY, double x, double y)
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
            double radius = Math.Sqrt(Math.Pow(end.X - start.X, 2) + Math.Pow(end.Y - start.Y, 2));
            return CalculatePoints(start, radius);
        }
    }
}