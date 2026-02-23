using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Services
{
    public class QuantityMeasurementService
    {
        // UC1 -> Feet Equality
        public bool AreEqual(double value1, double value2)
        {
            Feet f1 = new Feet(value1);
            Feet f2 = new Feet(value2);

            return f1.Equals(f2);
        }

        // UC2 -> Inches Equality Method
        public bool AreInchesEqual(double value1, double value2)
        {
            Inches i1 = new Inches(value1);
            Inches i2 = new Inches(value2);

            return i1.Equals(i2);
        }
    }
}