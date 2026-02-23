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

        // For Same Value
        [Test]
        public void testInchesEquality_SameValue()
        {
            bool result = service.AreInchesEqual(1.0, 1.0);
            Assert.That(result, Is.True);
        }

        // For Different Value
        [Test]
        public void testInchesEquality_DifferentValue()
        {
            bool result = service.AreInchesEqual(1.0, 2.0);
            Assert.That(result, Is.False);
        }

        //For Null Comparison
        [Test]
        public void testInchesEquality_NullComparison()
        {
            Inches inches = new Inches(1.0);
            Assert.That(inches.Equals(null), Is.False);
        }

        //For Different Class
        [Test]
        public void testInchesEquality_DifferentClass()
        {
            Inches inches = new Inches(1.0);
            Assert.That(inches.Equals("ABC"), Is.False);
        }

        //For Same Reference
        [Test]
        public void testInchesEquality_SameReference()
        {
            Inches inches = new Inches(1.0);
            Assert.That(inches.Equals(inches), Is.True);
        }
    }
}