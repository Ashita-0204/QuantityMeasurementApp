using NUnit.Framework;
using QuantityMeasurementBusinessLayer;
using QuantityMeasurementModel;
using QuantityMeasurementModel.DTOs;
using QuantityMeasurementRepository;

namespace QuantityMeasurementApp.Tests
{
    [TestFixture]
    public class QuantityMeasurementNTierTests
    {
        private QuantityMeasurementServiceImpl _service;

        [SetUp]
        public void SetUp()
        {
            _service = new QuantityMeasurementServiceImpl(
                QuantityMeasurementCacheRepository.Instance);
        }

        // ----------------------------------------------------------------
        // ENTITY TESTS
        // ----------------------------------------------------------------

        [Test]
        public void TestEntity_SingleOperand_Construction()
        {
            var input = new QuantityDTO(1, "Feet", "Length");
            var result = new QuantityDTO(12, "Inches", "Length");

            var entity = new QuantityMeasurementEntity("Convert", input, result);

            Assert.That(entity.Operation, Is.EqualTo("Convert"));
            Assert.That(entity.Operand1, Is.EqualTo(input));
            Assert.That(entity.Result, Is.EqualTo(result));
            Assert.That(entity.HasError, Is.False);
        }

        [Test]
        public void TestEntity_BinaryOperand_Construction()
        {
            var a = new QuantityDTO(1, "Feet", "Length");
            var b = new QuantityDTO(12, "Inches", "Length");
            var result = new QuantityDTO(2, "Feet", "Length");

            var entity = new QuantityMeasurementEntity("Add", a, b, result);

            Assert.That(entity.Operation, Is.EqualTo("Add"));
            Assert.That(entity.Operand1, Is.EqualTo(a));
            Assert.That(entity.Operand2, Is.EqualTo(b));
            Assert.That(entity.Result, Is.EqualTo(result));
            Assert.That(entity.HasError, Is.False);
        }

        [Test]
        public void TestEntity_Error_Construction()
        {
            var entity = new QuantityMeasurementEntity("Add", "Temperature does not support Add.");

            Assert.That(entity.HasError, Is.True);
            Assert.That(entity.Operation, Is.EqualTo("Add"));
            Assert.That(entity.ErrorMessage, Is.EqualTo("Temperature does not support Add."));
        }

        // ----------------------------------------------------------------
        // LENGTH TESTS
        // ----------------------------------------------------------------

        [Test]
        public void TestLength_1Feet_Equals_1Feet()
        {
            var response = _service.Compare(
                new QuantityDTO(1, "Feet", "Length"),
                new QuantityDTO(1, "Feet", "Length"));

            Assert.That(response.Success, Is.True);
            Assert.That(response.BoolResult, Is.True);
        }

        [Test]
        public void TestLength_1Feet_Equals_12Inches()
        {
            var response = _service.Compare(
                new QuantityDTO(1, "Feet", "Length"),
                new QuantityDTO(12, "Inches", "Length"));

            Assert.That(response.Success, Is.True);
            Assert.That(response.BoolResult, Is.True);
        }

        [Test]
        public void TestLength_1Yards_Equals_36Inches()
        {
            var response = _service.Compare(
                new QuantityDTO(1, "Yards", "Length"),
                new QuantityDTO(36, "Inches", "Length"));

            Assert.That(response.Success, Is.True);
            Assert.That(response.BoolResult, Is.True);
        }

        [Test]
        public void TestLength_Convert_1Feet_To_Inches()
        {
            var response = _service.Convert(
                new QuantityDTO(1, "Feet", "Length"), "Inches");

            Assert.That(response.Success, Is.True);
            Assert.That(response.Result!.Value, Is.EqualTo(12).Within(0.001));
            Assert.That(response.Result.Unit, Is.EqualTo("Inches"));
        }

        [Test]
        public void TestLength_Add_1Feet_And_12Inches()
        {
            var response = _service.Add(
                new QuantityDTO(1, "Feet", "Length"),
                new QuantityDTO(12, "Inches", "Length"));

            Assert.That(response.Success, Is.True);
            Assert.That(response.Result!.Value, Is.EqualTo(2).Within(0.001));
            Assert.That(response.Result.Unit, Is.EqualTo("Feet"));
        }

