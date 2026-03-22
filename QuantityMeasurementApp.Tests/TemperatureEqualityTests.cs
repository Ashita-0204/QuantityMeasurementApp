using NUnit.Framework;
using System;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Interfaces;
using QuantityMeasurementApp.Exceptions;
namespace QuantityMeasurementApp.Tests
{
    [TestFixture]
    public class TemperatureEqualityTests
    {
        private const double EPS = 0.001;

        [Test]
        public void testTemperatureEquality_CelsiusToCelsius_SameValue()
        {
            var a = new Quantity<TemperatureUnit>(0, TemperatureUnit.Celsius);
            var b = new Quantity<TemperatureUnit>(0, TemperatureUnit.Celsius);

            Assert.That(a.Equals(b), Is.True);
        }

            
        [Test]
        public void testTemperatureEquality_FahrenheitToFahrenheit_SameValue()
        {
            var a = new Quantity<TemperatureUnit>(32, TemperatureUnit.Fahrenheit);
            var b = new Quantity<TemperatureUnit>(32, TemperatureUnit.Fahrenheit);

            Assert.That(a.Equals(b), Is.True);
        }

  [Test]
        public void testTemperatureEquality_KelvinToKelvin_SameValue()
        {
            var a = new Quantity<TemperatureUnit>(273.15, TemperatureUnit.Kelvin);
            var b = new Quantity<TemperatureUnit>(273.15, TemperatureUnit.Kelvin);

            Assert.That(a.Equals(b), Is.True);
        }

        [Test]
        public void testTemperatureEquality_CelsiusToFahrenheit_0Celsius32Fahrenheit()
        {
            var a = new Quantity<TemperatureUnit>(0, TemperatureUnit.Celsius);
            var b = new Quantity<TemperatureUnit>(32, TemperatureUnit.Fahrenheit);

            Assert.That(a.Equals(b), Is.True);
        }

       [Test]
        public void testTemperatureEquality_CelsiusToKelvin()
        {
            var a = new Quantity<TemperatureUnit>(0, TemperatureUnit.Celsius);
            var b = new Quantity<TemperatureUnit>(273.15, TemperatureUnit.Kelvin);

            Assert.That(a.Equals(b), Is.True);
        }

        [Test]
        public void testTemperatureEquality_FahrenheitToKelvin()
        {
            var a = new Quantity<TemperatureUnit>(32, TemperatureUnit.Fahrenheit);
            var b = new Quantity<TemperatureUnit>(273.15, TemperatureUnit.Kelvin);

            Assert.That(a.Equals(b), Is.True);
        }

        [Test]
        public void testTemperatureEquality_SymmetricProperty()
        {
            var a = new Quantity<TemperatureUnit>(0, TemperatureUnit.Celsius);
            var b = new Quantity<TemperatureUnit>(32, TemperatureUnit.Fahrenheit);

            Assert.That(a.Equals(b) && b.Equals(a), Is.True);
        }

        [Test]
        public void testTemperatureEquality_ReflexiveProperty()
        {
            var a = new Quantity<TemperatureUnit>(100, TemperatureUnit.Celsius);

            Assert.That(a.Equals(a), Is.True);
        }

        [Test]
        public void testTemperatureEquality_TransitiveProperty()
        {
            var a = new Quantity<TemperatureUnit>(0, TemperatureUnit.Celsius);
            var b = new Quantity<TemperatureUnit>(32, TemperatureUnit.Fahrenheit);
            var c = new Quantity<TemperatureUnit>(273.15, TemperatureUnit.Kelvin);

            Assert.That(a.Equals(b) && b.Equals(c) && a.Equals(c), Is.True);
        }

        [Test]
        public void testTemperatureConversion_CelsiusToFahrenheit()
        {
            var a = new Quantity<TemperatureUnit>(100, TemperatureUnit.Celsius);

            double result = a.ConvertTo(TemperatureUnit.Fahrenheit);

            Assert.That(result, Is.EqualTo(212).Within(EPS));
        }
      
        [Test]
        public void testTemperatureConversion_FahrenheitToCelsius()
        {
            var a = new Quantity<TemperatureUnit>(212, TemperatureUnit.Fahrenheit);

            double result = a.ConvertTo(TemperatureUnit.Celsius);

            Assert.That(result, Is.EqualTo(100).Within(EPS));
        }

        [Test]
        public void testTemperatureConversion_CelsiusToKelvin()
        {
            var a = new Quantity<TemperatureUnit>(0, TemperatureUnit.Celsius);

            double result = a.ConvertTo(TemperatureUnit.Kelvin);

            Assert.That(result, Is.EqualTo(273.15).Within(EPS));
        }

