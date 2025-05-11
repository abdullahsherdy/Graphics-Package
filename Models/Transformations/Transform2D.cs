using System;
using System.Collections.Generic;
using GraphicsAlgorithmsApp.Models;

namespace GraphicsAlgorithmsApp.Models.Transformations
{
    public class Transform2D
    {
        private readonly double[,] _matrix;
        private const int MATRIX_SIZE = 3; // 3x3 matrix for 2D transformations
        private readonly double _canvasHeight;
        private readonly double _unitScale;
        private readonly double _canvasCenterX;
        private readonly double _canvasCenterY;

        public Transform2D(double canvasHeight = 0, double unitScale = 100.0, double canvasCenterX = 0, double canvasCenterY = 0)
        {
            _matrix = new double[MATRIX_SIZE, MATRIX_SIZE];
            _canvasHeight = canvasHeight;
            _unitScale = unitScale;
            _canvasCenterX = canvasCenterX;
            _canvasCenterY = canvasCenterY;
            SetIdentity();
        }

        private void SetIdentity()
        {
            for (int i = 0; i < MATRIX_SIZE; i++)
                for (int j = 0; j < MATRIX_SIZE; j++)
                    _matrix[i, j] = (i == j) ? 1.0 : 0.0;
        }

        public static Transform2D Identity(double canvasHeight = 0, double unitScale = 100.0, double canvasCenterX = 0, double canvasCenterY = 0)
            => new Transform2D(canvasHeight, unitScale, canvasCenterX, canvasCenterY);

        // Convert screen coordinates to unit circle coordinates
        private Point ToUnitCircleCoordinates(Point screenPoint)
        {
            double unitX = (screenPoint.X - _canvasCenterX) / _unitScale;
            double unitY = (_canvasCenterY - screenPoint.Y) / _unitScale;
            return new Point(unitX, unitY);
        }

        // Convert unit circle coordinates back to screen coordinates
        private Point ToScreenCoordinates(Point unitPoint)
        {
            double screenX = unitPoint.X * _unitScale + _canvasCenterX;
            double screenY = _canvasCenterY - unitPoint.Y * _unitScale;
            return new Point(screenX, screenY);
        }

        public static Transform2D Translation(double tx, double ty, double canvasHeight = 0, double unitScale = 100.0, double canvasCenterX = 0, double canvasCenterY = 0)
        {
            var transform = new Transform2D(canvasHeight, unitScale, canvasCenterX, canvasCenterY);
            // Convert translation to unit circle coordinates
            double unitTx = tx / unitScale;
            double unitTy = ty / unitScale;
            transform._matrix[0, 2] = unitTx;
            transform._matrix[1, 2] = unitTy;
            return transform;
        }

        public static Transform2D Scaling(double sx, double sy, Point refPoint = null, double canvasHeight = 0, double unitScale = 100.0, double canvasCenterX = 0, double canvasCenterY = 0)
        {
            var transform = new Transform2D(canvasHeight, unitScale, canvasCenterX, canvasCenterY);
            refPoint ??= new Point(0, 0);

            // Convert reference point to unit circle coordinates
            var unitRefPoint = transform.ToUnitCircleCoordinates(refPoint);

            transform._matrix[0, 0] = sx;
            transform._matrix[1, 1] = sy;
            transform._matrix[0, 2] = unitRefPoint.X * (1 - sx);
            transform._matrix[1, 2] = unitRefPoint.Y * (1 - sy);
            return transform;
        }

        public static Transform2D Rotation(double angle, Point refPoint = null, double canvasHeight = 0, double unitScale = 100.0, double canvasCenterX = 0, double canvasCenterY = 0)
        {
            var transform = new Transform2D(canvasHeight, unitScale, canvasCenterX, canvasCenterY);
            refPoint ??= new Point(0, 0);

            // Convert reference point to unit circle coordinates
            var unitRefPoint = transform.ToUnitCircleCoordinates(refPoint);

            double radians = angle * Math.PI / 180.0;
            double cosTheta = Math.Cos(radians);
            double sinTheta = Math.Sin(radians);

            transform._matrix[0, 0] = cosTheta;
            transform._matrix[0, 1] = -sinTheta;
            transform._matrix[1, 0] = sinTheta;
            transform._matrix[1, 1] = cosTheta;
            transform._matrix[0, 2] = unitRefPoint.X * (1 - cosTheta) + unitRefPoint.Y * sinTheta;
            transform._matrix[1, 2] = unitRefPoint.Y * (1 - cosTheta) - unitRefPoint.X * sinTheta;
            return transform;
        }