        [Test]
        public void TestLength_Subtract_2Feet_Minus_12Inches()
        {
            var response = _service.Subtract(
                new QuantityDTO(2, "Feet", "Length"),
                new QuantityDTO(12, "Inches", "Length"));

            Assert.That(response.Success, Is.True);
            Assert.That(response.Result!.Value, Is.EqualTo(1).Within(0.001));
        }

        [Test]
        public void TestLength_Divide_2Feet_By_1Feet()
        {
            var response = _service.Divide(
                new QuantityDTO(2, "Feet", "Length"),
                new QuantityDTO(1, "Feet", "Length"));

            Assert.That(response.Success, Is.True);
            Assert.That(response.ScalarResult!.Value, Is.EqualTo(2).Within(0.001));
        }

        [Test]
        public void TestLength_Divide_ByZero_ReturnsError()
        {
            var response = _service.Divide(
                new QuantityDTO(1, "Feet", "Length"),
                new QuantityDTO(0, "Feet", "Length"));

            Assert.That(response.Success, Is.False);
            Assert.That(response.ErrorMessage, Does.Contain("zero").IgnoreCase);
        }

        // ----------------------------------------------------------------
        // WEIGHT TESTS
        // ----------------------------------------------------------------

        [Test]
        public void TestWeight_1Kilogram_Equals_1000Gram()
        {
            var response = _service.Compare(
                new QuantityDTO(1, "Kilogram", "Weight"),
                new QuantityDTO(1000, "Gram", "Weight"));

            Assert.That(response.Success, Is.True);
            Assert.That(response.BoolResult, Is.True);
        }

        [Test]
        public void TestWeight_Convert_1Kilogram_To_Gram()
        {
            var response = _service.Convert(
                new QuantityDTO(1, "Kilogram", "Weight"), "Gram");

            Assert.That(response.Success, Is.True);
            Assert.That(response.Result!.Value, Is.EqualTo(1000).Within(0.001));
        }

        [Test]
        public void TestWeight_Add_500Gram_And_500Gram_In_Kilogram()
        {
            var response = _service.Add(
                new QuantityDTO(500, "Gram", "Weight"),
                new QuantityDTO(500, "Gram", "Weight"), "Kilogram");

            Assert.That(response.Success, Is.True);
            Assert.That(response.Result!.Value, Is.EqualTo(1).Within(0.001));
        }

        // ----------------------------------------------------------------
        // VOLUME TESTS
        // ----------------------------------------------------------------

        [Test]
        public void TestVolume_1Gallon_Equals_3point78Litre()
        {
            var response = _service.Compare(
                new QuantityDTO(1, "Gallon", "Volume"),
                new QuantityDTO(3.78541, "Litre", "Volume"));

            Assert.That(response.Success, Is.True);
            Assert.That(response.BoolResult, Is.True);
        }

        [Test]
        public void TestVolume_1Litre_Equals_1000Millilitre()
        {
            var response = _service.Compare(
                new QuantityDTO(1, "Litre", "Volume"),
                new QuantityDTO(1000, "Millilitre", "Volume"));

            Assert.That(response.Success, Is.True);
            Assert.That(response.BoolResult, Is.True);
        }

        [Test]
        public void TestVolume_Convert_1Gallon_To_Litre()
        {
            var response = _service.Convert(
                new QuantityDTO(1, "Gallon", "Volume"), "Litre");

            Assert.That(response.Success, Is.True);
            Assert.That(response.Result, Is.Not.Null);
        }

        // ----------------------------------------------------------------
        // TEMPERATURE TESTS
        // ----------------------------------------------------------------

        [Test]
        public void TestTemperature_0Celsius_Equals_32Fahrenheit()
        {
            var response = _service.Compare(
                new QuantityDTO(0, "Celsius", "Temperature"),
                new QuantityDTO(32, "Fahrenheit", "Temperature"));

            Assert.That(response.Success, Is.True);
            Assert.That(response.BoolResult, Is.True);
        }

