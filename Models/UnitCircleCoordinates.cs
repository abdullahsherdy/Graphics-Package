using System;

namespace GraphicsAlgorithmsApp.Models
{
    public static class UnitCircleCoordinates
    {
        public static (double, double) ScreenToUnitCircle(int screenX, int screenY, int centerX, int centerY, double scale)
        {
            double unitX = (screenX - centerX) / scale;
            double unitY = -(screenY - centerY) / scale;
            
            return (unitX, unitY);
        }
        
        public static (int, int) UnitCircleToScreen(double unitX, double unitY, int centerX, int centerY, double scale)
        {
            int screenX = (int)(centerX + unitX * scale);
            int screenY = (int)(centerY - unitY * scale);
            
            return (screenX, screenY);
        }
    }
}