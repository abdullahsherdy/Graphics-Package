using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using GraphicsAlgorithmsApp.Models;
using GraphicsAlgorithmsApp.Models.Algorithms;
using GraphicsAlgorithmsApp.Models.Transformations;
using GraphicsAlgorithmsApp.ViewModels;
using System;
using System.Collections.Generic;
using Point = GraphicsAlgorithmsApp.Models.Point;

namespace GraphicsAlgorithmsApp.Views
{
    public partial class MainWindow : Window
    {
        private Canvas _canvas;
        private Canvas _transformationCanvas;
        private MainWindowViewModel _viewModel;
        private Models.Point _startPoint;
        private bool _isDrawing = false;
        private List<Point> _currentPoints = new List<Point>();
        private int _canvasCenterX = 250;
        private int _canvasCenterY = 250;
        private double _unitScale = 50.0;
        private List<Point> _transformedState = new List<Point>();

        public MainWindow()
        {
            InitializeComponent();
            _canvas = this.FindControl<Canvas>("DrawingCanvas");
            _transformationCanvas = this.FindControl<Canvas>("TransformationCanvas");
            
            DataContext = _viewModel = new MainWindowViewModel();
            _viewModel.ClearAction = ClearCanvas;
            _viewModel.ApplyTransformationAction = ApplyTransformation;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void Canvas_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            var position = e.GetPosition(_canvas);
            _startPoint = new Models.Point((int)position.X, (int)position.Y);
            _isDrawing = true;
            
            var unitCoords = UnitCircleCoordinates.ScreenToUnitCircle(
                _startPoint.X, _startPoint.Y, _canvasCenterX, _canvasCenterY, _unitScale);
            _viewModel.UnitX = Math.Round(unitCoords.Item1, 2);
            _viewModel.UnitY = Math.Round(unitCoords.Item2, 2);
            
            if (_viewModel.IsCircleDrawing)
            {
                var endPoint = new Models.Point(_startPoint.X + 5, _startPoint.Y);
                DrawShape(_startPoint, endPoint);
            }
        }

        private void Canvas_PointerMoved(object sender, PointerEventArgs e)
        {
            var position = e.GetPosition(_canvas);
            var currentPoint = new Models.Point((int)position.X, (int)position.Y);
            
            var unitCoords = UnitCircleCoordinates.ScreenToUnitCircle(
                currentPoint.X, currentPoint.Y, _canvasCenterX, _canvasCenterY, _unitScale);
            _viewModel.UnitX = Math.Round(unitCoords.Item1, 2);
            _viewModel.UnitY = Math.Round(unitCoords.Item2, 2);
            
            if (!_isDrawing) return;

            if (_viewModel.ShowPreview)
            {
                ClearPreview();
                DrawPreview(_startPoint, currentPoint);
            }
        }

        private void Canvas_PointerReleased(object sender, PointerReleasedEventArgs e)
        {
            if (!_isDrawing) return;
            
            var position = e.GetPosition(_canvas);
            var endPoint = new Models.Point((int)position.X, (int)position.Y);
            
            DrawShape(_startPoint, endPoint);
            _isDrawing = false;
        }

        private void DrawPreview(Models.Point start, Models.Point end)
        {
            List<Point> previewPoints = new List<Point>();
            
            switch (_viewModel.SelectedAlgorithmIndex)
            {
                case 0:
                    var ddaAlgorithm = new DDALineAlgorithm();
                    previewPoints.AddRange(ddaAlgorithm.CalculatePoints(start, end));
                    break;
                case 1:
                    var bresenhamLineAlgorithm = new BresenhamLineAlgorithm();
                    previewPoints.AddRange(bresenhamLineAlgorithm.CalculatePoints(start, end));
                    break;
                case 2:
                    var circleAlgorithm = new BresenhamCircleAlgorithm();
                    int radius = CalculateDistance(start, end);
                    previewPoints.AddRange(circleAlgorithm.CalculatePoints(start, radius));
                    break;
                case 3:
                    var ellipseAlgorithm = new EllipseAlgorithm();
                    int radiusX = Math.Abs(end.X - start.X);
                    int radiusY = Math.Abs(end.Y - start.Y);
                    previewPoints.AddRange(ellipseAlgorithm.CalculatePoints(start, radiusX, radiusY));
                    break;
            }
            
            foreach (var point in previewPoints)
            {
                DrawPixel(_canvas, point.X, point.Y, Colors.Gray, true);
            }
        }

