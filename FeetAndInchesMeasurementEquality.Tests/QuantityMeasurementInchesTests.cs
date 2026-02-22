using NUnit.Framework;
using FeetAndInchesMeasurementEquality.Services;
using FeetAndInchesMeasurementEquality.Models;
using FeetAndInchesMeasurementEquality.Exceptions;

namespace FeetAndInchesMeasurementEquality.Tests
{
    // UC2 -> Inches Equality Test Cases
    public class QuantityMeasurementInchesTests
    {
        private QuantityMeasurementService service;

        [SetUp]
        public void Setup()
        {
            service = new QuantityMeasurementService();
        }

        // Same Value Equality
        [Test]
        public void GivenSameInchesValue_WhenCompared_ShouldReturnTrue()
        {
            bool result = service.AreInchesEqual(1.0, 1.0);

            Assert.That(result, Is.True);
        }

        // Different Value
        [Test]
        public void GivenDifferentInchesValue_WhenCompared_ShouldReturnFalse()
        {
            bool result = service.AreInchesEqual(1.0, 2.0);

            Assert.That(result, Is.False);
        }

        // Null Comparison
        [Test]
        public void GivenInches_WhenComparedWithNull_ShouldReturnFalse()
        {
            Inches inches = new Inches(1.0);

            Assert.That(inches.Equals(null), Is.False);
        }

        // Same Reference
        [Test]
        public void GivenSameReference_ShouldReturnTrue()
        {
            Inches inches = new Inches(1.0);

            Assert.That(inches.Equals(inches), Is.True);
        }

        // Different Object Type Comparison
        [Test]
        public void GivenInches_WhenComparedWithDifferentObject_ShouldReturnFalse()
        {
            Inches inches = new Inches(1.0);

            Assert.That(inches.Equals("ABC"), Is.False);
        }

        // Custom Exception -> NaN
        [Test]
        public void GivenInvalidInchesValue_ShouldThrowCustomException()
        {
            Assert.Throws<QuantityMeasurementException>(() =>
            {
                Inches inches = new Inches(double.NaN);
            });
        }

        // Infinity Exception
        [Test]
        public void GivenInfinityValue_ShouldThrowCustomException()
        {
            Assert.Throws<QuantityMeasurementException>(() =>
            {
                Inches inches = new Inches(double.PositiveInfinity);
            });
        }
    }
}