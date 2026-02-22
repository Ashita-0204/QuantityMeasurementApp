using FeetAndInchesMeasurementEquality.Models;

namespace FeetAndInchesMeasurementEquality.Services
{
    public class QuantityMeasurementService
    {
        // UC1 -> Feet Equality (Already Exists)
        public bool AreEqual(double value1, double value2)
        {
            Feet f1 = new Feet(value1);
            Feet f2 = new Feet(value2);

            return f1.Equals(f2);
        }

        // UC2 -> Inches Equality Method
        // Separate method reduces dependency on Main
        public bool AreInchesEqual(double value1, double value2)
        {
            Inches i1 = new Inches(value1);
            Inches i2 = new Inches(value2);

            return i1.Equals(i2);
        }

        //UC3 Implementation
        public bool AreLengthEqual(double value1, LengthUnit unit1, double value2, LengthUnit unit2)
        {
            QuantityLength q1 = new QuantityLength(value1, unit1);

            QuantityLength q2 = new QuantityLength(value2, unit2);

            return q1.Equals(q2);
        }
    }
}