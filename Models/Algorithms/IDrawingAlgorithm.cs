using System.Collections.Generic;

namespace GraphicsAlgorithmsApp.Models.Algorithms
{
    public interface IDrawingAlgorithm
    {
    }

    public interface ILineAlgorithm : IDrawingAlgorithm
    {
        List<Point> CalculatePoints(Point start, Point end);
    }

    public interface ICircleAlgorithm : IDrawingAlgorithm
    {
        List<Point> CalculatePoints(Point center, int radius);
    }

        public interface IGenericDrawingAlgorithm : IDrawingAlgorithm
    {
        IEnumerable<Point> Execute(Point start, Point end);
    }
}