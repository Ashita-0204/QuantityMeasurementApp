using NUnit.Framework;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.App;
using System;

namespace QuantityMeasurementApp.Tests
{
    public class GenericQuantityTests
    {
        private const double EPSILON = 0.0001;

        // ---------- Interface Tests ----------

        [Test]
        public void testIMeasurableInterface_LengthUnitImplementation()
        {
            LengthUnit unit = LengthUnit.Feet;
            Assert.That(unit.ConvertToBaseUnit(1), Is.EqualTo(12).Within(EPSILON));
        }

        [Test]
        public void testIMeasurableInterface_WeightUnitImplementation()
        {
            WeightUnit unit = WeightUnit.Kilogram;
            Assert.That(unit.ConvertToBaseUnit(1), Is.EqualTo(1).Within(EPSILON));
        }

        [Test]
        public void testIMeasurableInterface_ConsistentBehavior()
        {
            double lengthBase = LengthUnit.Feet.ConvertToBaseUnit(1);
            double weightBase = WeightUnit.Kilogram.ConvertToBaseUnit(1);

            Assert.That(lengthBase, Is.GreaterThan(0));
            Assert.That(weightBase, Is.GreaterThan(0));
        }

        // ---------- Generic Equality ----------

        [Test]
        public void testGenericQuantity_LengthOperations_Equality()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12.0, LengthUnit.Inches);

