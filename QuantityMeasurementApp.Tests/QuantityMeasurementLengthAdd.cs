using NUnit.Framework;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.App;
using System;

namespace QuantityMeasurementApp.Tests
{
    [TestFixture]
    public class QuantityMeasurementLengthAdd
    {

        [Test]
        public void testAddition_SameUnit_FeetPlusFeet()
        {
            Length result = QuantityMeasurementStatic.demonstrateLengthAddition(1.0, Length.LengthUnit.Feet, 2.0, Length.LengthUnit.Feet);
            Length expected = new Length(3.0, Length.LengthUnit.Feet);
            Assert.That(result.Equals(expected), Is.True);
        }

        [Test]
        public void testAddition_SameUnit_InchPlusInch()
        {
            Length result = QuantityMeasurementStatic.demonstrateLengthAddition(6.0, Length.LengthUnit.Inches, 6.0, Length.LengthUnit.Inches);
            Length expected = new Length(12.0, Length.LengthUnit.Inches);
            Assert.That(result.Equals(expected), Is.True);
        }

        [Test]
        public void testAddition_CrossUnit_FeetPlusInches()
        {
            Length result = QuantityMeasurementStatic.demonstrateLengthAddition(1.0, Length.LengthUnit.Feet, 12.0, Length.LengthUnit.Inches);
            Length expected = new Length(2.0, Length.LengthUnit.Feet);
            Assert.That(result.Equals(expected), Is.True);
        }

        [Test]
        public void testAddition_CrossUnit_InchPlusFeet()
        {
            Length result = QuantityMeasurementStatic.demonstrateLengthAddition(12.0, Length.LengthUnit.Inches, 1.0, Length.LengthUnit.Feet);
            Length expected = new Length(24.0, Length.LengthUnit.Inches);
            Assert.That(result.Equals(expected), Is.True);
        }

        [Test]
        public void testAddition_CrossUnit_YardPlusFeet()
        {
            Length result = QuantityMeasurementStatic.demonstrateLengthAddition(1.0, Length.LengthUnit.Yards, 3.0, Length.LengthUnit.Feet);
            Length expected = new Length(2.0, Length.LengthUnit.Yards);
            Assert.That(result.Equals(expected), Is.True);
        }

        [Test]
        public void testAddition_CrossUnit_CentimeterPlusInch()
        {
            Length result = QuantityMeasurementStatic.demonstrateLengthAddition(2.54, Length.LengthUnit.Centimeters, 1.0, Length.LengthUnit.Inches);
            Length expected = new Length(5.08, Length.LengthUnit.Centimeters);
            Assert.That(result.ConvertTo(Length.LengthUnit.Centimeters).Equals(expected), Is.True);
        }

        [Test]
        public void testAddition_Commutativity()
        {
            Length result1 = QuantityMeasurementStatic.demonstrateLengthAddition(1.0, Length.LengthUnit.Feet, 12.0, Length.LengthUnit.Inches);
            Length result2 = QuantityMeasurementStatic.demonstrateLengthAddition(12.0, Length.LengthUnit.Inches, 1.0, Length.LengthUnit.Feet);
            Assert.That(result1.ConvertTo(Length.LengthUnit.Inches).Equals(result2), Is.True);
        }

        [Test]
        public void testAddition_WithZero()
        {
            Length result = QuantityMeasurementStatic.demonstrateLengthAddition(5.0, Length.LengthUnit.Feet, 0.0, Length.LengthUnit.Inches);
            Length expected = new Length(5.0, Length.LengthUnit.Feet);
            Assert.That(result.Equals(expected), Is.True);
        }

        [Test]
        public void testAddition_NullSecondOperand()
        {
            Length l1 = new Length(1.0, Length.LengthUnit.Feet);
            Assert.Throws<ArgumentException>(() => l1.Add(null));
        }

        [Test]
        public void testAddition_LargeValues()
        {
            Length result = QuantityMeasurementStatic.demonstrateLengthAddition(1e6, Length.LengthUnit.Feet, 1e6, Length.LengthUnit.Feet);
            Length expected = new Length(2e6, Length.LengthUnit.Feet);
            Assert.That(result.Equals(expected), Is.True);
        }

        [Test]
        public void testAddition_SmallValues()
        {
            Length result = QuantityMeasurementStatic.demonstrateLengthAddition(0.001, Length.LengthUnit.Feet, 0.002, Length.LengthUnit.Feet);
            double expected = 0.003;
            Assert.That(result.Value, Is.EqualTo(expected).Within(0.000001));
            Assert.That(result.Unit, Is.EqualTo(Length.LengthUnit.Feet));
        }
        [Test]
        public void testAddition_FeetPlusYard()
        {
            Length result =
            QuantityMeasurementStatic.demonstrateLengthAddition(3.0, Length.LengthUnit.Feet, 1.0, Length.LengthUnit.Yards);
            Length expected = new Length(6.0, Length.LengthUnit.Feet);
            Assert.That(result.Equals(expected), Is.True);
        }
    }
}
