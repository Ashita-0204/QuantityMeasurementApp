using NUnit.Framework;
using QuantityMeasurementApp.Services;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Exceptions;

namespace QuantityMeasurementApp.Tests
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
        public void testFeetEquality_SameValue()
        {
            bool result = service.AreEqual(1.0, 1.0);
            Assert.That(result, Is.True);
        }

        // Different Value
        [Test]
        public void testFeetEquality_DifferentValue()
        {
            bool result = service.AreEqual(1.0, 2.0);
            Assert.That(result, Is.False);
        }

        // Null Comparison
        [Test]
        public void testFeetEquality_NullComparison()
        {
            Feet feet = new Feet(1.0);
            Assert.That(feet.Equals(null), Is.False);
        }

        // Same Reference
        [Test]
        public void testFeetEquality_SameReference()
        {
            Feet feet = new Feet(1.0);

            Assert.That(feet.Equals(feet), Is.True);
        }

        // Different Object Type
        [Test]
        public void testFeetEquality_DifferentClass()
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