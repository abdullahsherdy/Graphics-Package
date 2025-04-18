using System;
using GraphicsAlgorithmsApp.Models;

namespace GraphicsAlgorithmsApp.Models.Transformations
{
    public class Transform2D
    {
    public static Point Translate(Point p, double tx, double ty)
    {
        return new Point(p.X + (int)tx, p.Y + (int)ty);
    }
        
        public static Point Scale(Point p, double sx, double sy, Point refPoint = null)
        {
            refPoint ??= new Point(0, 0);
            
            int newX = (int)(refPoint.X + (p.X - refPoint.X) * sx);
            int newY = (int)(refPoint.Y + (p.Y - refPoint.Y) * sy);
            
            return new Point(newX, newY);
        }
        
        public static Point Rotate(Point p, double angle, Point refPoint = null)
        {
            refPoint ??= new Point(0, 0);
            
            double radians = angle * Math.PI / 180.0;
            double cosTheta = Math.Cos(radians);
            double sinTheta = Math.Sin(radians);
            
            int x = p.X - refPoint.X;
            int y = p.Y - refPoint.Y;
            
            int newX = (int)(x * cosTheta - y * sinTheta + refPoint.X);
            int newY = (int)(x * sinTheta + y * cosTheta + refPoint.Y);
            
            return new Point(newX, newY);
        }
        
        public static Point ReflectX(Point p, int yAxis = 0)
        {
            return new Point(p.X, 2 * yAxis - p.Y);
        }

        public static Point ReflectY(Point p, int xAxis = 0)
        {
            return new Point(2 * xAxis - p.X, p.Y);
        }
        
        public static Point ReflectAboutYEqualsX(Point p)
        {
            return new Point(p.Y, p.X);
        }
        
        public static Point ReflectAboutOrigin(Point p)
        {
            return new Point(-p.X, -p.Y);
        }
        
        public static Point ShearX(Point p, double shx)
        {
            return new Point((int)(p.X + shx * p.Y), p.Y);
        }

        public static Point ShearY(Point p, double shy)
        {
            return new Point(p.X, (int)(p.Y + shy * p.X));
        }
    }
}