        [Test]
        public void testTemperatureConversion_KelvinToCelsius()
        {
            var a = new Quantity<TemperatureUnit>(273.15, TemperatureUnit.Kelvin);

            double result = a.ConvertTo(TemperatureUnit.Celsius);

            Assert.That(result, Is.EqualTo(0).Within(EPS));
        }

        [Test]
        public void testTemperatureConversion_ZeroValue()
        {
            var a = new Quantity<TemperatureUnit>(0, TemperatureUnit.Celsius);

            double result = a.ConvertTo(TemperatureUnit.Fahrenheit);

            Assert.That(result, Is.EqualTo(32).Within(EPS));
        }

        [Test]
        public void testTemperatureConversion_Negative40Equal()
        {
            var a = new Quantity<TemperatureUnit>(-40, TemperatureUnit.Celsius);
            var b = new Quantity<TemperatureUnit>(-40, TemperatureUnit.Fahrenheit);

            Assert.That(a.Equals(b), Is.True);
        }

        [Test]
        public void testTemperatureConversion_RoundTrip()
        {
            var a = new Quantity<TemperatureUnit>(50, TemperatureUnit.Celsius);

            double f = a.ConvertTo(TemperatureUnit.Fahrenheit);

            var b = new Quantity<TemperatureUnit>(f, TemperatureUnit.Fahrenheit);

            double c = b.ConvertTo(TemperatureUnit.Celsius);

            Assert.That(c, Is.EqualTo(50).Within(EPS));
        }

        [Test]
        public void testTemperatureConversion_SameUnit()
        {
            var a = new Quantity<TemperatureUnit>(10, TemperatureUnit.Celsius);

            double result = a.ConvertTo(TemperatureUnit.Celsius);

            Assert.That(result, Is.EqualTo(10).Within(EPS));
        }

        [Test]
        public void testTemperatureConversion_LargeValues()
        {
            var a = new Quantity<TemperatureUnit>(1000, TemperatureUnit.Celsius);

            double result = a.ConvertTo(TemperatureUnit.Fahrenheit);

            Assert.That(result, Is.EqualTo(1832).Within(EPS));
        }

        [Test]
        public void testTemperatureAbsoluteZero()
        {
            var a = new Quantity<TemperatureUnit>(-273.15, TemperatureUnit.Celsius);
            var b = new Quantity<TemperatureUnit>(0, TemperatureUnit.Kelvin);

            Assert.That(a.Equals(b), Is.True);
        }

        [Test]
        public void testTemperatureUnsupportedOperation_Add()
        {
            var a = new Quantity<TemperatureUnit>(100, TemperatureUnit.Celsius);
            var b = new Quantity<TemperatureUnit>(50, TemperatureUnit.Celsius);

            Assert.Throws<QuantityMeasurementException>(() => a.Add(b));
        }

        [Test]
        public void testTemperatureUnsupportedOperation_Subtract()
        {
            var a = new Quantity<TemperatureUnit>(100, TemperatureUnit.Celsius);
            var b = new Quantity<TemperatureUnit>(50, TemperatureUnit.Celsius);

            Assert.Throws<QuantityMeasurementException>(() => a.Subtract(b));
        }

        [Test]
        public void testTemperatureUnsupportedOperation_Divide()
        {
            var a = new Quantity<TemperatureUnit>(100, TemperatureUnit.Celsius);
            var b = new Quantity<TemperatureUnit>(50, TemperatureUnit.Celsius);

            Assert.Throws<QuantityMeasurementException>(() => a.Divide(b));
        }

        [Test]
        public void testTemperatureDifferentValuesInequality()
        {
            var a = new Quantity<TemperatureUnit>(50, TemperatureUnit.Celsius);
            var b = new Quantity<TemperatureUnit>(100, TemperatureUnit.Celsius);

            Assert.That(a.Equals(b), Is.False);
        }

        [Test]
        public void testTemperatureNullOperandValidation()
        {
            var a = new Quantity<TemperatureUnit>(10, TemperatureUnit.Celsius);

            Assert.That(a.Equals(null), Is.False);
        }

       [Test]
        public void testTemperatureVsLengthIncompatibility()
        {
            var a = new Quantity<TemperatureUnit>(100, TemperatureUnit.Celsius);
            var b = new Quantity<LengthUnit>(100, LengthUnit.Feet);

            Assert.That(a.Equals(b), Is.False);
        }
 [Test]
        public void testTemperatureVsWeightIncompatibility()
        {
            var a = new Quantity<TemperatureUnit>(50, TemperatureUnit.Celsius);
            var b = new Quantity<WeightUnit>(50, WeightUnit.Kilogram);

            Assert.That(a.Equals(b), Is.False);
        }

