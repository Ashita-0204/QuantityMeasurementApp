using NUnit.Framework;
using QuantityMeasurementApp.Models;
using System;

namespace QuantityMeasurementApp.Tests
{
    public class QuantityMeasurementSubAndDivisionTests
    {
        private const double EPS = 0.01;

        // ---------------- SAME UNIT SUBTRACTION ----------------

        [Test]
        public void testSubtraction_SameUnit_FeetMinusFeet()
        {
            var a = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(5, LengthUnit.Feet);

            var result = a.Subtract(b);

            Assert.That(result.Value, Is.EqualTo(5).Within(EPS));
        }

        [Test]
        public void testSubtraction_SameUnit_LitreMinusLitre()
        {
            var a = new Quantity<VolumeUnit>(10, VolumeUnit.Litre);
            var b = new Quantity<VolumeUnit>(3, VolumeUnit.Litre);

            var result = a.Subtract(b);

            Assert.That(result.Value, Is.EqualTo(7).Within(EPS));
        }

        // ---------------- CROSS UNIT SUBTRACTION ----------------

        [Test]
        public void testSubtraction_CrossUnit_FeetMinusInches()
        {
            var a = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(6, LengthUnit.Inches);

            var result = a.Subtract(b);

            Assert.That(result.Value, Is.EqualTo(9.5).Within(EPS));
        }

        [Test]
        public void testSubtraction_CrossUnit_InchesMinusFeet()
        {
            var a = new Quantity<LengthUnit>(120, LengthUnit.Inches);
            var b = new Quantity<LengthUnit>(5, LengthUnit.Feet);

            var result = a.Subtract(b);

            Assert.That(result.Value, Is.EqualTo(60).Within(EPS));
        }

        // ---------------- EXPLICIT TARGET UNIT ----------------

        [Test]
        public void testSubtraction_ExplicitTargetUnit_Feet()
        {
            var a = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(6, LengthUnit.Inches);

            var result = a.Subtract(b, LengthUnit.Feet);

            Assert.That(result.Value, Is.EqualTo(9.5).Within(EPS));
        }

        [Test]
        public void testSubtraction_ExplicitTargetUnit_Inches()
        {
            var a = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(6, LengthUnit.Inches);

            var result = a.Subtract(b, LengthUnit.Inches);

            Assert.That(result.Value, Is.EqualTo(114).Within(EPS));
        }

        [Test]
        public void testSubtraction_ExplicitTargetUnit_Millilitre()
        {
            var a = new Quantity<VolumeUnit>(5, VolumeUnit.Litre);
            var b = new Quantity<VolumeUnit>(2, VolumeUnit.Litre);

            var result = a.Subtract(b, VolumeUnit.Millilitre);

            Assert.That(result.Value, Is.EqualTo(3000).Within(EPS));
        }

        // ---------------- NEGATIVE / ZERO ----------------

