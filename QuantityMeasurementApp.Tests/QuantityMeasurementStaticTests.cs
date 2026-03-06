using NUnit.Framework;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.App;

namespace QuantityMeasurementApp.Tests;

[TestFixture]
public class QuantityMeasurementStaticTests
{
    // ---------------- EXISTING TESTS ----------------

    [Test]
    public void convertFeetToInches()
    {
        var result = QuantityMeasurementStatic.demonstrateLengthConversion(3, LengthUnit.Feet, LengthUnit.Inches);
        Assert.That(result.ToString(), Is.EqualTo("36.00 Inches"));
    }

    [Test]
    public void convertYardsToInchesUsingOverloadedMethod()
    {
        var yards = new Length(2, LengthUnit.Yards);
        var result = QuantityMeasurementStatic.demonstrateLengthConversion(yards, LengthUnit.Inches);
        Assert.That(result.ToString(), Is.EqualTo("72.00 Inches"));
    }

    // ---------------- BASIC CONVERSIONS ----------------

    [Test]
    public void testConversion_FeetToInches()
    {
        double result = Length.Convert(1.0, LengthUnit.Feet, LengthUnit.Inches);
        Assert.That(result, Is.EqualTo(12.0));
    }

    [Test]
    public void testConversion_InchesToFeet()
    {
        double result = Length.Convert(24.0, LengthUnit.Inches, LengthUnit.Feet);
        Assert.That(result, Is.EqualTo(2.0));
    }

    [Test]
    public void testConversion_YardsToInches()
    {
        double result = Length.Convert(1.0, LengthUnit.Yards, LengthUnit.Inches);
        Assert.That(result, Is.EqualTo(36.0));
    }

    [Test]
    public void testConversion_InchesToYards()
    {
        double result = Length.Convert(72.0, LengthUnit.Inches, LengthUnit.Yards);
        Assert.That(result, Is.EqualTo(2.0));
    }

    [Test]
    public void testConversion_CentimetersToInches()
    {
        double result = Length.Convert(2.54, LengthUnit.Centimeters, LengthUnit.Inches);
        Assert.That(result, Is.EqualTo(1.0).Within(0.01));
    }

    [Test]
    public void testConversion_FeatToYard()
    {
        double result = Length.Convert(6.0, LengthUnit.Feet, LengthUnit.Yards);
        Assert.That(result, Is.EqualTo(2.0));
    }

    // ---------------- EDGE CASES ----------------

    [Test]
    public void testConversion_ZeroValue()
    {
        double result = Length.Convert(0.0, LengthUnit.Feet, LengthUnit.Inches);
        Assert.That(result, Is.EqualTo(0.0));
    }

    [Test]
    public void testConversion_NegativeValue()
    {
        double result = Length.Convert(-1.0, LengthUnit.Feet, LengthUnit.Inches);
        Assert.That(result, Is.EqualTo(-12.0));
    }

    [Test]
    public void testConversion_SameUnit()
    {
        double result = Length.Convert(5.0, LengthUnit.Feet, LengthUnit.Feet);
        Assert.That(result, Is.EqualTo(5.0));
    }

    // ---------------- ROUND TRIP TEST ----------------

    [Test]
    public void testConversion_RoundTrip_PreservesValue()
    {
        double value = 10.0;
        double inches = Length.Convert(value, LengthUnit.Feet, LengthUnit.Inches);
        double result = Length.Convert(inches, LengthUnit.Inches, LengthUnit.Feet);
        Assert.That(result, Is.EqualTo(value).Within(0.0001));
    }

    // ---------------- PRECISION TEST ----------------

    [Test]
    public void testConversion_PrecisionTolerance()
    {
        double result = Length.Convert(2.54, LengthUnit.Centimeters, LengthUnit.Inches);
        Assert.That(result, Is.EqualTo(1.0).Within(0.001));
    }

    // ---------------- INVALID INPUT TESTS ----------------

    [Test]
    public void testConversion_NaNOrInfinite_Throws()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            Length.Convert(double.NaN, LengthUnit.Feet, LengthUnit.Inches);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            Length.Convert(double.PositiveInfinity, LengthUnit.Feet, LengthUnit.Inches);
        });
    }
}