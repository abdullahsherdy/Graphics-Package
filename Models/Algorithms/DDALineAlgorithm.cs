using System;
using System.Collections.Generic;

namespace GraphicsAlgorithmsApp.Models.Algorithms
{
    public class DDALineAlgorithm : ILineAlgorithm, IGenericDrawingAlgorithm
    {
        public List<Point> CalculatePoints(Point start, Point end)
        {
            List<Point> points = new List<Point>();
            
            int dx = end.X - start.X;
            int dy = end.Y - start.Y;
            
            int steps = Math.Max(Math.Abs(dx), Math.Abs(dy));
            
            float xIncrement = dx / (float)steps;
            float yIncrement = dy / (float)steps;
            
            float x = start.X;
            float y = start.Y;
            
            points.Add(new Point(start.X, start.Y));
            
            for (int i = 0; i < steps; i++)
            {
                x += xIncrement;
                y += yIncrement;
                points.Add(new Point((int)Math.Round(x), (int)Math.Round(y)));
            }
            
            return points;
        }
        
        public IEnumerable<Point> Execute(Point start, Point end)
        {
            return CalculatePoints(start, end);
        }
    }
}