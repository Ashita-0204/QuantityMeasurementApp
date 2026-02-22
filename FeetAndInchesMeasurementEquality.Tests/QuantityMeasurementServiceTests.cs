using NUnit.Framework;
using FeetAndInchesMeasurementEquality.Services;
using FeetAndInchesMeasurementEquality.Models;
using FeetAndInchesMeasurementEquality.Exceptions;

namespace FeetAndInchesMeasurementEquality.Tests
{
    public class QuantityMeasurementServiceTests
    {
        private QuantityMeasurementService service;

        [SetUp]
        public void Setup()
        {
            service = new QuantityMeasurementService();
        }

        // Same Value Equality
        [Test]
        public void GivenSameFeetValue_WhenCompared_ShouldReturnTrue()
        {
            bool result = service.AreEqual(1.0, 1.0);

            Assert.That(result, Is.True);
        }

        // Different Value
        [Test]
        public void GivenDifferentFeetValue_WhenCompared_ShouldReturnFalse()
        {
            bool result = service.AreEqual(1.0, 2.0);

            Assert.That(result, Is.False);
        }

        // Null Comparison
        [Test]
        public void GivenFeet_WhenComparedWithNull_ShouldReturnFalse()
        {
            Feet feet = new Feet(1.0);

            Assert.That(feet.Equals(null), Is.False);
        }

        // Same Reference
        [Test]
        public void GivenSameReference_ShouldReturnTrue()
        {
            Feet feet = new Feet(1.0);

            Assert.That(feet.Equals(feet), Is.True);
        }

        // Different Object Type
        [Test]
        public void GivenFeet_WhenComparedWithDifferentObject_ShouldReturnFalse()
        {
            Feet feet = new Feet(1.0);

            Assert.That(feet.Equals("ABC"), Is.False);
        }

        // Custom Exception
        [Test]
        public void GivenInvalidFeetValue_ShouldThrowCustomException()
        {
            Assert.Throws<QuantityMeasurementException>(() =>
            {
                Feet feet = new Feet(double.NaN);
            });
        }

        // Infinity Exception
        [Test]
        public void GivenInfinityValue_ShouldThrowCustomException()
        {
            Assert.Throws<QuantityMeasurementException>(() =>
            {
                Feet feet = new Feet(double.PositiveInfinity);
            });
        }
    }
}