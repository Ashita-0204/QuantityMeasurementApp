using NUnit.Framework;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
    [TestFixture]
    public class LengthEqualityTests
    {
        private Length feetOne;
        private Length feetTwo;

        private Length inchOne;
        private Length inchTwo;

        // Runs before every test
        [SetUp]
        public void Setup()
        {
            feetOne = new Length(1.0, LengthUnit.Feet);
            feetTwo = new Length(1.0, LengthUnit.Feet);

            inchOne =
                new Length(1.0, LengthUnit.Inches);

            inchTwo =
                new Length(1.0, LengthUnit.Inches);
        }

        // Feet Equality
        [Test]
        public void testFeetEquality()
        {
            Assert.That(feetOne.Equals(feetTwo), Is.True);
        }

        // Inches Equality
        [Test]
        public void testInchesEquality()
        {
            Assert.That(inchOne.Equals(inchTwo), Is.True);
        }

        // Feet vs Inches Equality
        [Test]
        public void testFeetInchesComparison()
        {
            var inches = new Length(12.0, LengthUnit.Inches);
            Assert.That(feetOne.Equals(inches), Is.True);
        }

        // Feet Inequality
        [Test]
        public void testFeetInequality()
        {
            var feetDifferent = new Length(2.0, LengthUnit.Feet);
            Assert.That(feetOne.Equals(feetDifferent), Is.False);
        }

        // Inches Inequality
        [Test]
        public void testInchesInequality()
        {
            var inchDifferent = new Length(2.0, LengthUnit.Inches);
            Assert.That(inchOne.Equals(inchDifferent), Is.False);
        }

        // Cross Unit Inequality
        [Test]
        public void testCrossUnitInequality()
        {
            var inches = new Length(10.0, LengthUnit.Inches);
            Assert.That(feetOne.Equals(inches), Is.False);
        }

        // Transitive Property
        [Test]
        public void testMultipleFeetComparison()
        {
            var feet3 = new Length(1.0, LengthUnit.Feet);
            Assert.That(feetOne.Equals(feetTwo), Is.True);
            Assert.That(feetTwo.Equals(feet3), Is.True);
            Assert.That(feetOne.Equals(feet3), Is.True);
        }

        // Null Safety
        [Test]
        public void TestNullComparison()
        {
            Assert.That(feetOne.Equals(null), Is.False);
        }

        // Exception Validation
        [Test]
        public void TestInvalidValue()
        {
            Assert.Throws<ArgumentException>(() =>
             {
                 new Length(-1, LengthUnit.Feet);
             });
        }
    }
}