        [Test]
        public void TestTemperature_Convert_100Celsius_To_Fahrenheit()
        {
            var response = _service.Convert(
                new QuantityDTO(100, "Celsius", "Temperature"), "Fahrenheit");

            Assert.That(response.Success, Is.True);
            Assert.That(response.Result!.Value, Is.EqualTo(212).Within(0.001));
        }

        [Test]
        public void TestTemperature_Add_IsNotSupported()
        {
            var response = _service.Add(
                new QuantityDTO(100, "Celsius", "Temperature"),
                new QuantityDTO(50, "Celsius", "Temperature"));

            Assert.That(response.Success, Is.False);
            Assert.That(response.ErrorMessage, Does.Contain("Temperature"));
        }

        [Test]
        public void TestTemperature_Subtract_IsNotSupported()
        {
            var response = _service.Subtract(
                new QuantityDTO(100, "Celsius", "Temperature"),
                new QuantityDTO(50, "Celsius", "Temperature"));

            Assert.That(response.Success, Is.False);
            Assert.That(response.ErrorMessage, Does.Contain("Temperature"));
        }

        [Test]
        public void TestTemperature_Divide_IsNotSupported()
        {
            var response = _service.Divide(
                new QuantityDTO(100, "Celsius", "Temperature"),
                new QuantityDTO(50, "Celsius", "Temperature"));

            Assert.That(response.Success, Is.False);
            Assert.That(response.ErrorMessage, Does.Contain("Temperature"));
        }

        // ----------------------------------------------------------------
        // CROSS CATEGORY TESTS
        // ----------------------------------------------------------------

        [Test]
        public void TestCrossCategory_Compare_ReturnsError()
        {
            var response = _service.Compare(
                new QuantityDTO(1, "Feet", "Length"),
                new QuantityDTO(1, "Kilogram", "Weight"));

            Assert.That(response.Success, Is.False);
            Assert.That(response.ErrorMessage, Does.Contain("categories"));
        }

        [Test]
        public void TestCrossCategory_Add_ReturnsError()
        {
            var response = _service.Add(
                new QuantityDTO(1, "Feet", "Length"),
                new QuantityDTO(1, "Kilogram", "Weight"));

            Assert.That(response.Success, Is.False);
        }

        // ----------------------------------------------------------------
        // NULL VALIDATION
        // ----------------------------------------------------------------

        [Test]
        public void TestService_NullRepository_ThrowsException()
        {
            Assert.That(() => new QuantityMeasurementServiceImpl(null!),
                Throws.TypeOf<ArgumentNullException>());
        }

        // ----------------------------------------------------------------
        // OPERATION NAME TESTS
        // ----------------------------------------------------------------

        [Test]
        public void TestOperation_Compare_HasCorrectName()
        {
            var response = _service.Compare(
                new QuantityDTO(1, "Feet", "Length"),
                new QuantityDTO(1, "Feet", "Length"));

            Assert.That(response.Operation, Is.EqualTo("Compare"));
        }

        [Test]
        public void TestOperation_Convert_HasCorrectName()
        {
            var response = _service.Convert(
                new QuantityDTO(1, "Feet", "Length"), "Inches");

            Assert.That(response.Operation, Is.EqualTo("Convert"));
        }

        [Test]
        public void TestOperation_Add_HasCorrectName()
        {
            var response = _service.Add(
                new QuantityDTO(1, "Feet", "Length"),
                new QuantityDTO(1, "Feet", "Length"));

            Assert.That(response.Operation, Is.EqualTo("Add"));
        }

        [Test]
        public void TestOperation_Subtract_HasCorrectName()
        {
            var response = _service.Subtract(
                new QuantityDTO(2, "Feet", "Length"),
                new QuantityDTO(1, "Feet", "Length"));

            Assert.That(response.Operation, Is.EqualTo("Subtract"));
        }

        [Test]
        public void TestOperation_Divide_HasCorrectName()
        {
            var response = _service.Divide(
                new QuantityDTO(2, "Feet", "Length"),
                new QuantityDTO(1, "Feet", "Length"));

            Assert.That(response.Operation, Is.EqualTo("Divide"));
        }
    }
}