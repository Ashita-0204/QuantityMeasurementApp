using NUnit.Framework;
using LengthMeasurementConversion.App.Models;
using LengthMeasurementConversion.App.Exceptions;

namespace LengthMeasurementConversion.Tests;

public class QuantityConversionTests
{

    [Test]
    public void FeetToInches()
    {
        Assert.That(QuantityLength.Convert(1, LengthUnit.Feet, LengthUnit.Inches), Is.EqualTo(12));
    }

    [Test]
    public void InchesToFeet()
    {
        Assert.That(QuantityLength.Convert(24, LengthUnit.Inches, LengthUnit.Feet), Is.EqualTo(2));
    }

    [Test]
    public void YardToFeet()
    {
        Assert.That(QuantityLength.Convert(1, LengthUnit.Yards, LengthUnit.Feet), Is.EqualTo(3));
    }

    [Test]
    public void CMToInches()
    {
        Assert.That(QuantityLength.Convert(2.54, LengthUnit.Centimeters, LengthUnit.Inches), Is.EqualTo(1).Within(0.001));
    }

    [Test]
    public void RoundTrip()
    {
        double v = 5;
        double x = QuantityLength.Convert(v, LengthUnit.Feet, LengthUnit.Inches);
        double y = QuantityLength.Convert(x, LengthUnit.Inches, LengthUnit.Feet);
        Assert.That(y, Is.EqualTo(v).Within(0.0001));
    }

    [Test]
    public void InvalidThrows()
    {
        Assert.Throws<QuantityMeasurementException>(
        () => QuantityLength.Convert(double.NaN, LengthUnit.Feet, LengthUnit.Inches));
    }
}