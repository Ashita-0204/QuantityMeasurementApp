namespace QmaService.Services
{
    // ── Enums ─────────────────────────────────────────────────────────────────

    public enum LengthUnit    { Feet, Inches, Yards, Centimeters }
    public enum WeightUnit    { Kilogram, Gram, Pound }
    public enum VolumeUnit    { Litre, Millilitre, Gallon }
    public enum TemperatureUnit { Celsius, Fahrenheit, Kelvin }

    // ── Conversion helpers ────────────────────────────────────────────────────

    public static class UnitConversions
    {
        public static double LengthToBase(LengthUnit unit, double value) =>
            value * unit switch
            {
                LengthUnit.Feet        => 12.0,
                LengthUnit.Inches      => 1.0,
                LengthUnit.Yards       => 36.0,
                LengthUnit.Centimeters => 0.393701,
                _                      => throw new ArgumentException($"Invalid length unit: {unit}")
            };

        public static double LengthFromBase(LengthUnit unit, double baseValue) =>
            baseValue / unit switch
            {
                LengthUnit.Feet        => 12.0,
                LengthUnit.Inches      => 1.0,
                LengthUnit.Yards       => 36.0,
                LengthUnit.Centimeters => 0.393701,
                _                      => throw new ArgumentException($"Invalid length unit: {unit}")
            };

        public static double WeightToBase(WeightUnit unit, double value) =>
            value * unit switch
            {
                WeightUnit.Kilogram => 1.0,
                WeightUnit.Gram     => 0.001,
                WeightUnit.Pound    => 0.453592,
                _                   => throw new ArgumentException($"Invalid weight unit: {unit}")
            };

        public static double WeightFromBase(WeightUnit unit, double baseValue) =>
            baseValue / unit switch
            {
                WeightUnit.Kilogram => 1.0,
                WeightUnit.Gram     => 0.001,
                WeightUnit.Pound    => 0.453592,
                _                   => throw new ArgumentException($"Invalid weight unit: {unit}")
            };

        public static double VolumeToBase(VolumeUnit unit, double value) =>
            value * unit switch
            {
                VolumeUnit.Litre      => 1.0,
                VolumeUnit.Millilitre => 0.001,
                VolumeUnit.Gallon     => 3.78541,
                _                     => throw new ArgumentException($"Invalid volume unit: {unit}")
            };

        public static double VolumeFromBase(VolumeUnit unit, double baseValue) =>
            baseValue / unit switch
            {
                VolumeUnit.Litre      => 1.0,
                VolumeUnit.Millilitre => 0.001,
                VolumeUnit.Gallon     => 3.78541,
                _                     => throw new ArgumentException($"Invalid volume unit: {unit}")
            };

        public static double TemperatureToBase(TemperatureUnit unit, double value) =>
            unit switch
            {
                TemperatureUnit.Celsius    => value,
                TemperatureUnit.Fahrenheit => (value - 32) * 5.0 / 9.0,
                TemperatureUnit.Kelvin     => value - 273.15,
                _                          => throw new ArgumentException($"Invalid temperature unit: {unit}")
            };

        public static double TemperatureFromBase(TemperatureUnit unit, double baseValue) =>
            unit switch
            {
                TemperatureUnit.Celsius    => baseValue,
                TemperatureUnit.Fahrenheit => baseValue * 9.0 / 5.0 + 32,
                TemperatureUnit.Kelvin     => baseValue + 273.15,
                _                          => throw new ArgumentException($"Invalid temperature unit: {unit}")
            };
    }
}
