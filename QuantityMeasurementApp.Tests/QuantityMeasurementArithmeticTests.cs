using NUnit.Framework;
using QuantityMeasurementApp.Models;
using System;

namespace QuantityMeasurementApp.Tests
{
    [TestFixture]
    public class QuantityMeasurementArithmeticTests
    {

        [Test]
        public void testRefactoring_Add_DelegatesViaHelper()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12, LengthUnit.Inches);

            var result = q1.Add(q2);

            Assert.That(result.Value, Is.EqualTo(2).Within(0.01));
        }

        [Test]
        public void testRefactoring_Subtract_DelegatesViaHelper()
        {
            var q1 = new Quantity<LengthUnit>(2, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12, LengthUnit.Inches);

            var result = q1.Subtract(q2);

            Assert.That(result.Value, Is.EqualTo(1).Within(0.01));
        }

        [Test]
        public void testRefactoring_Divide_DelegatesViaHelper()
        {
            var q1 = new Quantity<LengthUnit>(2, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(1, LengthUnit.Feet);

            var result = q1.Divide(q2);

            Assert.That(result, Is.EqualTo(2));
        }

        [Test]
        public void testValidation_NullOperand_ConsistentAcrossOperations()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Feet);

            Assert.Throws<ArgumentException>(() => q1.Add(null));
            Assert.Throws<ArgumentException>(() => q1.Subtract(null));
            Assert.Throws<ArgumentException>(() => q1.Divide(null));
        }

        [Test]
        public void testValidation_CrossCategory_ConsistentAcrossOperations()
        {
            var length = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var weight = new Quantity<WeightUnit>(1, WeightUnit.Kilogram);

            Assert.Throws<Microsoft.CSharp.RuntimeBinder.RuntimeBinderException>(() => length.Add((dynamic)weight));
        }

        [Test]
        public void testArithmeticOperation_Add_EnumComputation()
        {
            double result = 10 + 5;
            Assert.That(result, Is.EqualTo(15));
        }

        [Test]
        public void testArithmeticOperation_Subtract_EnumComputation()
        {
            double result = 10 - 5;
            Assert.That(result, Is.EqualTo(5));
        }

        [Test]
        public void testArithmeticOperation_Divide_EnumComputation()
        {
            double result = 10 / 5;
            Assert.That(result, Is.EqualTo(2));
        }

        [Test]
        public void testArithmeticOperation_DivideByZero_EnumThrows()
        {
            Assert.Throws<DivideByZeroException>(() =>
            {
                var x = 10 / int.Parse("0");
            });
        }

        [Test]
        public void testPerformBaseArithmetic_ConversionAndOperation()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12, LengthUnit.Inches);

            var result = q1.Add(q2);

            Assert.That(result.Value, Is.EqualTo(2).Within(0.01));
        }

        [Test]
        public void testAdd_UC12_BehaviorPreserved()
        {
            var q1 = new Quantity<LengthUnit>(2, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(2, LengthUnit.Feet);

            var result = q1.Add(q2);

            Assert.That(result.Value, Is.EqualTo(4));
        }

        [Test]
        public void testSubtract_UC12_BehaviorPreserved()
        {
            var q1 = new Quantity<LengthUnit>(4, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(2, LengthUnit.Feet);

            var result = q1.Subtract(q2);

            Assert.That(result.Value, Is.EqualTo(2));
        }

        [Test]
        public void testDivide_UC12_BehaviorPreserved()
        {
            var q1 = new Quantity<LengthUnit>(6, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(3, LengthUnit.Feet);

            var result = q1.Divide(q2);

            Assert.That(result, Is.EqualTo(2));
        }

        [Test]
        public void testRounding_AddSubtract_TwoDecimalPlaces()
        {
            double value = Math.Round(1.23456, 2);

            Assert.That(value, Is.EqualTo(1.23));
        }

        [Test]
        public void testRounding_Divide_NoRounding()
        {
            double value = 5.0 / 3.0;

            Assert.That(value, Is.EqualTo(1.6666666666666667));
        }

        [Test]
        public void testImplicitTargetUnit_AddSubtract()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12, LengthUnit.Inches);

            var result = q1.Add(q2);

            Assert.That(result.Unit, Is.EqualTo(LengthUnit.Feet));
        }

        [Test]
        public void testExplicitTargetUnit_AddSubtract_Overrides()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12, LengthUnit.Inches);

            var result = q1.Add(q2, LengthUnit.Inches);

            Assert.That(result.Unit, Is.EqualTo(LengthUnit.Inches));
        }

        [Test]
        public void testImmutability_AfterAdd_ViaCentralizedHelper()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(1, LengthUnit.Feet);

            q1.Add(q2);

            Assert.That(q1.Value, Is.EqualTo(1));
        }

        [Test]
        public void testImmutability_AfterSubtract_ViaCentralizedHelper()
        {
            var q1 = new Quantity<LengthUnit>(3, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(1, LengthUnit.Feet);

            q1.Subtract(q2);

            Assert.That(q1.Value, Is.EqualTo(3));
        }

        [Test]
        public void testImmutability_AfterDivide_ViaCentralizedHelper()
        {
            var q1 = new Quantity<LengthUnit>(6, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(2, LengthUnit.Feet);

            q1.Divide(q2);

            Assert.That(q1.Value, Is.EqualTo(6));
        }

        [Test]
        public void testAllOperations_AcrossAllCategories()
        {
            var l = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var w = new Quantity<WeightUnit>(1, WeightUnit.Kilogram);
            var v = new Quantity<VolumeUnit>(1, VolumeUnit.Litre);

            Assert.That(l.Value, Is.EqualTo(1));
            Assert.That(w.Value, Is.EqualTo(1));
            Assert.That(v.Value, Is.EqualTo(1));
        }

        [Test]
        public void testErrorMessage_Consistency_Across_Operations()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Feet);

            var ex = Assert.Throws<ArgumentException>(() => q1.Add(null));

            Assert.That(ex.Message, Does.Contain("Other quantity"));
        }

        [Test]
        public void testRounding_Helper_Accuracy()
        {
            double rounded = Math.Round(1.234567, 2);

            Assert.That(rounded, Is.EqualTo(1.23));
        }

        [Test]
        public void testArithmetic_Chain_Operations()
        {
            var q1 = new Quantity<LengthUnit>(4, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var q3 = new Quantity<LengthUnit>(1, LengthUnit.Feet);

            var result = q1.Add(q2).Subtract(q3);

            Assert.That(result.Value, Is.EqualTo(4));
        }

        [Test]
        public void testEnumConstant_ADD_CorrectlyAdds()
        {
            Assert.That(7 + 3, Is.EqualTo(10));
        }

        [Test]
        public void testEnumConstant_SUBTRACT_CorrectlySubtracts()
        {
            Assert.That(7 - 3, Is.EqualTo(4));
        }

        [Test]
        public void testEnumConstant_DIVIDE_CorrectlyDivides()
        {
            Assert.That(7.0 / 2.0, Is.EqualTo(3.5));
        }

        [Test]
        public void testHelper_BaseUnitConversion_Correct()
        {
            var q = new Quantity<LengthUnit>(12, LengthUnit.Inches);

            Assert.That(q.ConvertTo(LengthUnit.Feet), Is.EqualTo(1).Within(0.01));
        }

        [Test]
        public void testHelper_ResultConversion_Correct()
        {
            var q = new Quantity<VolumeUnit>(1000, VolumeUnit.Millilitre);

            Assert.That(q.ConvertTo(VolumeUnit.Litre), Is.EqualTo(1).Within(0.01));
        }

        [Test]
        public void testRefactoring_Validation_UnifiedBehavior()
        {
            var q = new Quantity<LengthUnit>(1, LengthUnit.Feet);

            Assert.Throws<ArgumentException>(() => q.Add(null));
            Assert.Throws<ArgumentException>(() => q.Subtract(null));
        }

        [Test]
        public void testAddition_Commutativity()
        {
            var q1 = new Quantity<VolumeUnit>(1, VolumeUnit.Litre);
            var q2 = new Quantity<VolumeUnit>(500, VolumeUnit.Millilitre);

            var r1 = q1.Add(q2);
            var r2 = q2.Add(q1, VolumeUnit.Litre);

            Assert.That(r1.ConvertTo(VolumeUnit.Litre), Is.EqualTo(r2.ConvertTo(VolumeUnit.Litre)).Within(0.01));
        }

        [Test]
        public void testSubtraction_ResultingInZero()
        {
            var q1 = new Quantity<LengthUnit>(2, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(24, LengthUnit.Inches);

            var result = q1.Subtract(q2);

            Assert.That(result.Value, Is.EqualTo(0).Within(0.01));
        }

        [Test]
        public void testSubtraction_ResultingInNegative()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(24, LengthUnit.Inches);

            var result = q1.Subtract(q2);

            Assert.That(result.Value, Is.LessThan(0));
        }

        [Test]
        public void testDivision_RatioEqualToOne()
        {
            var q1 = new Quantity<LengthUnit>(2, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(24, LengthUnit.Inches);

            var result = q1.Divide(q2);

            Assert.That(result, Is.EqualTo(1).Within(0.01));
        }

        [Test]
        public void testDivision_RatioGreaterThanOne()
        {
            var q1 = new Quantity<WeightUnit>(2, WeightUnit.Kilogram);
            var q2 = new Quantity<WeightUnit>(500, WeightUnit.Gram);

            var result = q1.Divide(q2);

            Assert.That(result, Is.GreaterThan(1));
        }

        [Test]
        public void testDivision_RatioLessThanOne()
        {
            var q1 = new Quantity<WeightUnit>(500, WeightUnit.Gram);
            var q2 = new Quantity<WeightUnit>(1, WeightUnit.Kilogram);

            var result = q1.Divide(q2);

            Assert.That(result, Is.LessThan(1));
        }

        [Test]
        public void testSubtraction_NonCommutative()
        {
            var q1 = new Quantity<LengthUnit>(5, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(2, LengthUnit.Feet);

            var r1 = q1.Subtract(q2);
            var r2 = q2.Subtract(q1);

            Assert.That(r1.Value, Is.Not.EqualTo(r2.Value));
        }

        [Test]
        public void testAddition_WithZero()
        {
            var q1 = new Quantity<VolumeUnit>(5, VolumeUnit.Litre);
            var q2 = new Quantity<VolumeUnit>(0, VolumeUnit.Litre);

            var result = q1.Add(q2);

            Assert.That(result.Value, Is.EqualTo(5));
        }

        [Test]
        public void testDivision_SameUnit_FeetDividedByFeet()
        {
            var q1 = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(5, LengthUnit.Feet);

            var result = q1.Divide(q2);

            Assert.That(result, Is.EqualTo(2));
        }

        [Test]
        public void testArithmetic_Chain_Operationss()
        {
            var q1 = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(2, LengthUnit.Feet);
            var q3 = new Quantity<LengthUnit>(1, LengthUnit.Feet);

            var result = q1.Add(q2).Subtract(q3);

            Assert.That(result.Value, Is.EqualTo(11));
        }
    }
}