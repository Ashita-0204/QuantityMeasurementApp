using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Services
{
    public class QuantityMeasurementService
    {
        // UC1 -> Feet Equality
        public bool AreEqual(double valueOne, double valueTwo)
        {
            Feet feetOne = new Feet(valueOne);
            Feet feetTwo = new Feet(valueTwo);

            return feetOne.Equals(feetTwo);
        }

        // UC2 -> Inches Equality Method
        public bool AreInchesEqual(double valueOne, double valueTwo)
        {
            Inches inchOne = new Inches(valueOne);
            Inches inchTwo = new Inches(valueTwo);

            return inchOne.Equals(inchTwo);
        }

        //UC3-Implementation of Generic Quantity
        public bool AreEqual(Length lenOne, Length lenTwo)
        {
            return lenOne.Equals(lenTwo);
        }
    }
}