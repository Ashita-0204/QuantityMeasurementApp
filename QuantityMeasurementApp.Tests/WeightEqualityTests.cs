using NUnit.Framework;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
    [TestFixture]
    public class WeightEqualityTests
    {
        private const double EPSILON = 0.0001;

        // ---------------- Equality Tests ----------------

        [Test]
        public void testEquality_KilogramToKilogram_SameValue()
        {
            var w1 = new Weight(1.0, WeightUnit.Kilogram);
            var w2 = new Weight(1.0, WeightUnit.Kilogram);

            Assert.That(w1.Equals(w2), Is.True);
        }

        [Test]
        public void testEquality_KilogramToKilogram_DifferentValue()
        {
            var w1 = new Weight(1.0, WeightUnit.Kilogram);
            var w2 = new Weight(2.0, WeightUnit.Kilogram);

            Assert.That(w1.Equals(w2), Is.False);
        }

        [Test]
        public void testEquality_KilogramToGram_EquivalentValue()
        {
            var w1 = new Weight(1.0, WeightUnit.Kilogram);
            var w2 = new Weight(1000.0, WeightUnit.Gram);

            Assert.That(w1.Equals(w2), Is.True);
        }

        [Test]
        public void testEquality_GramToKilogram_EquivalentValue()
        {
            var w1 = new Weight(1000.0, WeightUnit.Gram);
            var w2 = new Weight(1.0, WeightUnit.Kilogram);

            Assert.That(w1.Equals(w2), Is.True);
        }

        [Test]
        public void testEquality_WeightVsLength_Incompatible()
        {
            var weight = new Weight(1.0, WeightUnit.Kilogram);
            var length = new Length(1.0, LengthUnit.Feet);

            Assert.That(weight.Equals(length), Is.False);
        }

        [Test]
        public void testEquality_NullComparison()
        {
            var weight = new Weight(1.0, WeightUnit.Kilogram);

            Assert.That(weight.Equals(null), Is.False);
        }

        [Test]
        public void testEquality_SameReference()
        {
            var weight = new Weight(1.0, WeightUnit.Kilogram);

            Assert.That(weight.Equals(weight), Is.True);
        }

        [Test]
        public void testEquality_NullUnit()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new Weight(1.0, (WeightUnit)(-1));
            });
        }
        [Test]
        public void testEquality_TransitiveProperty()
        {
            var a = new Weight(1.0, WeightUnit.Kilogram);
            var b = new Weight(1000.0, WeightUnit.Gram);
            var c = new Weight(1.0, WeightUnit.Kilogram);

            Assert.That(a.Equals(b), Is.True);
            Assert.That(b.Equals(c), Is.True);
            Assert.That(a.Equals(c), Is.True);
        }

        [Test]
        public void testEquality_ZeroValue()
        {
            var w1 = new Weight(0.0, WeightUnit.Kilogram);
            var w2 = new Weight(0.0, WeightUnit.Gram);

            Assert.That(w1.Equals(w2), Is.True);
        }

        [Test]
        public void testEquality_NegativeWeight()
        {
            var w1 = new Weight(-1.0, WeightUnit.Kilogram);
            var w2 = new Weight(-1000.0, WeightUnit.Gram);

            Assert.That(w1.Equals(w2), Is.True);
        }

        [Test]
        public void testEquality_LargeWeightValue()
        {
            var w1 = new Weight(1000000.0, WeightUnit.Gram);
            var w2 = new Weight(1000.0, WeightUnit.Kilogram);

            Assert.That(w1.Equals(w2), Is.True);
        }

        [Test]
        public void testEquality_SmallWeightValue()
        {
            var w1 = new Weight(0.001, WeightUnit.Kilogram);
            var w2 = new Weight(1.0, WeightUnit.Gram);

            Assert.That(w1.Equals(w2), Is.True);
        }

        // ---------------- Conversion Tests ----------------

        [Test]
        public void testConversion_PoundToKilogram()
        {
            var result = new Weight(2.20462, WeightUnit.Pound)
                .ConvertTo(WeightUnit.Kilogram);

            Assert.That(result.Value, Is.EqualTo(1.0).Within(EPSILON));
        }

        [Test]
        public void testConversion_KilogramToPound()
        {
            var result = new Weight(1.0, WeightUnit.Kilogram)
                .ConvertTo(WeightUnit.Pound);

            Assert.That(result.Value, Is.EqualTo(2.20462).Within(EPSILON));
        }

        [Test]
        public void testConversion_SameUnit()
        {
            var result = new Weight(5.0, WeightUnit.Kilogram)
                .ConvertTo(WeightUnit.Kilogram);

            Assert.That(result.Value, Is.EqualTo(5.0));
        }

        [Test]
        public void testConversion_ZeroValue()
        {
            var result = new Weight(0.0, WeightUnit.Kilogram)
                .ConvertTo(WeightUnit.Gram);

            Assert.That(result.Value, Is.EqualTo(0.0));
        }

        [Test]
        public void testConversion_NegativeValue()
        {
            var result = new Weight(-1.0, WeightUnit.Kilogram)
                .ConvertTo(WeightUnit.Gram);

            Assert.That(result.Value, Is.EqualTo(-1000.0));
        }

        [Test]
        public void testConversion_RoundTrip()
        {
            var result = new Weight(1.5, WeightUnit.Kilogram)
                .ConvertTo(WeightUnit.Gram)
                .ConvertTo(WeightUnit.Kilogram);

            Assert.That(result.Value, Is.EqualTo(1.5).Within(EPSILON));
        }

        // ---------------- Addition Tests ----------------

        [Test]
        public void testAddition_SameUnit_KilogramPlusKilogram()
        {
            var result = new Weight(1.0, WeightUnit.Kilogram)
                .Add(new Weight(2.0, WeightUnit.Kilogram));

            Assert.That(result.Value, Is.EqualTo(3.0));
        }

        [Test]
        public void testAddition_CrossUnit_KilogramPlusGram()
        {
            var result = new Weight(1.0, WeightUnit.Kilogram)
                .Add(new Weight(1000.0, WeightUnit.Gram));

            Assert.That(result.Value, Is.EqualTo(2.0));
        }

        [Test]
        public void testAddition_CrossUnit_PoundPlusKilogram()
        {
            var result = new Weight(2.20462, WeightUnit.Pound)
                .Add(new Weight(1.0, WeightUnit.Kilogram));

            Assert.That(result.Value, Is.EqualTo(4.40924).Within(EPSILON));
        }

        [Test]
        public void testAddition_ExplicitTargetUnit_Kilogram()
        {
            var result = new Weight(1.0, WeightUnit.Kilogram)
                .Add(new Weight(1000.0, WeightUnit.Gram), WeightUnit.Gram);

            Assert.That(result.Value, Is.EqualTo(2000.0));
        }

        [Test]
        public void testAddition_Commutativity()
        {
            var a = new Weight(1.0, WeightUnit.Kilogram)
                .Add(new Weight(1000.0, WeightUnit.Gram));

            var b = new Weight(1000.0, WeightUnit.Gram)
                .Add(new Weight(1.0, WeightUnit.Kilogram));

            Assert.That(a.Value, Is.EqualTo(b.ConvertTo(a.Unit).Value).Within(EPSILON));
        }

        [Test]
        public void testAddition_WithZero()
        {
            var result = new Weight(5.0, WeightUnit.Kilogram)
                .Add(new Weight(0.0, WeightUnit.Gram));

            Assert.That(result.Value, Is.EqualTo(5.0));
        }

        [Test]
        public void testAddition_NegativeValues()
        {
            var result = new Weight(5.0, WeightUnit.Kilogram)
                .Add(new Weight(-2000.0, WeightUnit.Gram));

            Assert.That(result.Value, Is.EqualTo(3.0));
        }

        [Test]
        public void testAddition_LargeValues()
        {
            var result = new Weight(1e6, WeightUnit.Kilogram)
                .Add(new Weight(1e6, WeightUnit.Kilogram));

            Assert.That(result.Value, Is.EqualTo(2e6));
        }
    }
}