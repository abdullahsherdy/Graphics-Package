using System;
using System.Collections.Generic;

namespace GraphicsAlgorithmsApp.Models.Algorithms
{
    public class BresenhamLineAlgorithm : ILineAlgorithm, IGenericDrawingAlgorithm
    {
        public List<Point> CalculatePoints(Point start, Point end)
        {
            List<Point> points = new List<Point>();
            
            int x1 = start.X;
            int y1 = start.Y;
            int x2 = end.X;
            int y2 = end.Y;
            
            int dx = Math.Abs(x2 - x1);
            int dy = -Math.Abs(y2 - y1);
            
            int sx = x1 < x2 ? 1 : -1;
            int sy = y1 < y2 ? 1 : -1;
            
            int error = dx + dy;
            int e2;
            
            while (true)
            {
                points.Add(new Point(x1, y1));
                
                if (x1 == x2 && y1 == y2)
                    break;
                
                e2 = 2 * error;
                
                if (e2 >= dy)
                {
                    if (x1 == x2)
                        break;
                    error += dy;
                    x1 += sx;
                }
                
                if (e2 <= dx)
                {
                    if (y1 == y2)
                        break;
                    error += dx;
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