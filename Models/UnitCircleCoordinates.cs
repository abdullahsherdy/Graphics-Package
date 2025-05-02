using System;

namespace GraphicsAlgorithmsApp.Models
{
    public static class UnitCircleCoordinates
    {
        public static (double, double) ScreenToUnitCircle(double screenX, double screenY, double centerX, double centerY, double scale)
        {
            double unitX = (screenX - centerX) / scale;
            double unitY = -(screenY - centerY) / scale;
            
            return (unitX, unitY);
        }
        
        public static (double, double) UnitCircleToScreen(double unitX, double unitY, double centerX, double centerY, double scale)
        {
            double screenX = (centerX + unitX * scale);
            double screenY = (centerY - unitY * scale);
            
            return (screenX, screenY);
        }
    }
}