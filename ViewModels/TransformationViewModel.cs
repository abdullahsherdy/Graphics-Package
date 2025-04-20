using System;
using System.Collections.Generic;
using GraphicsAlgorithmsApp.Models;
using GraphicsAlgorithmsApp.Models.Transformations;

namespace GraphicsAlgorithmsApp.ViewModels
{
    public class TransformationViewModel
    {
        public List<Point> OriginalPoints { get; set; }
        public List<Point> TransformedPoints { get; set; }
        
        public TransformationViewModel()
        {
            OriginalPoints = new List<Point>();
            TransformedPoints = new List<Point>();
        }
        
        public void ApplyTranslation(int tx, int ty)
        {
            TransformedPoints.Clear();
            foreach (var point in OriginalPoints)
            {
                TransformedPoints.Add(Transform2D.Translate(point, tx, ty));
            }
        }
        
        public void ApplyScaling(double sx, double sy, Point referencePoint = null)
        {
            TransformedPoints.Clear();
            foreach (var point in OriginalPoints)
            {
                TransformedPoints.Add(Transform2D.Scale(point, sx, sy, referencePoint));
            }
        }
        
        public void ApplyRotation(double angle, Point center = null)
        {
            TransformedPoints.Clear();
            foreach (var point in OriginalPoints)
            {
                TransformedPoints.Add(Transform2D.Rotate(point, angle, center));
            }
        }
        
        public void ApplyReflectionX(int yAxis = 0)
        {
            TransformedPoints.Clear();
            foreach (var point in OriginalPoints)
            {
                TransformedPoints.Add(Transform2D.ReflectX(point, yAxis));
            }
        }
        
        public void ApplyReflectionY(int xAxis = 0)
        {
            TransformedPoints.Clear();
            foreach (var point in OriginalPoints)
            {
                TransformedPoints.Add(Transform2D.ReflectY(point, xAxis));
            }
        }
        
        public void ApplyShearingX(double shx)
        {
            TransformedPoints.Clear();
            foreach (var point in OriginalPoints)
            {
                TransformedPoints.Add(Transform2D.ShearX(point, shx));
            }
        }
        
        public void ApplyShearingY(double shy)
        {
            TransformedPoints.Clear();
            foreach (var point in OriginalPoints)
            {
                TransformedPoints.Add(Transform2D.ShearY(point, shy));
            }
        }
    }
}