        [Test]
        public void testTemperatureVsVolumeIncompatibility()
        {
            var a = new Quantity<TemperatureUnit>(25, TemperatureUnit.Celsius);
            var b = new Quantity<VolumeUnit>(25, VolumeUnit.Litre);

            Assert.That(a.Equals(b), Is.False);
        }

        [Test]
        public void testTemperatureUnit_AllConstants()
        {
            Assert.That(Enum.IsDefined(typeof(TemperatureUnit), "Celsius"));
            Assert.That(Enum.IsDefined(typeof(TemperatureUnit), "Fahrenheit"));
            Assert.That(Enum.IsDefined(typeof(TemperatureUnit), "Kelvin"));
        }

        [Test]
        public void testTemperatureUnit_ImplementsIMeasurable()
        {
            Assert.That(TemperatureUnit.Celsius.GetUnitName(), Is.EqualTo("Celsius"));
            Assert.That(TemperatureUnit.Celsius.ConvertToBaseUnit(100), Is.EqualTo(100));
        }

       [Test]
        public void testTemperatureConversionPrecision()
        {
            var a = new Quantity<TemperatureUnit>(1, TemperatureUnit.Celsius);

            double result = a.ConvertTo(TemperatureUnit.Fahrenheit);

            Assert.That(result, Is.EqualTo(33.8).Within(EPS));
        }

        [Test]
        public void testTemperatureVerySmallDifference()
        {
            var a = new Quantity<TemperatureUnit>(0.0001, TemperatureUnit.Celsius);
            var b = new Quantity<TemperatureUnit>(0.0001, TemperatureUnit.Celsius);

            Assert.That(a.Equals(b), Is.True);
        }

        [Test]
        public void testTemperatureCrossUnitAdditionAttempt()
        {
            var a = new Quantity<TemperatureUnit>(10, TemperatureUnit.Celsius);
            var b = new Quantity<TemperatureUnit>(50, TemperatureUnit.Fahrenheit);

            Assert.Throws<QuantityMeasurementException>(() => a.Add(b));
        }


        [Test]
        public void testTemperatureOperationSupportFalse()
        {
            Assert.That(TemperatureUnit.Celsius.SupportsArithmetic(), Is.False);
        }

        [Test]
        public void testLengthUnitOperationSupportTrue()
        {
            Assert.That(LengthUnit.Feet.SupportsArithmetic(), Is.True);
        }

       [Test]
        public void testWeightUnitOperationSupportTrue()
        {
            Assert.That(WeightUnit.Kilogram.SupportsArithmetic(), Is.True);
        }

        [Test]
        public void testTemperatureEnumToString()
        {
            Assert.That(TemperatureUnit.Celsius.ToString(), Is.EqualTo("Celsius"));
        }

        [Test]
        public void testTemperatureGenericIntegration()
        {
            var q = new Quantity<TemperatureUnit>(10, TemperatureUnit.Celsius);

            Assert.That(q.Value, Is.EqualTo(10));
        }

        [Test]
        public void testTemperatureConvertKelvinToFahrenheit()
        {
            var a = new Quantity<TemperatureUnit>(273.15, TemperatureUnit.Kelvin);

            double result = a.ConvertTo(TemperatureUnit.Fahrenheit);

            Assert.That(result, Is.EqualTo(32).Within(EPS));
        }

        [Test]
        public void testTemperatureConvertFahrenheitToKelvin()
        {
            var a = new Quantity<TemperatureUnit>(32, TemperatureUnit.Fahrenheit);

            double result = a.ConvertTo(TemperatureUnit.Kelvin);

            Assert.That(result, Is.EqualTo(273.15).Within(EPS));
        }

        [Test]
        public void testTemperatureHashCodeConsistency()
        {
            var a = new Quantity<TemperatureUnit>(0, TemperatureUnit.Celsius);
            var b = new Quantity<TemperatureUnit>(32, TemperatureUnit.Fahrenheit);

            Assert.That(a.GetHashCode(), Is.EqualTo(b.GetHashCode()));
        }

        [Test]
        public void testTemperatureIntegrationWithGenericQuantity()
        {
            var q = new Quantity<TemperatureUnit>(100, TemperatureUnit.Celsius);

            double f = q.ConvertTo(TemperatureUnit.Fahrenheit);

            Assert.That(f, Is.EqualTo(212).Within(EPS));
        }
    }
}