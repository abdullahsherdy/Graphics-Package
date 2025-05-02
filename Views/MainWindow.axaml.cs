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
        private double _canvasCenterX = 250;
        private double _canvasCenterY = 250;
        private double _unitScale = 100.0;
        private List<Point> _transformedState = new List<Point>();

        public MainWindow()
        {
            InitializeComponent();
            
            DataContext = _viewModel = new MainWindowViewModel();
            
            _canvas = this.FindControl<Canvas>("DrawingCanvas");
            _transformationCanvas = this.FindControl<Canvas>("TransformationCanvas");
            
            if (_canvas != null && _transformationCanvas != null)
            {
                _canvas.Width = 700;
                _canvas.Height = 700;
                _transformationCanvas.Width = 700;
                _transformationCanvas.Height = 700;
                
                _canvasCenterX = 350;
                _canvasCenterY = 350;

                _canvas.SizeChanged += (s, e) =>
                {
                    _canvasCenterX = (_canvas.Width / 2);
                    _canvasCenterY = (_canvas.Height / 2);
                    DrawCoordinateAxes();
                };
                
                _transformationCanvas.SizeChanged += (s, e) =>
                {
                    DrawCoordinateAxes(_transformationCanvas);
                };
                
                _viewModel.ClearAction = ClearCanvas;
                _viewModel.ApplyTransformationAction = ApplyTransformation;

                DrawCoordinateAxes();
                DrawCoordinateAxes(_transformationCanvas);
            }
            else
            {
                _viewModel.ClearAction = () => { };
                _viewModel.ApplyTransformationAction = () => { };
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void Canvas_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (_canvas == null) return;
            
            var position = e.GetPosition(_canvas);
            _startPoint = new Models.Point(position.X, position.Y);
            _isDrawing = true;
            
            var unitCoords = UnitCircleCoordinates.ScreenToUnitCircle(
                _startPoint.X, _startPoint.Y, _canvasCenterX, _canvasCenterY, _unitScale);
            _viewModel.UnitX = Math.Round(unitCoords.Item1, 2);
            _viewModel.UnitY = Math.Round(unitCoords.Item2, 2);
            
            if (_viewModel.IsCircleDrawing)
            {
                var endPoint = new Models.Point(_startPoint.X + 5, _startPoint.Y);
                if (_viewModel.ShowPreview)
                {
                    DrawPreview(_startPoint, endPoint);
                }
            }
        }

        private void Canvas_PointerMoved(object sender, PointerEventArgs e)
        {
            if (_canvas == null) return;
            
            var position = e.GetPosition(_canvas);
            var currentPoint = new Models.Point(position.X, position.Y);
            
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
            if (_canvas == null || !_isDrawing) return;
            
            var position = e.GetPosition(_canvas);
            var endPoint = new Models.Point(position.X, position.Y);
            
            DrawShape(_startPoint, endPoint);
            _isDrawing = false;
        }

        private void DrawPreview(Models.Point start, Models.Point end)
        {
            if (_canvas == null) return;
            
            List<Point> previewPoints = new List<Point>();
            
            switch (_viewModel.SelectedAlgorithmIndex)
            {
                case 0: // DDA Line
                    var ddaAlgorithm = new DDALineAlgorithm();
                    previewPoints.AddRange(ddaAlgorithm.CalculatePoints(start, end));
                    break;
                case 1: // Bresenham Line
                    var bresenhamLineAlgorithm = new BresenhamLineAlgorithm();
                    previewPoints.AddRange(bresenhamLineAlgorithm.CalculatePoints(start, end));
                    break;
                case 2: // Circle
                    var circleAlgorithm = new BresenhamCircleAlgorithm();
                    double radius = CalculateDistance(start, end);
                    previewPoints.AddRange(circleAlgorithm.CalculatePoints(start, radius));
                    break;
                case 3: // Ellipse
                    var ellipseAlgorithm = new EllipseAlgorithm();
                    double radiusX = Math.Abs(end.X - start.X);
                    double radiusY = Math.Abs(end.Y - start.Y);
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
            if (_canvas == null) return;
            
            _currentPoints.Clear();
            _transformedState.Clear();

            switch (_viewModel.SelectedAlgorithmIndex)
            {
                case 0: // DDA Line
                    var ddaAlgorithm = new DDALineAlgorithm();
                    _currentPoints.AddRange(ddaAlgorithm.CalculatePoints(start, end));
                    break;
                case 1: // Bresenham Line
                    var bresenhamLineAlgorithm = new BresenhamLineAlgorithm();
                    _currentPoints.AddRange(bresenhamLineAlgorithm.CalculatePoints(start, end));
                    break;
                case 2: // Circle
                    var circleAlgorithm = new BresenhamCircleAlgorithm();
                    double radius = CalculateDistance(start, end);
                    _currentPoints.AddRange(circleAlgorithm.CalculatePoints(start, radius));
                    break;
                case 3: // Ellipse
                    var ellipseAlgorithm = new EllipseAlgorithm();
                    double radiusX = Math.Abs(end.X - start.X);
                    double radiusY = Math.Abs(end.Y - start.Y);
                    _currentPoints.AddRange(ellipseAlgorithm.CalculatePoints(start, radiusX, radiusY));
                    break;
            }

            foreach (var point in _currentPoints)
                DrawPixel(_canvas, point.X, point.Y, _viewModel.SelectedColor);
        }  

        private void ClearPreview()
        {
            if (_canvas == null) return;
            
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

        private void DrawPixel(Canvas targetCanvas, double x, double y, Color color, bool isPreview = false)
        {
            if (targetCanvas == null) return;
            if (x < 0 || x >= targetCanvas.Width || y < 0 || y >= targetCanvas.Height) return;
            
            var ellipse = new Avalonia.Controls.Shapes.Ellipse
            {
                Width = 3,
                Height = 3,
                Fill = new SolidColorBrush(color)
            };
            
            if (isPreview)
            {
                ellipse.Tag = "Preview";
            }
            
            Canvas.SetLeft(ellipse, x - 1.5);
            Canvas.SetTop(ellipse, y - 1.5);
            targetCanvas.Children.Add(ellipse);
        }

        private void ApplyTransformation()
        {
            if (_transformationCanvas == null || _currentPoints.Count == 0) return;

            _transformationCanvas.Children.Clear();
            DrawCoordinateAxes(_transformationCanvas);
            
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
                        transformedPoints.Add(Transform2D.ReflectX(point, _canvasCenterY));
                    break;
                case 4: // Reflection Y-Axis
                    foreach (var point in sourcePoints)
                        transformedPoints.Add(Transform2D.ReflectY(point, _canvasCenterX));
                    break;
                case 5: // Reflection Origin
                    foreach (var point in sourcePoints)
                        transformedPoints.Add(Transform2D.ReflectAboutOrigin(point, _canvasCenterX, _canvasCenterY));
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

            double sumX = 0, sumY = 0;
            foreach (var point in points)
            {
                sumX += point.X;
                sumY += point.Y;
            }

            return new Point(sumX / points.Count, sumY / points.Count);
        }
        
        private double CalculateDistance(Models.Point p1, Models.Point p2)
        {
            return Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
        }

        private void ClearCanvas()
        {
            if (_canvas == null && _transformationCanvas == null) return;

            if (_canvas != null)
            {
                List<Control> toRemove = new List<Control>();
                foreach (var child in _canvas.Children)
                {
                    if (child is Control control && control.Tag?.ToString() != "CoordinateAxis")
                    {
                        toRemove.Add(control);
                    }
                }

                foreach (var control in toRemove)
                {
                    _canvas.Children.Remove(control);
                }
            }
            
            if (_transformationCanvas != null)
            {
                List<Control> toRemove = new List<Control>();
                foreach (var child in _transformationCanvas.Children)
                {
                    if (child is Control control && control.Tag?.ToString() != "CoordinateAxis")
                    {
                        toRemove.Add(control);
                    }
                }

                foreach (var control in toRemove)
                {
                    _transformationCanvas.Children.Remove(control);
                }
            }
            
            _currentPoints.Clear();
            _transformedState.Clear();
        }

        private void DrawCoordinateLabel(Canvas targetCanvas, int x, int y, double unitX, double unitY)
        {
            if (targetCanvas == null) return;
            
            var textBlock = new TextBlock
            {
                Text = $"({unitX:F2}, {unitY:F2})",
                Foreground = Brushes.Black,
                FontSize = 10,
                Background = Brushes.White,
                Tag = "CoordLabel"
            };

            Canvas.SetLeft(textBlock, x + 5);
            Canvas.SetTop(textBlock, y + 5);
            targetCanvas.Children.Add(textBlock);
        }

        private void DrawCoordinateAxes(Canvas targetCanvas = null)
        {
            if (targetCanvas == null) targetCanvas = _canvas;
            if (targetCanvas == null) return;
            
            List<Control> toRemove = new List<Control>();
            foreach (var child in targetCanvas.Children)
            {
                if (child is Control control && control.Tag?.ToString() == "CoordinateAxis")
                {
                    toRemove.Add(control);
                }
            }

            foreach (var control in toRemove)
            {
                targetCanvas.Children.Remove(control);
            }

            double centerX = (targetCanvas.Width / 2);
            double centerY = (targetCanvas.Height / 2);
            
            var xAxis = new Avalonia.Controls.Shapes.Line
            {
                StartPoint = new Avalonia.Point(0, centerY),
                EndPoint = new Avalonia.Point(targetCanvas.Width, centerY),
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Tag = "CoordinateAxis"
            };
            
            var yAxis = new Avalonia.Controls.Shapes.Line
            {
                StartPoint = new Avalonia.Point(centerX, 0),
                EndPoint = new Avalonia.Point(centerX, targetCanvas.Height),
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Tag = "CoordinateAxis"
            };
            
            var origin = new Avalonia.Controls.Shapes.Ellipse
            {
                Width = 6,
                Height = 6,
                Fill = Brushes.Red,
                Tag = "CoordinateAxis"
            };
            Canvas.SetLeft(origin, centerX - 3);
            Canvas.SetTop(origin, centerY - 3);
            
            targetCanvas.Children.Add(xAxis);
            targetCanvas.Children.Add(yAxis);
            targetCanvas.Children.Add(origin);
        }
    }
}