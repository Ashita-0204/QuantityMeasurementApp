using NUnit.Framework;
using QuantityMeasurementApp.Models;
using System;

namespace QuantityMeasurementApp.Tests
{
    public class VolumeQuantityTests
    {
        private const double EPS = 0.001;

        // ---------- Equality Tests ----------

        [Test]
        public void testEquality_LitreToLitre_SameValue()
        {
            var a = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            var b = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);

            Assert.That(a.Equals(b), Is.True);
        }

        [Test]
        public void testEquality_LitreToLitre_DifferentValue()
        {
            var a = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            var b = new Quantity<VolumeUnit>(2.0, VolumeUnit.Litre);

            Assert.That(a.Equals(b), Is.False);
        }

        [Test]
        public void testEquality_LitreToMillilitre_EquivalentValue()
        {
            var a = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            var b = new Quantity<VolumeUnit>(1000.0, VolumeUnit.Millilitre);

            Assert.That(a.Equals(b), Is.True);
        }

        [Test]
        public void testEquality_MillilitreToLitre_EquivalentValue()
        {
            var a = new Quantity<VolumeUnit>(1000.0, VolumeUnit.Millilitre);
            var b = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);

            Assert.That(a.Equals(b), Is.True);
        }

        [Test]
        public void testEquality_LitreToGallon_EquivalentValue()
        {
            var a = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            var b = new Quantity<VolumeUnit>(0.264172, VolumeUnit.Gallon);

            Assert.That(a.Equals(b), Is.True);
        }

        [Test]
        public void testEquality_GallonToLitre_EquivalentValue()
        {
            var a = new Quantity<VolumeUnit>(1.0, VolumeUnit.Gallon);
            var b = new Quantity<VolumeUnit>(3.78541, VolumeUnit.Litre);

            Assert.That(a.Equals(b), Is.True);
        }

        [Test]
        public void testEquality_VolumeVsLength_Incompatible()
        {
            var volume = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            var length = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);

            Assert.That(volume.Equals(length), Is.False);
        }

        [Test]
        public void testEquality_VolumeVsWeight_Incompatible()
        {
            var volume = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            var weight = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);

            Assert.That(volume.Equals(weight), Is.False);
        }

        [Test]
        public void testEquality_NullComparison()
        {
            var a = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);

            Assert.That(a.Equals(null), Is.False);
        }

        [Test]
        public void testEquality_SameReference()
        {
            var a = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);

            Assert.That(a.Equals(a), Is.True);
        }

        [Test]
        public void testEquality_TransitiveProperty()
        {
            var a = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            var b = new Quantity<VolumeUnit>(1000.0, VolumeUnit.Millilitre);
            var c = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);

            Assert.That(a.Equals(b));
            Assert.That(b.Equals(c));
            Assert.That(a.Equals(c));
        }

        [Test]
        public void testEquality_ZeroValue()
        {
            var a = new Quantity<VolumeUnit>(0.0, VolumeUnit.Litre);
            var b = new Quantity<VolumeUnit>(0.0, VolumeUnit.Millilitre);

            Assert.That(a.Equals(b), Is.True);
        }

        [Test]
        public void testEquality_NegativeVolume()
        {
            var a = new Quantity<VolumeUnit>(-1.0, VolumeUnit.Litre);
            var b = new Quantity<VolumeUnit>(-1000.0, VolumeUnit.Millilitre);

            Assert.That(a.Equals(b), Is.True);
        }

        [Test]
        public void testEquality_LargeVolumeValue()
        {
            var a = new Quantity<VolumeUnit>(1000000.0, VolumeUnit.Millilitre);
            var b = new Quantity<VolumeUnit>(1000.0, VolumeUnit.Litre);

            Assert.That(a.Equals(b), Is.True);
        }

        [Test]
        public void testEquality_SmallVolumeValue()
        {
            var a = new Quantity<VolumeUnit>(0.001, VolumeUnit.Litre);
            var b = new Quantity<VolumeUnit>(1.0, VolumeUnit.Millilitre);

            Assert.That(a.Equals(b), Is.True);
        }

        // ---------- Conversion ----------

        [Test]
        public void testConversion_LitreToMillilitre()
        {
            var q = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);

            Assert.That(q.ConvertTo(VolumeUnit.Millilitre), Is.EqualTo(1000).Within(EPS));
        }

        [Test]
        public void testConversion_MillilitreToLitre()
        {
            var q = new Quantity<VolumeUnit>(1000, VolumeUnit.Millilitre);

            Assert.That(q.ConvertTo(VolumeUnit.Litre), Is.EqualTo(1).Within(EPS));
        }

        [Test]
        public void testConversion_GallonToLitre()
        {
            var q = new Quantity<VolumeUnit>(1, VolumeUnit.Gallon);

            Assert.That(q.ConvertTo(VolumeUnit.Litre), Is.EqualTo(3.78541).Within(EPS));
        }

        [Test]
        public void testConversion_LitreToGallon()
        {
            var q = new Quantity<VolumeUnit>(3.78541, VolumeUnit.Litre);

            Assert.That(q.ConvertTo(VolumeUnit.Gallon), Is.EqualTo(1).Within(EPS));
        }

        [Test]
        public void testConversion_MillilitreToGallon()
        {
            var q = new Quantity<VolumeUnit>(1000, VolumeUnit.Millilitre);

            Assert.That(q.ConvertTo(VolumeUnit.Gallon), Is.EqualTo(0.264172).Within(EPS));
        }

        [Test]
        public void testConversion_SameUnit()
        {
            var q = new Quantity<VolumeUnit>(5, VolumeUnit.Litre);

            Assert.That(q.ConvertTo(VolumeUnit.Litre), Is.EqualTo(5));
        }

        [Test]
        public void testConversion_ZeroValue()
        {
            var q = new Quantity<VolumeUnit>(0, VolumeUnit.Litre);

            Assert.That(q.ConvertTo(VolumeUnit.Millilitre), Is.EqualTo(0));
        }

        [Test]
        public void testConversion_NegativeValue()
        {
            var q = new Quantity<VolumeUnit>(-1, VolumeUnit.Litre);

            Assert.That(q.ConvertTo(VolumeUnit.Millilitre), Is.EqualTo(-1000).Within(EPS));
        }

        [Test]
        public void testConversion_RoundTrip()
        {
            var q = new Quantity<VolumeUnit>(1.5, VolumeUnit.Litre);

            var result = new Quantity<VolumeUnit>(q.ConvertTo(VolumeUnit.Millilitre), VolumeUnit.Millilitre)
                .ConvertTo(VolumeUnit.Litre);

            Assert.That(result, Is.EqualTo(1.5).Within(EPS));
        }

        // ---------- Addition ----------

        [Test]
        public void testAddition_SameUnit_LitrePlusLitre()
        {
            var a = new Quantity<VolumeUnit>(1, VolumeUnit.Litre);
            var b = new Quantity<VolumeUnit>(2, VolumeUnit.Litre);

            var result = a.Add(b);

            Assert.That(result.Value, Is.EqualTo(3));
        }

        [Test]
        public void testAddition_SameUnit_MillilitrePlusMillilitre()
        {
            var a = new Quantity<VolumeUnit>(500, VolumeUnit.Millilitre);
            var b = new Quantity<VolumeUnit>(500, VolumeUnit.Millilitre);

            var result = a.Add(b);

            Assert.That(result.Value, Is.EqualTo(1000));
        }

        [Test]
        public void testAddition_CrossUnit_LitrePlusMillilitre()
        {
            var a = new Quantity<VolumeUnit>(1, VolumeUnit.Litre);
            var b = new Quantity<VolumeUnit>(1000, VolumeUnit.Millilitre);

            var result = a.Add(b);

            Assert.That(result.Value, Is.EqualTo(2).Within(EPS));
        }

        [Test]
        public void testAddition_CrossUnit_MillilitrePlusLitre()
        {
            var a = new Quantity<VolumeUnit>(1000, VolumeUnit.Millilitre);
            var b = new Quantity<VolumeUnit>(1, VolumeUnit.Litre);

            var result = a.Add(b);

            Assert.That(result.Value, Is.EqualTo(2000).Within(EPS));
        }

        [Test]
        public void testAddition_CrossUnit_GallonPlusLitre()
        {
            var a = new Quantity<VolumeUnit>(1, VolumeUnit.Gallon);
            var b = new Quantity<VolumeUnit>(3.78541, VolumeUnit.Litre);

            var result = a.Add(b);

            Assert.That(result.Value, Is.EqualTo(2).Within(EPS));
        }

        [Test]
        public void testAddition_ExplicitTargetUnit_Litre()
        {
            var a = new Quantity<VolumeUnit>(1, VolumeUnit.Litre);
            var b = new Quantity<VolumeUnit>(1000, VolumeUnit.Millilitre);

            var result = a.Add(b, VolumeUnit.Litre);

            Assert.That(result.Value, Is.EqualTo(2).Within(EPS));
        }

        [Test]
        public void testAddition_ExplicitTargetUnit_Millilitre()
        {
            var a = new Quantity<VolumeUnit>(1, VolumeUnit.Litre);
            var b = new Quantity<VolumeUnit>(1000, VolumeUnit.Millilitre);

            var result = a.Add(b, VolumeUnit.Millilitre);

            Assert.That(result.Value, Is.EqualTo(2000).Within(EPS));
        }

        [Test]
        public void testAddition_Commutativity()
        {
            var a = new Quantity<VolumeUnit>(1, VolumeUnit.Litre);
            var b = new Quantity<VolumeUnit>(1000, VolumeUnit.Millilitre);

            Assert.That(a.Add(b).Value, Is.EqualTo(b.Add(a).ConvertTo(VolumeUnit.Litre)).Within(EPS));
        }

        [Test]
        public void testAddition_WithZero()
        {
            var a = new Quantity<VolumeUnit>(5, VolumeUnit.Litre);
            var b = new Quantity<VolumeUnit>(0, VolumeUnit.Millilitre);

            var result = a.Add(b);

            Assert.That(result.Value, Is.EqualTo(5));
        }

        [Test]
        public void testAddition_NegativeValues()
        {
            var a = new Quantity<VolumeUnit>(5, VolumeUnit.Litre);
            var b = new Quantity<VolumeUnit>(-2000, VolumeUnit.Millilitre);

            var result = a.Add(b);

            Assert.That(result.Value, Is.EqualTo(3).Within(EPS));
        }

        [Test]
        public void testAddition_LargeValues()
        {
            var a = new Quantity<VolumeUnit>(1e6, VolumeUnit.Litre);
            var b = new Quantity<VolumeUnit>(1e6, VolumeUnit.Litre);

            var result = a.Add(b);

            Assert.That(result.Value, Is.EqualTo(2e6));
        }

        [Test]
        public void testAddition_SmallValues()
        {
            var a = new Quantity<VolumeUnit>(0.001, VolumeUnit.Litre);
            var b = new Quantity<VolumeUnit>(0.002, VolumeUnit.Litre);

            var result = a.Add(b);

            Assert.That(result.Value, Is.EqualTo(0.003).Within(EPS));
        }

        // ---------- Enum Tests ----------

        [Test]
        public void testVolumeUnitEnum_LitreConstant()
        {
            Assert.That(VolumeUnit.Litre.GetConversionFactor(), Is.EqualTo(1));
        }

        [Test]
        public void testVolumeUnitEnum_MillilitreConstant()
        {
            Assert.That(VolumeUnit.Millilitre.GetConversionFactor(), Is.EqualTo(0.001));
        }

        [Test]
        public void testVolumeUnitEnum_GallonConstant()
        {
            Assert.That(VolumeUnit.Gallon.GetConversionFactor(), Is.EqualTo(3.78541));
        }
    }
}