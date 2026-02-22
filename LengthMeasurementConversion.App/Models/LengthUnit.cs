namespace LengthMeasurementConversion.App.Models
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
        //UC5 Implementation
        // Conversion factor relative to FEET (base unit)

        public static double GetFactor(this LengthUnit unit)
        {
            switch (unit)
            {
                case LengthUnit.Feet:
                    return 1.0;

                case LengthUnit.Inches:
                    return 1.0 / 12.0;

                case LengthUnit.Yards:
                    return 3.0;

                case LengthUnit.Centimeters:
                    // 1 cm = 0.393701 inch
                    // inch -> feet
                    return 0.393701 / 12.0;

                default:
                    throw new Exception("Invalid Unit");
            }
        }
    }
}