    private void DrawShape(Models.Point start, Models.Point end)
    {
        _currentPoints.Clear();
        _transformedState.Clear();

        switch (_viewModel.SelectedAlgorithmIndex)
        {
            case 0:
                var ddaAlgorithm = new DDALineAlgorithm();
                _currentPoints.AddRange(ddaAlgorithm.CalculatePoints(start, end));
                break;
            case 1:
                var bresenhamLineAlgorithm = new BresenhamLineAlgorithm();
                _currentPoints.AddRange(bresenhamLineAlgorithm.CalculatePoints(start, end));
                break;
            case 2:
                var circleAlgorithm = new BresenhamCircleAlgorithm();
                int radius = CalculateDistance(start, end);
                _currentPoints.AddRange(circleAlgorithm.CalculatePoints(start, radius));
                break;
            case 3:
                var ellipseAlgorithm = new EllipseAlgorithm();
                int radiusX = Math.Abs(end.X - start.X);
                int radiusY = Math.Abs(end.Y - start.Y);
                _currentPoints.AddRange(ellipseAlgorithm.CalculatePoints(start, radiusX, radiusY));
                break;
            }

            foreach (var point in _currentPoints)
            DrawPixel(_canvas, point.X, point.Y, _viewModel.SelectedColor);
    }  


        private void ClearPreview()
        {
            List<Control> toRemove = new List<Control>();
            foreach (var child in _canvas.Children)
            {
                if (child is Control control && control.Tag?.ToString() == "Preview")
                {
                    toRemove.Add(control);
                }
            }

            foreach (var control in toRemove)
            {
                _canvas.Children.Remove(control);
            }
        }

        private void DrawPixel(Canvas targetCanvas, int x, int y, Color color, bool isPreview = false)
        {
            var ellipse = new Avalonia.Controls.Shapes.Ellipse
            {
                Width = 2,
                Height = 2,
                Fill = new SolidColorBrush(color)
            };
            
            if (isPreview)
            {
                ellipse.Tag = "Preview";
            }
            
            Canvas.SetLeft(ellipse, x - 1);
            Canvas.SetTop(ellipse, y - 1);
            targetCanvas.Children.Add(ellipse);
        }

        private void ApplyTransformation()
        {
            if (_currentPoints.Count == 0) return;

            _transformationCanvas.Children.Clear();
            List<Point> sourcePoints = _transformedState.Count > 0 
                ? new List<Point>(_transformedState) 
                : new List<Point>(_currentPoints);

        List<Point> transformedPoints = new List<Point>();
            Point referencePoint = CalculateCentroid(sourcePoints);

            switch (_viewModel.SelectedTransformationIndex)
            {
                case 0: // Translation
                    foreach (var point in sourcePoints)
                        transformedPoints.Add(Transform2D.Translate(point, _viewModel.TranslateX, _viewModel.TranslateY));
                    break;
                case 1: // Scaling
                    foreach (var point in sourcePoints)
                        transformedPoints.Add(Transform2D.Scale(point, _viewModel.ScaleX, _viewModel.ScaleY, referencePoint));
                    break;
                case 2: // Rotation
                    foreach (var point in sourcePoints)
                        transformedPoints.Add(Transform2D.Rotate(point, _viewModel.RotationAngle, referencePoint));
                    break;
                case 3: // Reflection X-Axis
                    foreach (var point in sourcePoints)
                        transformedPoints.Add(Transform2D.ReflectX(point, 0));
                    break;
                case 4: // Reflection Y-Axis
                    foreach (var point in sourcePoints)
                        transformedPoints.Add(Transform2D.ReflectY(point, 0));
                    break;
                case 5: // Reflection Origin
                    foreach (var point in sourcePoints)
                        transformedPoints.Add(Transform2D.ReflectAboutOrigin(point));
                    break;
                case 6: // Shear X
                    foreach (var point in sourcePoints)
                        transformedPoints.Add(Transform2D.ShearX(point, _viewModel.ShearFactor));
                    break;
                case 7: // Shear Y
                    foreach (var point in sourcePoints)
                        transformedPoints.Add(Transform2D.ShearY(point, _viewModel.ShearFactor));
                    break;
            }

            foreach (var point in transformedPoints)
                DrawPixel(_transformationCanvas, point.X, point.Y, _viewModel.TransformedColor);

            _transformedState = transformedPoints;
        }

        private Point CalculateCentroid(List<Point> points)
        {
            if (points.Count == 0) return new Point(0, 0);

            int sumX = 0, sumY = 0;
            foreach (var point in points)
            {
                sumX += point.X;
                sumY += point.Y;
            }

            return new Point(sumX / points.Count, sumY / points.Count);
        }
        
        private int CalculateDistance(Models.Point p1, Models.Point p2)
        {
            return (int)Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
        }

        private void ClearCanvas()
        {
            _canvas.Children.Clear();
            _transformationCanvas.Children.Clear();
            _currentPoints.Clear();
            _transformedState.Clear();
        }

        private void DrawCoordinateLabel(Canvas targetCanvas, int x, int y, double unitX, double unitY)
        {
            var textBlock = new TextBlock
            {
                Text = $"({unitX:F2}, {unitY:F2})",
                Foreground = Brushes.Black,
                FontSize = 12,
                Background = Brushes.White,
                Tag = "CoordLabel"
            };

            Canvas.SetLeft(textBlock, x + 5); // Slightly offset
            Canvas.SetTop(textBlock, y + 5);
            targetCanvas.Children.Add(textBlock);
        }
    }
}