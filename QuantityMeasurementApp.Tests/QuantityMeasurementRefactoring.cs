using NUnit.Framework;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
    [TestFixture]
    public class QuantityMeasurementRefactoring
    {
        private const double EPSILON = 0.01;

        // ---------- ENUM CONSTANT TESTS ----------

        [Test]
        public void testLengthUnitEnum_FeetConstant()
        {
            double result = LengthUnit.Feet.ConvertToBaseUnit(1.0);
            Assert.That(result, Is.EqualTo(12.0).Within(EPSILON));
        }

        [Test]
        public void testLengthUnitEnum_InchesConstant()
        {
            double result = LengthUnit.Inches.ConvertToBaseUnit(1.0);
            Assert.That(result, Is.EqualTo(1.0).Within(EPSILON));
        }

        [Test]
        public void testLengthUnitEnum_YardsConstant()
        {
            double result = LengthUnit.Yards.ConvertToBaseUnit(1.0);
            Assert.That(result, Is.EqualTo(36.0).Within(EPSILON));
        }

        [Test]
        public void testLengthUnitEnum_CentimetersConstant()
        {
            double result = LengthUnit.Centimeters.ConvertToBaseUnit(1.0);
            Assert.That(result, Is.EqualTo(0.3937).Within(EPSILON));
        }

        // ---------- CONVERT TO BASE UNIT ----------

        [Test]
        public void testConvertToBaseUnit_FeetToFeet()
        {
            double result = LengthUnit.Feet.ConvertToBaseUnit(5.0);
            Assert.That(result, Is.EqualTo(60.0).Within(EPSILON));
        }

        [Test]
        public void testConvertToBaseUnit_InchesToFeet()
        {
            double result = LengthUnit.Inches.ConvertToBaseUnit(12.0);
            Assert.That(result, Is.EqualTo(12.0).Within(EPSILON));
        }

        [Test]
        public void testConvertToBaseUnit_YardsToFeet()
        {
            double result = LengthUnit.Yards.ConvertToBaseUnit(1.0);
            Assert.That(result, Is.EqualTo(36.0).Within(EPSILON));
        }

        [Test]
        public void testConvertToBaseUnit_CentimetersToFeet()
        {
            double result = LengthUnit.Centimeters.ConvertToBaseUnit(30.48);
            Assert.That(result, Is.EqualTo(12.0).Within(EPSILON));
        }

        // ---------- CONVERT FROM BASE UNIT ----------

        [Test]
        public void testConvertFromBaseUnit_FeetToFeet()
        {
            double result = LengthUnit.Feet.ConvertFromBaseUnit(24.0);
            Assert.That(result, Is.EqualTo(2.0).Within(EPSILON));
        }

        [Test]
        public void testConvertFromBaseUnit_FeetToInches()
        {
            double result = LengthUnit.Inches.ConvertFromBaseUnit(12.0);
            Assert.That(result, Is.EqualTo(12.0).Within(EPSILON));
        }

        [Test]
        public void testConvertFromBaseUnit_FeetToYards()
        {
            double result = LengthUnit.Yards.ConvertFromBaseUnit(36.0);
            Assert.That(result, Is.EqualTo(1.0).Within(EPSILON));
        }

        [Test]
        public void testConvertFromBaseUnit_FeetToCentimeters()
        {
            double result = LengthUnit.Centimeters.ConvertFromBaseUnit(12.0);
            Assert.That(result, Is.EqualTo(30.48).Within(EPSILON));
        }

        // ---------- REFACTORED LENGTH CLASS ----------

        [Test]
        public void testQuantityLengthRefactored_Equality()
        {
            Length l1 = new Length(1.0, LengthUnit.Feet);
            Length l2 = new Length(12.0, LengthUnit.Inches);

            Assert.That(l1.Equals(l2), Is.True);
        }

        [Test]
        public void testQuantityLengthRefactored_ConvertTo()
        {
            Length length = new Length(1.0, LengthUnit.Feet);

            Length result = length.ConvertTo(LengthUnit.Inches);

            Assert.That(result.Value, Is.EqualTo(12.0).Within(EPSILON));
        }

        [Test]
        public void testQuantityLengthRefactored_Add()
        {
            Length l1 = new Length(1.0, LengthUnit.Feet);
            Length l2 = new Length(12.0, LengthUnit.Inches);

            Length result = l1.Add(l2, LengthUnit.Feet);

            Assert.That(result.Value, Is.EqualTo(2.0).Within(EPSILON));
        }

        [Test]
        public void testQuantityLengthRefactored_AddWithTargetUnit()
        {
            Length l1 = new Length(1.0, LengthUnit.Feet);
            Length l2 = new Length(12.0, LengthUnit.Inches);

            Length result = l1.Add(l2, LengthUnit.Yards);

            Assert.That(result.Value, Is.EqualTo(0.667).Within(EPSILON));
        }

        [Test]
        public void testQuantityLengthRefactored_NullUnit()
        {
            Assert.Throws<System.ArgumentException>(() =>
            {
                Length l = new Length(1.0, (LengthUnit)(-1));
            });
        }

        [Test]
        public void testQuantityLengthRefactored_InvalidValue()
        {
            Assert.Throws<System.ArgumentException>(() =>
            {
                Length l = new Length(double.NaN, LengthUnit.Feet);
            });
        }

        // ---------- BACKWARD COMPATIBILITY ----------

        [Test]
        public void testBackwardCompatibility_UC1EqualityTests()
        {
            Length l1 = new Length(1, LengthUnit.Feet);
            Length l2 = new Length(1, LengthUnit.Feet);

            Assert.That(l1.Equals(l2), Is.True);
        }

        [Test]
        public void testBackwardCompatibility_UC5ConversionTests()
        {
            Length l = new Length(1, LengthUnit.Feet);

            Length result = l.ConvertTo(LengthUnit.Inches);

            Assert.That(result.Value, Is.EqualTo(12.0).Within(EPSILON));
        }

        [Test]
        public void testBackwardCompatibility_UC6AdditionTests()
        {
            Length l1 = new Length(1, LengthUnit.Feet);
            Length l2 = new Length(12, LengthUnit.Inches);

            Length result = l1.Add(l2);

            Assert.That(result.Value, Is.EqualTo(2.0).Within(EPSILON));
        }

        [Test]
        public void testBackwardCompatibility_UC7AdditionWithTargetUnitTests()
        {
            Length l1 = new Length(1, LengthUnit.Feet);
            Length l2 = new Length(12, LengthUnit.Inches);

            Length result = l1.Add(l2, LengthUnit.Yards);

            Assert.That(result.Value, Is.EqualTo(0.667).Within(EPSILON));
        }

        // ---------- ARCHITECTURE TESTS ----------

        [Test]
        public void testArchitecturalScalability_MultipleCategories()
        {
            Assert.That(typeof(LengthUnit).IsEnum, Is.True);
        }

        [Test]
        public void testRoundTripConversion_RefactoredDesign()
        {
            double original = 5.0;

            double inches = LengthUnit.Feet.ConvertToBaseUnit(original);
            double back = LengthUnit.Feet.ConvertFromBaseUnit(inches);

            Assert.That(back, Is.EqualTo(original).Within(EPSILON));
        }

        [Test]
        public void testUnitImmutability()
        {
            Assert.That(LengthUnit.Feet.ToString(), Is.EqualTo("Feet"));
        }
    }
}