using System;
using GraphicsAlgorithmsApp.Models;

namespace GraphicsAlgorithmsApp.Models.Transformations
{
    public class Transform2D
    {
        public static Point Translate(Point p, double tx, double ty)
        {
            return new Point(p.X + tx, p.Y + ty);
        }
        
        public static Point Scale(Point p, double sx, double sy, Point refPoint = null)
        {
            refPoint ??= new Point(0, 0);
            
            double newX = refPoint.X + (p.X - refPoint.X) * sx;
            double newY = refPoint.Y + (p.Y - refPoint.Y) * sy;
            
            return new Point(newX, newY);
        }
        
        public static Point Rotate(Point p, double angle, Point refPoint = null)
        {
            refPoint ??= new Point(0, 0);
            
            double radians = angle * Math.PI / 180.0;
            double cosTheta = Math.Cos(radians);
            double sinTheta = Math.Sin(radians);
            
            double x = p.X - refPoint.X;
            double y = p.Y - refPoint.Y;
            
            double newX = x * cosTheta - y * sinTheta + refPoint.X;
            double newY = x * sinTheta + y * cosTheta + refPoint.Y;
            
            return new Point(newX, newY);
        }
        
        public static Point ReflectX(Point p, double canvasCenterY = 0)
        {
            return new Point(p.X, 2 * canvasCenterY - p.Y);
        }

        public static Point ReflectY(Point p, double canvasCenterX = 0)
        {
            return new Point(2 * canvasCenterX - p.X, p.Y);
        }
        
        public static Point ReflectAboutYEqualsX(Point p)
        {
            return new Point(p.Y, p.X);
        }
        
        public static Point ReflectAboutOrigin(Point p, double canvasCenterX = 0, double canvasCenterY = 0)
        {
            return new Point(2 * canvasCenterX - p.X, 2 * canvasCenterY - p.Y);
        }
        
        public static Point ShearX(Point p, double shx)
        {
            return new Point(p.X + shx * p.Y, p.Y);
        }

        public static Point ShearY(Point p, double shy)
        {
            return new Point(p.X, p.Y + shy * p.X);
        }
    }
}