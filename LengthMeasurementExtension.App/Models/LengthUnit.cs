namespace LengthMeasurementExtension.App.Models
{
    // Supported Units
    public enum LengthUnit
    {
        Feet, Inches, Yards, Centimeters
    }

    // Conversion Helper
    public static class LengthUnitExtensions
    {
        // Convert everything to feet (Base)
        public static double GetConversionFactor(this LengthUnit unit)
        {
            switch (unit)
            {
                case LengthUnit.Feet:
                    return 1.0;
                case LengthUnit.Inches:
                    return 1.0 / 12.0;
                default:
                    throw new Exception("Unsupported Unit");
            }
        }
    }
}