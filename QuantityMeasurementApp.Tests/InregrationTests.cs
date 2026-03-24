using QuantityMeasurementBusinessLayer;
using QuantityMeasurementModel.DTOs;
using QuantityMeasurementRepository;
using QuantityMeasurementRepository.Database;
using QuantityMeasurementRepository.Util;

namespace QuantityMeasurementApp.Tests
{
    // Valid unit names from your enums:
    // Length:      Feet, Inches, Yards, Centimeters
    // Weight:      Kilogram, Gram, Pound
    // Volume:      Litre, Millilitre, Gallon
    // Temperature: (check TemperatureUnit.cs for valid names)

    [TestFixture]
    public class IntegrationTests
    {
        private IQuantityMeasurementRepository _repo = null!;
        private QuantityMeasurementServiceImpl _service = null!;

        private const string TestConnectionString =
            "Server=localhost\\SQLEXPRESS;Database=QuantityMeasurementDb;Trusted_Connection=True;TrustServerCertificate=True;";

        [SetUp]
        public void SetUp()
        {
            _repo = new QuantityMeasurementDatabaseRepository(new TestDatabaseConfig());
            _service = new QuantityMeasurementServiceImpl(_repo);
            _repo.DeleteAll();
        }

        [TearDown]
        public void TearDown()
        {
            _repo.DeleteAll();
            _repo.ReleaseResources();
        }

        // ── 1. Compare ────────────────────────────────────────────────────────
        // 1 Feet = 12 Inches → they are equal

        [Test]
        public void Compare_1Feet_And_12Inches_AreEqual_AndPersisted()
        {
            var a = new QuantityDTO(1, "Feet", "Length");
            var b = new QuantityDTO(12, "Inches", "Length");

            var response = _service.Compare(a, b);

            Assert.That(response.BoolResult, Is.True);
            Assert.That(_repo.GetTotalCount(), Is.EqualTo(1));

            var stored = _repo.GetAll()[0];
            Assert.That(stored.Operation, Is.EqualTo("Compare"));
            Assert.That(stored.BoolResult, Is.True);
        }

        // ── 2. Convert ────────────────────────────────────────────────────────
        // 1 Kilogram = 1000 Gram

        [Test]
        public void Convert_1Kilogram_To_Gram_PersistsResult()
        {
            var input = new QuantityDTO(1, "Kilogram", "Weight");
            var response = _service.Convert(input, "Gram");

            Assert.That(response.Success, Is.True);
            Assert.That(_repo.GetTotalCount(), Is.EqualTo(1));

            var stored = _repo.GetAll()[0];
            Assert.That(stored.Operation, Is.EqualTo("Convert"));
            Assert.That(stored.Result?.Value, Is.EqualTo(1000).Within(0.001));
        }

        // ── 3. Add ────────────────────────────────────────────────────────────
        // 1 Feet + 12 Inches = 2 Feet

        [Test]
        public void Add_1Feet_And_12Inches_Equals2Feet_AndPersisted()
        {
            var a = new QuantityDTO(1, "Feet", "Length");
            var b = new QuantityDTO(12, "Inches", "Length");
            var response = _service.Add(a, b, "Feet");

            Assert.That(response.Success, Is.True);
            Assert.That(_repo.GetTotalCount(), Is.EqualTo(1));

            var stored = _repo.GetAll()[0];
            Assert.That(stored.Result?.Value, Is.EqualTo(2).Within(0.001));
        }

        // ── 4. GetByOperation filtering ───────────────────────────────────────

        [Test]
        public void MultipleOperations_GetByOperation_FiltersCorrectly()
        {
            _service.Compare(new QuantityDTO(1, "Feet", "Length"),
                             new QuantityDTO(12, "Inches", "Length"));
            _service.Compare(new QuantityDTO(1, "Kilogram", "Weight"),
                             new QuantityDTO(1, "Kilogram", "Weight"));
            _service.Convert(new QuantityDTO(1, "Feet", "Length"), "Inches");

            Assert.That(_repo.GetTotalCount(), Is.EqualTo(3));
            Assert.That(_repo.GetByOperation("Compare").Count, Is.EqualTo(2));
            Assert.That(_repo.GetByOperation("Convert").Count, Is.EqualTo(1));
        }

        // ── 5. GetByCategory filtering ────────────────────────────────────────