        public static Transform2D ReflectionX(double yAxis = 0, double canvasHeight = 0, double unitScale = 100.0, double canvasCenterX = 0, double canvasCenterY = 0)
        {
            var transform = new Transform2D(canvasHeight, unitScale, canvasCenterX, canvasCenterY);
            // Convert yAxis to unit circle coordinates
            double unitYAxis = (canvasCenterY - yAxis) / unitScale;
            transform._matrix[1, 1] = -1;
            transform._matrix[1, 2] = 2 * unitYAxis;
            return transform;
        }

        public static Transform2D ReflectionY(double xAxis = 0, double canvasHeight = 0, double unitScale = 100.0, double canvasCenterX = 0, double canvasCenterY = 0)
        {
            var transform = new Transform2D(canvasHeight, unitScale, canvasCenterX, canvasCenterY);
            // Convert xAxis to unit circle coordinates
            double unitXAxis = (xAxis - canvasCenterX) / unitScale;
            transform._matrix[0, 0] = -1;
            transform._matrix[0, 2] = 2 * unitXAxis;
            return transform;
        }

        public static Transform2D ReflectionAboutYEqualsX(double canvasHeight = 0, double unitScale = 100.0, double canvasCenterX = 0, double canvasCenterY = 0)
        {
            var transform = new Transform2D(canvasHeight, unitScale, canvasCenterX, canvasCenterY);
            transform._matrix[0, 0] = 0;
            transform._matrix[0, 1] = 1;
            transform._matrix[1, 0] = 1;
            transform._matrix[1, 1] = 0;
            return transform;
        }

        public static Transform2D ReflectionAboutOrigin(double canvasCenterX = 0, double canvasCenterY = 0, double canvasHeight = 0, double unitScale = 100.0)
        {
            var transform = new Transform2D(canvasHeight, unitScale, canvasCenterX, canvasCenterY);
            transform._matrix[0, 0] = -1;
            transform._matrix[1, 1] = -1;
            return transform;
        }

        public static Transform2D ShearX(double shx, double canvasHeight = 0, double unitScale = 100.0, double canvasCenterX = 0, double canvasCenterY = 0)
        {
            var transform = new Transform2D(canvasHeight, unitScale, canvasCenterX, canvasCenterY);
            transform._matrix[0, 1] = shx;
            return transform;
        }

        public static Transform2D ShearY(double shy, double canvasHeight = 0, double unitScale = 100.0, double canvasCenterX = 0, double canvasCenterY = 0)
        {
            var transform = new Transform2D(canvasHeight, unitScale, canvasCenterX, canvasCenterY);
            transform._matrix[1, 0] = shy;
            return transform;
        }

        public Transform2D Compose(Transform2D other)
        {
            var result = new Transform2D(_canvasHeight, _unitScale, _canvasCenterX, _canvasCenterY);
            for (int i = 0; i < MATRIX_SIZE; i++)
            {
                for (int j = 0; j < MATRIX_SIZE; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < MATRIX_SIZE; k++)
                    {
                        sum += _matrix[i, k] * other._matrix[k, j];
                    }
                    result._matrix[i, j] = sum;
                }
            }
            return result;
        }

        public Point TransformPoint(Point p)
        {
            // Convert to unit circle coordinates, transform, then convert back
            var unitPoint = ToUnitCircleCoordinates(p);
            double x = unitPoint.X * _matrix[0, 0] + unitPoint.Y * _matrix[0, 1] + _matrix[0, 2];
            double y = unitPoint.X * _matrix[1, 0] + unitPoint.Y * _matrix[1, 1] + _matrix[1, 2];
            return ToScreenCoordinates(new Point(x, y));
        }

        public List<Point> TransformPoints(IEnumerable<Point> points)
        {
            var result = new List<Point>();
            foreach (var point in points)
            {
                result.Add(TransformPoint(point));
            }
            return result;
        }

        public static Transform2D CreateTransformationChain(double canvasHeight, double unitScale, double canvasCenterX, double canvasCenterY, params Transform2D[] transformations)
        {
            if (transformations == null || transformations.Length == 0)
                return Identity(canvasHeight, unitScale, canvasCenterX, canvasCenterY);

            var result = transformations[0];
            for (int i = 1; i < transformations.Length; i++)
            {
                result = result.Compose(transformations[i]);
            }
            return result;
        }
    }
}