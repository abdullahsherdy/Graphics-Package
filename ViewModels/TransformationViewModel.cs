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
        private List<Transform2D> _transformationChain;
        private double _canvasHeight;
        private double _unitScale;
        private double _canvasCenterX;
        private double _canvasCenterY;

        public TransformationViewModel(double canvasHeight = 700, double unitScale = 100.0, double canvasCenterX = 350, double canvasCenterY = 350)
        {
            OriginalPoints = new List<Point>();
            TransformedPoints = new List<Point>();
            _transformationChain = new List<Transform2D>();
            _canvasHeight = canvasHeight;
            _unitScale = unitScale;
            _canvasCenterX = canvasCenterX;
            _canvasCenterY = canvasCenterY;
        }

        public void SetCanvasParameters(double height, double unitScale, double centerX, double centerY)
        {
            _canvasHeight = height;
            _unitScale = unitScale;
            _canvasCenterX = centerX;
            _canvasCenterY = centerY;
            ApplyTransformations();
        }

        public void ClearTransformations()
        {
            _transformationChain.Clear();
            ApplyTransformations();
        }

        public void AddTranslation(double tx, double ty)
        {
            _transformationChain.Add(Transform2D.Translation(tx, ty, _canvasHeight, _unitScale, _canvasCenterX, _canvasCenterY));
            ApplyTransformations();
        }

        public void AddScaling(double sx, double sy, Point referencePoint = null)
        {
            _transformationChain.Add(Transform2D.Scaling(sx, sy, referencePoint, _canvasHeight, _unitScale, _canvasCenterX, _canvasCenterY));
            ApplyTransformations();
        }

        public void AddRotation(double angle, Point center = null)
        {
            _transformationChain.Add(Transform2D.Rotation(angle, center, _canvasHeight, _unitScale, _canvasCenterX, _canvasCenterY));
            ApplyTransformations();
        }

        public void AddReflectionX(double yAxis = 0)
        {
            _transformationChain.Add(Transform2D.ReflectionX(yAxis, _canvasHeight, _unitScale, _canvasCenterX, _canvasCenterY));
            ApplyTransformations();
        }

        public void AddReflectionY(double xAxis = 0)
        {
            _transformationChain.Add(Transform2D.ReflectionY(xAxis, _canvasHeight, _unitScale, _canvasCenterX, _canvasCenterY));
            ApplyTransformations();
        }

        public void AddReflectionAboutYEqualsX()
        {
            _transformationChain.Add(Transform2D.ReflectionAboutYEqualsX(_canvasHeight, _unitScale, _canvasCenterX, _canvasCenterY));
            ApplyTransformations();
        }

        public void AddReflectionAboutOrigin()
        {
            _transformationChain.Add(Transform2D.ReflectionAboutOrigin(_canvasCenterX, _canvasCenterY, _canvasHeight, _unitScale));
            ApplyTransformations();
        }

        public void AddShearingX(double shx)
        {
            _transformationChain.Add(Transform2D.ShearX(shx, _canvasHeight, _unitScale, _canvasCenterX, _canvasCenterY));
            ApplyTransformations();
        }

        public void AddShearingY(double shy)
        {
            _transformationChain.Add(Transform2D.ShearY(shy, _canvasHeight, _unitScale, _canvasCenterX, _canvasCenterY));
            ApplyTransformations();
        }

        private void ApplyTransformations()
        {
            if (OriginalPoints.Count == 0) return;

            // Create a single transformation matrix from the chain
            var combinedTransform = Transform2D.CreateTransformationChain(
                _canvasHeight, _unitScale, _canvasCenterX, _canvasCenterY,
                _transformationChain.ToArray());

            // Apply the combined transformation to all points
            TransformedPoints = combinedTransform.TransformPoints(OriginalPoints);
        }

        public void SetOriginalPoints(IEnumerable<Point> points)
        {
            OriginalPoints.Clear();
            OriginalPoints.AddRange(points);
            ApplyTransformations();
        }
    }
}