using NUnit.Framework;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
    [TestFixture]
    public class QuantityMeasurementTargetAdd
    {
        private const double EPSILON = 0.01;

        [Test]
        public void testAddition_ExplicitTargetUnit_Feet()
        {
            Length l1 = new Length(1.0, Length.LengthUnit.Feet);
            Length l2 = new Length(12.0, Length.LengthUnit.Inches);

            Length result = l1.Add(l2, Length.LengthUnit.Feet);

            Assert.That(result.Value, Is.EqualTo(2.0).Within(EPSILON));
            Assert.That(result.Unit, Is.EqualTo(Length.LengthUnit.Feet));
        }

        [Test]
        public void testAddition_ExplicitTargetUnit_Inches()
        {
            Length l1 = new Length(1.0, Length.LengthUnit.Feet);
            Length l2 = new Length(12.0, Length.LengthUnit.Inches);

            Length result = l1.Add(l2, Length.LengthUnit.Inches);

            Assert.That(result.Value, Is.EqualTo(24.0).Within(EPSILON));
            Assert.That(result.Unit, Is.EqualTo(Length.LengthUnit.Inches));
        }

        [Test]
        public void testAddition_ExplicitTargetUnit_Yards()
        {
            Length l1 = new Length(1.0, Length.LengthUnit.Feet);
            Length l2 = new Length(12.0, Length.LengthUnit.Inches);

            Length result = l1.Add(l2, Length.LengthUnit.Yards);

            Assert.That(result.Value, Is.EqualTo(0.667).Within(EPSILON));
            Assert.That(result.Unit, Is.EqualTo(Length.LengthUnit.Yards));
        }

        [Test]
        public void testAddition_ExplicitTargetUnit_Centimeters()
        {
            Length l1 = new Length(1.0, Length.LengthUnit.Inches);
            Length l2 = new Length(1.0, Length.LengthUnit.Inches);

            Length result = l1.Add(l2, Length.LengthUnit.Centimeters);

            Assert.That(result.Value, Is.EqualTo(5.08).Within(EPSILON));
            Assert.That(result.Unit, Is.EqualTo(Length.LengthUnit.Centimeters));
        }

        [Test]
        public void testAddition_ExplicitTargetUnit_SameAsFirstOperand()
        {
            Length l1 = new Length(2.0, Length.LengthUnit.Yards);
            Length l2 = new Length(3.0, Length.LengthUnit.Feet);

            Length result = l1.Add(l2, Length.LengthUnit.Yards);

            Assert.That(result.Value, Is.EqualTo(3.0).Within(EPSILON));
        }

        [Test]
        public void testAddition_ExplicitTargetUnit_SameAsSecondOperand()
        {
            Length l1 = new Length(2.0, Length.LengthUnit.Yards);
            Length l2 = new Length(3.0, Length.LengthUnit.Feet);

            Length result = l1.Add(l2, Length.LengthUnit.Feet);

            Assert.That(result.Value, Is.EqualTo(9.0).Within(EPSILON));
        }

        [Test]
        public void testAddition_ExplicitTargetUnit_Commutativity()
        {
            Length l1 = new Length(1.0, Length.LengthUnit.Feet);
            Length l2 = new Length(12.0, Length.LengthUnit.Inches);

            Length result1 = l1.Add(l2, Length.LengthUnit.Yards);
            Length result2 = l2.Add(l1, Length.LengthUnit.Yards);

            Assert.That(result1.Value, Is.EqualTo(result2.Value).Within(EPSILON));
        }

        [Test]
        public void testAddition_ExplicitTargetUnit_WithZero()
        {
            Length l1 = new Length(5.0, Length.LengthUnit.Feet);
            Length l2 = new Length(0.0, Length.LengthUnit.Inches);

            Length result = l1.Add(l2, Length.LengthUnit.Yards);

            Assert.That(result.Value, Is.EqualTo(1.667).Within(EPSILON));
        }

        [Test]
        public void testAddition_ExplicitTargetUnit_NegativeValues()
        {
            Length l1 = new Length(5.0, Length.LengthUnit.Feet);

            Assert.Throws<ArgumentException>(() =>
            {
                Length l2 = new Length(-2.0, Length.LengthUnit.Feet);
            });
        }

        [Test]
        public void testAddition_ExplicitTargetUnit_NullTargetUnit()
        {
            Length l1 = new Length(1.0, Length.LengthUnit.Feet);
            Length l2 = new Length(12.0, Length.LengthUnit.Inches);

            Assert.Throws<System.ArgumentException>(() =>
            {
                l1.Add(l2, (Length.LengthUnit)(-1));
            });
        }

        [Test]
        public void testAddition_ExplicitTargetUnit_LargeToSmallScale()
        {
            Length l1 = new Length(1000.0, Length.LengthUnit.Feet);
            Length l2 = new Length(500.0, Length.LengthUnit.Feet);

            Length result = l1.Add(l2, Length.LengthUnit.Inches);

            Assert.That(result.Value, Is.EqualTo(18000.0).Within(EPSILON));
        }

        [Test]
        public void testAddition_ExplicitTargetUnit_SmallToLargeScale()
        {
            Length l1 = new Length(12.0, Length.LengthUnit.Inches);
            Length l2 = new Length(12.0, Length.LengthUnit.Inches);

            Length result = l1.Add(l2, Length.LengthUnit.Yards);

            Assert.That(result.Value, Is.EqualTo(0.667).Within(EPSILON));
        }

        [Test]
        public void testAddition_ExplicitTargetUnit_AllUnitCombinations()
        {
            Length feet = new Length(3.0, Length.LengthUnit.Feet);
            Length inches = new Length(36.0, Length.LengthUnit.Inches);

            Length result = feet.Add(inches, Length.LengthUnit.Feet);

            Assert.That(result.Value, Is.EqualTo(6.0).Within(EPSILON));
        }

        [Test]
        public void testAddition_ExplicitTargetUnit_PrecisionTolerance()
        {
            Length l1 = new Length(2.54, Length.LengthUnit.Centimeters);
            Length l2 = new Length(1.0, Length.LengthUnit.Inches);

            Length result = l1.Add(l2, Length.LengthUnit.Centimeters);

            Assert.That(result.Value, Is.EqualTo(5.08).Within(EPSILON));
        }
    }
}