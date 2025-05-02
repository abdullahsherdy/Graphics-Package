using System;
using System.Collections.Generic;

namespace GraphicsAlgorithmsApp.Models.Algorithms
{
    public class DDALineAlgorithm : ILineAlgorithm, IGenericDrawingAlgorithm
    {
        public List<Point> CalculatePoints(Point start, Point end)
        {
            List<Point> points = new List<Point>();
            
            double dx = end.X - start.X;
            double dy = end.Y - start.Y;
            
            double steps = Math.Max(Math.Abs(dx), Math.Abs(dy));
            
            double xIncrement = dx / (float)steps;
            double yIncrement = dy / (float)steps;
            
            double x = start.X;
            double y = start.Y;
            
            points.Add(new Point(start.X, start.Y));
            
            for (int i = 0; i < steps; i++)
            {
                x += xIncrement;
                y += yIncrement;
                points.Add(new Point(Math.Round(x), (int)Math.Round(y)));
            }
            
            return points;
        }
        
        public IEnumerable<Point> Execute(Point start, Point end)
        {
            return CalculatePoints(start, end);
        }
    }
}