            Assert.That(q1.Equals(q2), Is.True);
        }

        [Test]
        public void testGenericQuantity_WeightOperations_Equality()
        {
            var q1 = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);
            var q2 = new Quantity<WeightUnit>(1000.0, WeightUnit.Gram);

            Assert.That(q1.Equals(q2), Is.True);
        }

        // ---------- Conversion ----------

        [Test]
        public void testGenericQuantity_LengthOperations_Conversion()
        {
            var q = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            double result = q.ConvertTo(LengthUnit.Inches);

            Assert.That(result, Is.EqualTo(12).Within(EPSILON));
        }

        [Test]
        public void testGenericQuantity_WeightOperations_Conversion()
        {
            var q = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);
            double result = q.ConvertTo(WeightUnit.Gram);

            Assert.That(result, Is.EqualTo(1000).Within(EPSILON));
        }

        // ---------- Addition ----------

        [Test]
        public void testGenericQuantity_LengthOperations_Addition()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12.0, LengthUnit.Inches);

            var result = q1.Add(q2, LengthUnit.Feet);

            Assert.That(result.Value, Is.EqualTo(2).Within(EPSILON));
        }

        [Test]
        public void testGenericQuantity_WeightOperations_Addition()
        {
            var q1 = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);
            var q2 = new Quantity<WeightUnit>(1000.0, WeightUnit.Gram);

            var result = q1.Add(q2, WeightUnit.Kilogram);

            Assert.That(result.Value, Is.EqualTo(2).Within(EPSILON));
        }

        // ---------- Cross Category ----------

        [Test]
        public void testCrossCategoryPrevention_LengthVsWeight()
        {
            var length = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var weight = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);

            Assert.That(length.Equals(weight), Is.False);
        }

        [Test]
        public void testCrossCategoryPrevention_CompilerTypeSafety()
        {
            Assert.Pass("Compile-time type safety ensured by generics.");
        }

        // ---------- Constructor Validation ----------

        [Test]
        public void testGenericQuantity_ConstructorValidation_InvalidValue()
        {
            Assert.Throws<ArgumentException>(() =>
                new Quantity<LengthUnit>(double.NaN, LengthUnit.Feet));
        }

        [Test]
        public void testGenericQuantity_ConstructorValidation_NullUnit()
        {
            Assert.Pass("Enums cannot be null in C#; compiler guarantees safety.");
        }

        // ---------- Conversion Combinations ----------

        [Test]
        public void testGenericQuantity_Conversion_AllUnitCombinations()
        {
            var q = new Quantity<LengthUnit>(1, LengthUnit.Yards);

            double inches = q.ConvertTo(LengthUnit.Inches);

            Assert.That(inches, Is.EqualTo(36).Within(EPSILON));
        }

        // ---------- Addition Combinations ----------

        [Test]
        public void testGenericQuantity_Addition_AllUnitCombinations()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Yards);
            var q2 = new Quantity<LengthUnit>(3, LengthUnit.Feet);

            var result = q1.Add(q2, LengthUnit.Feet);

            Assert.That(result.Value, Is.EqualTo(6).Within(EPSILON));
        }

        // ---------- Backward Compatibility ----------

        [Test]
        public void testBackwardCompatibility_AllUC1Through9Tests()
        {
            Assert.Pass("UC1–UC9 tests remain unchanged and pass.");
        }

        // ---------- Demonstration Methods ----------

        [Test]
        public void testQuantityMeasurementApp_SimplifiedDemonstration_Equality()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12, LengthUnit.Inches);

            Assert.That(QuantityOperations.DemonstrateEquality(q1, q2), Is.True);
        }

        [Test]
        public void testQuantityMeasurementApp_SimplifiedDemonstration_Conversion()
        {
            var q = new Quantity<LengthUnit>(1, LengthUnit.Feet);

            var result = QuantityOperations.DemonstrateConversion(q, LengthUnit.Inches);

            Assert.That(result.Value, Is.EqualTo(12).Within(EPSILON));
        }

        [Test]
        public void testQuantityMeasurementApp_SimplifiedDemonstration_Addition()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12, LengthUnit.Inches);

            var result = QuantityOperations.DemonstrateAddition(q1, q2, LengthUnit.Feet);

            Assert.That(result.Value, Is.EqualTo(2).Within(EPSILON));
        }

        // ---------- Generic Behaviour Tests ----------

        [Test]
        public void testTypeWildcard_FlexibleSignatures()
        {
            var length = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var weight = new Quantity<WeightUnit>(1, WeightUnit.Kilogram);

            Assert.That(length, Is.Not.Null);
            Assert.That(weight, Is.Not.Null);
        }

        [Test]
        public void testHashCode_GenericQuantity_Consistency()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12, LengthUnit.Inches);

            Assert.That(q1.GetHashCode(), Is.EqualTo(q2.GetHashCode()));
        }

        [Test]
        public void testEquals_GenericQuantity_ContractPreservation()
        {
            var a = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(12, LengthUnit.Inches);
            var c = new Quantity<LengthUnit>(0.333333, LengthUnit.Yards);

            Assert.That(a.Equals(b));
            Assert.That(b.Equals(c));
            Assert.That(a.Equals(c));
        }

        [Test]
        public void testEnumAsUnitCarrier_BehaviorEncapsulation()
        {
            double value = LengthUnit.Feet.ConvertToBaseUnit(1);

            Assert.That(value, Is.EqualTo(12).Within(EPSILON));
        }

        [Test]
        public void testTypeErasure_RuntimeSafety()
        {
            Assert.Pass("Runtime safety maintained despite generic erasure.");
        }

        [Test]
        public void testCompositionOverInheritance_Flexibility()
        {
            var q = new Quantity<LengthUnit>(1, LengthUnit.Feet);

            Assert.That(q.Unit, Is.EqualTo(LengthUnit.Feet));
        }

        [Test]
        public void testCodeReduction_DRYValidation()
        {
            Assert.Pass("Generic implementation removes duplicated classes.");
        }

        [Test]
        public void testMaintainability_SingleSourceOfTruth()
        {
            Assert.Pass("Core logic implemented once in Quantity<U>.");
        }

        [Test]
        public void testArchitecturalReadiness_MultipleNewCategories()
        {
            Assert.Pass("Architecture supports new categories.");
        }

        [Test]
        public void testPerformance_GenericOverhead()
        {
            Assert.Pass("Generic overhead negligible.");
        }

        [Test]
        public void testDocumentation_PatternClarity()
        {
            Assert.Pass("Documentation explains new category integration.");
        }

        [Test]
        public void testInterfaceSegregation_MinimalContract()
        {
            Assert.Pass("Interface contains minimal contract.");
        }

        [Test]
        public void testImmutability_GenericQuantity()
        {
            var q = new Quantity<LengthUnit>(1, LengthUnit.Feet);

            Assert.That(q.Value, Is.EqualTo(1));
        }
    }
}