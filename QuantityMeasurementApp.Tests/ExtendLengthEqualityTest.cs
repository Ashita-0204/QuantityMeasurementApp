using NUnit.Framework;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
    [TestFixture]
    public class ExtendLengthEqualityTest
    {

        [Test]
        public void yardEquals36Inches()
        {
            var yard = new Length(1, Length.LengthUnit.Yards);
            var inches = new Length(36, Length.LengthUnit.Inches);

            Assert.That(yard.Equals(inches), Is.True);
        }

        [Test]
        public void centimeterEquals39Point3701Inches()
        {
            var cm = new Length(100, Length.LengthUnit.Centimeters);
            var inches = new Length(39.3701, Length.LengthUnit.Inches);
            Assert.That(cm.Equals(inches), Is.True);
        }

        [Test]
        public void threeFeetEqualsOneYard()
        {
            var feet = new Length(3, Length.LengthUnit.Feet);
            var yard = new Length(1, Length.LengthUnit.Yards);
            Assert.That(feet.Equals(yard), Is.True);
        }

        [Test]
        public void thirtyPoint48CmEqualsOneFoot()
        {
            var cm = new Length(30.48, Length.LengthUnit.Centimeters);
            var foot = new Length(1, Length.LengthUnit.Feet);
            Assert.That(cm.Equals(foot), Is.True);
        }

        [Test]
        public void yardNotEqualToInches()
        {
            var yard = new Length(1, Length.LengthUnit.Yards);
            var inch = new Length(20, Length.LengthUnit.Inches);
            Assert.That(yard.Equals(inch), Is.False);
        }

        [Test]
        public void referenceEqualitySameObject()
        {
            var yard = new Length(1, Length.LengthUnit.Yards);
            Assert.That(yard.Equals(yard), Is.True);
        }

        [Test]
        public void equalsReturnsFalseForNull()
        {
            var yard = new Length(1, Length.LengthUnit.Yards);
            Assert.That(yard.Equals(null), Is.False);
        }

        [Test]
        public void reflexiveSymmetricAndTransitiveProperty()
        {
            var yard = new Length(1, Length.LengthUnit.Yards);
            var feet = new Length(3, Length.LengthUnit.Feet);
            var inches = new Length(36, Length.LengthUnit.Inches);
            Assert.That(yard.Equals(feet), Is.True);
            Assert.That(feet.Equals(inches), Is.True);
            Assert.That(yard.Equals(inches), Is.True);
        }

    }
}