        [Test]
        public void testSubtraction_ResultingInNegative()
        {
            var a = new Quantity<LengthUnit>(5, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(10, LengthUnit.Feet);

            var result = a.Subtract(b);

            Assert.That(result.Value, Is.EqualTo(-5).Within(EPS));
        }

        [Test]
        public void testSubtraction_ResultingInZero()
        {
            var a = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(120, LengthUnit.Inches);

            var result = a.Subtract(b);

            Assert.That(result.Value, Is.EqualTo(0).Within(EPS));
        }

        [Test]
        public void testSubtraction_WithZeroOperand()
        {
            var a = new Quantity<LengthUnit>(5, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(0, LengthUnit.Inches);

            var result = a.Subtract(b);

            Assert.That(result.Value, Is.EqualTo(5).Within(EPS));
        }

        [Test]
        public void testSubtraction_WithNegativeValues()
        {
            var a = new Quantity<LengthUnit>(5, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(-2, LengthUnit.Feet);

            var result = a.Subtract(b);

            Assert.That(result.Value, Is.EqualTo(7).Within(EPS));
        }

        // ---------------- NON COMMUTATIVE ----------------

        [Test]
        public void testSubtraction_NonCommutative()
        {
            var a = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(5, LengthUnit.Feet);

            var r1 = a.Subtract(b);
            var r2 = b.Subtract(a);

            Assert.That(r1.Value, Is.Not.EqualTo(r2.Value));
        }

        // ---------------- LARGE / SMALL ----------------

        [Test]
        public void testSubtraction_WithLargeValues()
        {
            var a = new Quantity<WeightUnit>(1e6, WeightUnit.Kilogram);
            var b = new Quantity<WeightUnit>(5e5, WeightUnit.Kilogram);

            var result = a.Subtract(b);

            Assert.That(result.Value, Is.EqualTo(5e5).Within(EPS));
        }

        [Test]
        public void testSubtraction_WithSmallValues()
        {
            var a = new Quantity<LengthUnit>(0.001, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(0.0005, LengthUnit.Feet);

            var result = a.Subtract(b);

            Assert.That(result.Value, Is.EqualTo(0.0005).Within(EPS));
        }

        // ---------------- NULL / ERROR ----------------

        [Test]
        public void testSubtraction_NullOperand()
        {
            var a = new Quantity<LengthUnit>(10, LengthUnit.Feet);

            Assert.Throws<ArgumentException>(() => a.Subtract(null));
        }

        [Test]
        public void testDivision_NullOperand()
        {
            var a = new Quantity<LengthUnit>(10, LengthUnit.Feet);

            Assert.Throws<ArgumentException>(() => a.Divide(null));
        }

        // ---------------- DIVISION ----------------

        [Test]
        public void testDivision_SameUnit_FeetDividedByFeet()
        {
            var a = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(2, LengthUnit.Feet);

            Assert.That(a.Divide(b), Is.EqualTo(5).Within(EPS));
        }

        [Test]
        public void testDivision_SameUnit_LitreDividedByLitre()
        {
            var a = new Quantity<VolumeUnit>(10, VolumeUnit.Litre);
            var b = new Quantity<VolumeUnit>(5, VolumeUnit.Litre);

            Assert.That(a.Divide(b), Is.EqualTo(2).Within(EPS));
        }

        [Test]
        public void testDivision_CrossUnit_FeetDividedByInches()
        {
            var a = new Quantity<LengthUnit>(24, LengthUnit.Inches);
            var b = new Quantity<LengthUnit>(2, LengthUnit.Feet);

            Assert.That(a.Divide(b), Is.EqualTo(1).Within(EPS));
        }

        [Test]
        public void testDivision_CrossUnit_KilogramDividedByGram()
        {
            var a = new Quantity<WeightUnit>(2, WeightUnit.Kilogram);
            var b = new Quantity<WeightUnit>(2000, WeightUnit.Gram);

            Assert.That(a.Divide(b), Is.EqualTo(1).Within(EPS));
        }

        // ---------------- RATIO CASES ----------------

        [Test]
        public void testDivision_RatioGreaterThanOne()
        {
            var a = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(2, LengthUnit.Feet);

            Assert.That(a.Divide(b), Is.GreaterThan(1));
        }

        [Test]
        public void testDivision_RatioLessThanOne()
        {
            var a = new Quantity<LengthUnit>(5, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(10, LengthUnit.Feet);

            Assert.That(a.Divide(b), Is.LessThan(1));
        }

        [Test]
        public void testDivision_RatioEqualToOne()
        {
            var a = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(10, LengthUnit.Feet);

            Assert.That(a.Divide(b), Is.EqualTo(1).Within(EPS));
        }

        // ---------------- NON COMMUTATIVE DIVISION ----------------

        [Test]
        public void testDivision_NonCommutative()
        {
            var a = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(5, LengthUnit.Feet);

            Assert.That(a.Divide(b), Is.Not.EqualTo(b.Divide(a)));
        }

        // ---------------- DIVISION BY ZERO ----------------

        [Test]
        public void testDivision_ByZero()
        {
            var a = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(0, LengthUnit.Feet);

            Assert.Throws<ArithmeticException>(() => a.Divide(b));
        }

        // ---------------- LARGE / SMALL RATIOS ----------------

        [Test]
        public void testDivision_WithLargeRatio()
        {
            var a = new Quantity<WeightUnit>(1e6, WeightUnit.Kilogram);
            var b = new Quantity<WeightUnit>(1, WeightUnit.Kilogram);

            Assert.That(a.Divide(b), Is.EqualTo(1e6).Within(EPS));
        }

        [Test]
        public void testDivision_WithSmallRatio()
        {
            var a = new Quantity<WeightUnit>(1, WeightUnit.Kilogram);
            var b = new Quantity<WeightUnit>(1e6, WeightUnit.Kilogram);

            Assert.That(a.Divide(b), Is.EqualTo(1e-6).Within(EPS));
        }

        // ---------------- IMMUTABILITY ----------------

        [Test]
        public void testSubtraction_Immutability()
        {
            var a = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(5, LengthUnit.Feet);

            var result = a.Subtract(b);

            Assert.That(a.Value, Is.EqualTo(10));
            Assert.That(b.Value, Is.EqualTo(5));
        }

        [Test]
        public void testDivision_Immutability()
        {
            var a = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(5, LengthUnit.Feet);

            var result = a.Divide(b);

            Assert.That(a.Value, Is.EqualTo(10));
            Assert.That(b.Value, Is.EqualTo(5));
        }

        // ---------------- INTEGRATION ----------------

        [Test]
        public void testSubtractionAddition_Inverse()
        {
            var a = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(5, LengthUnit.Feet);

            var result = a.Add(b).Subtract(b);

            Assert.That(result.Value, Is.EqualTo(10).Within(EPS));
        }

        [Test]
        public void testSubtraction_ChainedOperations()
        {
            var a = new Quantity<LengthUnit>(10, LengthUnit.Feet);

            var result = a
                .Subtract(new Quantity<LengthUnit>(2, LengthUnit.Feet))
                .Subtract(new Quantity<LengthUnit>(1, LengthUnit.Feet));

            Assert.That(result.Value, Is.EqualTo(7).Within(EPS));
        }
    }
}