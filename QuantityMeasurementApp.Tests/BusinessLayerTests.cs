using NUnit.Framework;
using QuantityMeasurementBusinessLayer;
using QuantityMeasurementModel.DTOs;
using QuantityMeasurementRepository;

namespace QuantityMeasurementApp.Tests
{
    [TestFixture]
    public class BusinessLayerTests
    {
        private IQuantityMeasurementService _service;
        private QuantityMeasurementCacheRepository _repo;

        [SetUp]
        public void Setup()
        {
            _repo = QuantityMeasurementCacheRepository.Instance;
            _repo.Clear();
            _service = new QuantityMeasurementServiceImpl(_repo);
        }

        [Test]
        public void Compare_SameLengthDifferentUnits_ReturnsTrue()
        {
            var a = new QuantityDTO(1.0, "Feet", "Length");
            var b = new QuantityDTO(12.0, "Inches", "Length");

            var resp = _service.Compare(a, b);

            Assert.That(resp.Success, Is.True);
            Assert.That(resp.BoolResult, Is.True);
        }

        [Test]
        public void Convert_FeetToInches_ReturnsCorrectUnit()
        {
            var input = new QuantityDTO(1.0, "Feet", "Length");
            var resp = _service.Convert(input, "Inches");

            Assert.That(resp.Success, Is.True);
            Assert.That(resp.Result, Is.Not.Null);
            Assert.That(resp.Result!.Unit, Is.EqualTo("Inches"));
            Assert.That(resp.Result.Value, Is.EqualTo(12.0).Within(0.0001));
        }

        [Test]
        public void Add_TwoLengths_ReturnsSumInTargetUnit()
        {
            var a = new QuantityDTO(1.0, "Feet", "Length");
            var b = new QuantityDTO(12.0, "Inches", "Length");
            var resp = _service.Add(a, b, "Feet");

            Assert.That(resp.Success, Is.True);
            Assert.That(resp.Result, Is.Not.Null);
            Assert.That(resp.Result!.Value, Is.EqualTo(2.0).Within(0.0001));
        }
    }
}