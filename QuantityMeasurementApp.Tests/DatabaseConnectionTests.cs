using QuantityMeasurementModel;
using QuantityMeasurementModel.DTOs;
using QuantityMeasurementRepository;
using QuantityMeasurementRepository.Database;
using QuantityMeasurementRepository.Util;

namespace QuantityMeasurementApp.Tests
{
    // UC16: Unit tests for QuantityMeasurementDatabaseRepository
    // Uses a separate test database on SQL Server Express
    [TestFixture]
    public class DatabaseConnectionTests
    {
        private IQuantityMeasurementRepository _repo = null!;

        // Connection string pointing to a test database
        // A separate database keeps test data away from real app data
        private const string TestConnectionString =
            "Server=localhost\\SQLEXPRESS;Database=QuantityMeasurementDb;Trusted_Connection=True;TrustServerCertificate=True;";

        [SetUp]
        public void SetUp()
        {
            // Create the repo using the test database
            _repo = new QuantityMeasurementDatabaseRepository(new TestDatabaseConfig());

            // Clear all data before each test so tests don't affect each other
            _repo.DeleteAll();
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up after each test
            _repo.DeleteAll();
            _repo.ReleaseResources();
        }

        // -- Helper methods to create test entities ----------------------------

        private static QuantityMeasurementEntity MakeCompareEntity()
        {
            var a = new QuantityDTO(100, "Centimeter", "Length");
            var b = new QuantityDTO(1, "Meter", "Length");
            return new QuantityMeasurementEntity("Compare", a, b, true);
        }

        private static QuantityMeasurementEntity MakeConvertEntity()
        {
            var input = new QuantityDTO(1, "Kilogram", "Weight");
            var output = new QuantityDTO(1000, "Gram", "Weight");
            return new QuantityMeasurementEntity("Convert", input, output);
        }

        private static QuantityMeasurementEntity MakeAddEntity()
        {
            var a = new QuantityDTO(1, "Foot", "Length");
            var b = new QuantityDTO(12, "Inch", "Length");
            var result = new QuantityDTO(2, "Foot", "Length");
            return new QuantityMeasurementEntity("Add", a, b, result);
        }

        private static QuantityMeasurementEntity MakeErrorEntity()
        {
            return new QuantityMeasurementEntity("Compare", "Incompatible categories");
        }

        // -- Tests -------------------------------------------------------------

        [Test]
        public void Save_OneEntity_TotalCountIsOne()
        {
            _repo.Save(MakeCompareEntity());

            Assert.That(_repo.GetTotalCount(), Is.EqualTo(1));
        }

        [Test]
        public void GetAll_AfterSavingThree_ReturnsThree()
        {
            _repo.Save(MakeCompareEntity());
            _repo.Save(MakeConvertEntity());
            _repo.Save(MakeAddEntity());

            var all = _repo.GetAll();

            Assert.That(all.Count, Is.EqualTo(3));
        }

        [Test]
        public void GetRecent_RequestTwo_GetsLatestTwo()
        {
            _repo.Save(MakeCompareEntity());
            _repo.Save(MakeConvertEntity());
            _repo.Save(MakeAddEntity());

            var recent = _repo.GetRecent(2);

            Assert.That(recent.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetByOperation_Compare_OnlyReturnsCompareRecords()
        {
            _repo.Save(MakeCompareEntity());
            _repo.Save(MakeCompareEntity());
            _repo.Save(MakeConvertEntity());

            var compareRecords = _repo.GetByOperation("Compare");

            Assert.That(compareRecords.Count, Is.EqualTo(2));
            Assert.That(compareRecords.All(e => e.Operation == "Compare"), Is.True);
        }

        [Test]
        public void GetByOperation_NoMatchingRecords_ReturnsEmpty()
        {
            _repo.Save(MakeCompareEntity());

            var result = _repo.GetByOperation("Multiply"); // doesn't exist

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GetByCategory_Length_OnlyReturnsLengthRecords()
        {
            _repo.Save(MakeCompareEntity()); // Length
            _repo.Save(MakeConvertEntity()); // Weight

            var lengthRecords = _repo.GetByCategory("Length");

            Assert.That(lengthRecords.Count, Is.EqualTo(1));
        }

        [Test]
        public void GetTotalCount_AfterFiveSaves_ReturnsFive()
        {
            for (int i = 0; i < 5; i++)
                _repo.Save(MakeCompareEntity());

            Assert.That(_repo.GetTotalCount(), Is.EqualTo(5));
        }

        [Test]
        public void DeleteAll_AfterSavingTwo_CountBecomesZero()
        {
            _repo.Save(MakeCompareEntity());
            _repo.Save(MakeConvertEntity());

            _repo.DeleteAll();

            Assert.That(_repo.GetTotalCount(), Is.EqualTo(0));
        }

        [Test]
        public void Clear_SameAsDeleteAll_CountBecomesZero()
        {
            _repo.Save(MakeCompareEntity());

            _repo.Clear();

            Assert.That(_repo.GetTotalCount(), Is.EqualTo(0));
        }

        [Test]
        public void Save_ErrorEntity_HasErrorIsTrue()
        {
            _repo.Save(MakeErrorEntity());

            var all = _repo.GetAll();

            Assert.That(all.Count, Is.EqualTo(1));
            Assert.That(all[0].HasError, Is.True);
            Assert.That(all[0].ErrorMessage, Is.Not.Empty);
        }

        [Test]
        public void Save_ThenGetAll_DataIsPreservedCorrectly()
        {
            var original = MakeCompareEntity();
            _repo.Save(original);

            var loaded = _repo.GetAll()[0];

            Assert.That(loaded.Operation, Is.EqualTo("Compare"));
            Assert.That(loaded.BoolResult, Is.True);
            Assert.That(loaded.Operand1?.Value, Is.EqualTo(100));
            Assert.That(loaded.Operand1?.Unit, Is.EqualTo("Centimeter"));
            Assert.That(loaded.Operand1?.Category, Is.EqualTo("Length"));
            Assert.That(loaded.HasError, Is.False);
        }

        [Test]
        public void GetByOperation_SqlInjectionAttempt_ReturnsEmpty()
        {
            _repo.Save(MakeCompareEntity());

            // This injection string would break non-parameterised queries
            string injection = "Compare' OR '1'='1";
            var result = _repo.GetByOperation(injection);

            Assert.That(result, Is.Empty, "Parameterised SQL must block injection.");
        }

        [Test]
        public void GetPoolStatistics_ReturnsNonEmptyString()
        {
            string stats = _repo.GetPoolStatistics();

            Assert.That(stats, Is.Not.Null.And.Not.Empty);
            Console.WriteLine(stats);
        }

        // -- Test config that points to the test database ----------------------

        private class TestDatabaseConfig : DatabaseConfig
        {
            public TestDatabaseConfig() : base(null) { }
            public override string ConnectionString => TestConnectionString;
            public override string RepositoryType => "database";
        }
    }
}