        [Test]
        public void MixedOperations_GetByCategory_IsolatesRecords()
        {
            // Length
            _service.Compare(new QuantityDTO(1, "Feet", "Length"),
                             new QuantityDTO(12, "Inches", "Length"));
            // Weight
            _service.Compare(new QuantityDTO(1, "Kilogram", "Weight"),
                             new QuantityDTO(1000, "Gram", "Weight"));
            // Volume
            _service.Convert(new QuantityDTO(1, "Litre", "Volume"), "Millilitre");

            Assert.That(_repo.GetByCategory("Length"), Has.Count.EqualTo(1));
            Assert.That(_repo.GetByCategory("Weight"), Has.Count.EqualTo(1));
            Assert.That(_repo.GetByCategory("Volume"), Has.Count.EqualTo(1));
        }

        // ── 6. Error is persisted ─────────────────────────────────────────────

        [Test]
        public void Compare_IncompatibleCategories_ErrorIsPersisted()
        {
            var a = new QuantityDTO(1, "Feet", "Length");
            var b = new QuantityDTO(1, "Kilogram", "Weight");

            var response = _service.Compare(a, b);

            Assert.That(response.Success, Is.False);
            Assert.That(_repo.GetTotalCount(), Is.EqualTo(1));

            var stored = _repo.GetAll()[0];
            Assert.That(stored.HasError, Is.True);
            Assert.That(stored.ErrorMessage, Is.Not.Empty);
        }

        // ── 7. DeleteAll ──────────────────────────────────────────────────────

        [Test]
        public void DeleteAll_AfterOperations_CountIsZero()
        {
            _service.Compare(new QuantityDTO(1, "Feet", "Length"),
                             new QuantityDTO(12, "Inches", "Length"));
            _service.Convert(new QuantityDTO(1, "Kilogram", "Weight"), "Gram");

            _repo.DeleteAll();

            Assert.That(_repo.GetTotalCount(), Is.EqualTo(0));
        }

        // ── 8. Divide ─────────────────────────────────────────────────────────
        // 2 Feet / 1 Feet = scalar 2

        [Test]
        public void Divide_2FeetBy1Feet_ScalarIs2_Persisted()
        {
            var a = new QuantityDTO(2, "Feet", "Length");
            var b = new QuantityDTO(1, "Feet", "Length");
            var response = _service.Divide(a, b);

            Assert.That(response.Success, Is.True);

            var stored = _repo.GetAll()[0];
            Assert.That(stored.ScalarResult, Is.EqualTo(2).Within(0.0001));
        }

        // ── 9. GetRecent ordering ─────────────────────────────────────────────

        [Test]
        public void GetRecent_ReturnsLatestFirst()
        {
            _service.Compare(new QuantityDTO(1, "Feet", "Length"),
                             new QuantityDTO(12, "Inches", "Length"));
            _service.Convert(new QuantityDTO(1, "Kilogram", "Weight"), "Gram");

            var recent = _repo.GetRecent(1);
            Assert.That(recent.Count, Is.EqualTo(1));
            Assert.That(recent[0].Operation, Is.EqualTo("Convert"));
        }

        // ── 10. All UC15 operations still work ───────────────────────────────

        [Test]
        public void AllUC15Operations_WorkWithDatabaseRepository()
        {
            Assert.DoesNotThrow(() =>
                _service.Compare(new QuantityDTO(1, "Feet", "Length"),
                                 new QuantityDTO(12, "Inches", "Length")));

            Assert.DoesNotThrow(() =>
                _service.Convert(new QuantityDTO(1, "Litre", "Volume"), "Gallon"));

            Assert.DoesNotThrow(() =>
                _service.Add(new QuantityDTO(1, "Feet", "Length"),
                             new QuantityDTO(12, "Inches", "Length")));

            Assert.DoesNotThrow(() =>
                _service.Subtract(new QuantityDTO(2, "Feet", "Length"),
                                  new QuantityDTO(1, "Feet", "Length")));

            Assert.DoesNotThrow(() =>
                _service.Divide(new QuantityDTO(2, "Feet", "Length"),
                                new QuantityDTO(1, "Feet", "Length")));

            Assert.That(_repo.GetTotalCount(), Is.EqualTo(5));
        }

        // ── Test config ───────────────────────────────────────────────────────

        private class TestDatabaseConfig : DatabaseConfig
        {
            public TestDatabaseConfig() : base(null) { }
            public override string ConnectionString => TestConnectionString;
            public override string RepositoryType => "database";
        }
    }
}