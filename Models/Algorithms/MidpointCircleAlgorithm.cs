using System;
using System.Collections.Generic;
using GraphicsAlgorithmsApp.Models;

namespace GraphicsAlgorithmsApp.Models.Algorithms
{
    public class MidpointCircleAlgorithm : IDrawingAlgorithm
    {
        public IEnumerable<Point> Execute(Point center, Point edge)
        {
            int radius = (int)Math.Sqrt(Math.Pow(edge.X - center.X, 2) + Math.Pow(edge.Y - center.Y, 2));
            
            List<Point> points = new List<Point>();
            
            int x = 0;
            int y = radius;
            int p = 1 - radius;
            
            AddCirclePoints(points, center.X, center.Y, x, y);
            
            while (x < y)
            {
                x++;
                
                if (p < 0)
                    p += 2 * x + 1;
                else
                {
                    y--;
                    p += 2 * (x - y) + 1;
                }
                
                AddCirclePoints(points, center.X, center.Y, x, y);
            }
            
            return points;
        }
        
        private void AddCirclePoints(List<Point> points, double centerX, double centerY, double x, double y)
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
    }
}