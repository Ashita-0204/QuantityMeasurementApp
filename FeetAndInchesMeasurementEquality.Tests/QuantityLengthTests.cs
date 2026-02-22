using NUnit.Framework;
using FeetAndInchesMeasurementEquality.Models;
using FeetAndInchesMeasurementEquality.Exceptions;

namespace FeetAndInchesMeasurementEquality.Tests
{
    // UC3 Test Cases--Generic Quantity Length
    public class QuantityLengthTests
    {

        // Feet-Feet Same Value
        [Test]
        public void testEquality_FeetToFeet_SameValue()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.Feet);
            var q2 = new QuantityLength(1.0, LengthUnit.Feet);
            Assert.That(q1.Equals(q2), Is.True);
        }

        // Inch-Inch Same Value
        [Test]
        public void testEquality_InchToInch_SameValue()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.Inches);
            var q2 = new QuantityLength(1.0, LengthUnit.Inches);
            Assert.That(q1.Equals(q2), Is.True);
        }

        // Cross Unit Equality
        // 12 inch = 1 feet
        [Test]
        public void testEquality_InchToFeet_EquivalentValue()
        {
            var q1 = new QuantityLength(12.0, LengthUnit.Inches);
            var q2 = new QuantityLength(1.0, LengthUnit.Feet);
            Assert.That(q1.Equals(q2), Is.True);
        }

        // Feet Different Value
        [Test]
        public void testEquality_FeetToFeet_DifferentValue()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.Feet);
            var q2 = new QuantityLength(2.0, LengthUnit.Feet);
            Assert.That(q1.Equals(q2), Is.False);
        }

        // Inch Different Value
        [Test]
        public void testEquality_InchToInch_DifferentValue()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.Inches);
            var q2 = new QuantityLength(2.0, LengthUnit.Inches);
            Assert.That(q1.Equals(q2), Is.False);
        }

        // Same Reference
        [Test]
        public void testEquality_SameReference()
        {
            var q = new QuantityLength(1.0, LengthUnit.Feet);
            Assert.That(q.Equals(q), Is.True);
        }

        // Null Comparison
        [Test]
        public void testEquality_NullComparison()
        {
            var q = new QuantityLength(1.0, LengthUnit.Feet);
            Assert.That(q.Equals(null), Is.False);
        }

        // Invalid Value -> Custom Exception
        [Test]
        public void testEquality_InvalidValue()
        {
            Assert.Throws<QuantityMeasurementException>(() =>
            {
                new QuantityLength(double.NaN, LengthUnit.Feet);
            });
        }

        // Infinity Exception
        [Test]
        public void testEquality_InfinityValue()
        {
            Assert.Throws<QuantityMeasurementException>(() =>
            {
                new QuantityLength(double.PositiveInfinity, LengthUnit.Inches);
            });
        }
    }
}