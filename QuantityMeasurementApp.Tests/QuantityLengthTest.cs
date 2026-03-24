using NUnit.Framework;
using QuantityMeasurementModel;
using QuantityMeasurementService.Core;

namespace QuantityMeasurementApp.Tests
{
    [TestFixture]
    public class QuantityMeasurementServiceLengthTests
    {
        private IQuantityMeasurementService _service = null!;

        [SetUp]
        public void Setup()
        {
            _service = new QuantityMeasurementService.Core.QuantityMeasurementService(new MockRepository());
        }

        [Test]
        public void Compare_SameValuesInDifferentUnits_ReturnsTrue()
        {
            var request = new MeasurementRequestDTO
            {
                MeasurementCategory = "Length",
                OperationType = MeasurementAction.Compare,
                MeasurementValue1 = 1.0,
                MeasurementUnit1 = "FEET",
                MeasurementValue2 = 12.0,
                MeasurementUnit2 = "INCH"
            };

            var response = _service.ProcessMeasurement(request);

            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.IsComparison, Is.True);
            Assert.That(response.AreEqual, Is.True);
        }

        [Test]
        public void Add_OneFootAndTwelveInches_ReturnsTwoFeet()
        {
            var request = new MeasurementRequestDTO
            {
                MeasurementCategory = "Length",
                OperationType = MeasurementAction.Add,
                MeasurementValue1 = 1.0,
                MeasurementUnit1 = "FEET",
                MeasurementValue2 = 12.0,
                MeasurementUnit2 = "INCH",
                TargetMeasurementUnit = "FEET"
            };

            var response = _service.ProcessMeasurement(request);

            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.CalculatedValue, Is.EqualTo(2.0).Within(1e-6));
            Assert.That(response.FormattedMessage, Is.EqualTo("1 FEET + 12 INCH = 2 FEET"));
        }

        [Test]
        public void Subtract_FeetAndInches_ReturnsCorrectFeet()
        {
            var request = new MeasurementRequestDTO
            {
                MeasurementCategory = "Length",
                OperationType = MeasurementAction.Subtract,
                MeasurementValue1 = 10.0,
                MeasurementUnit1 = "FEET",
                MeasurementValue2 = 6.0,
                MeasurementUnit2 = "INCH",
                TargetMeasurementUnit = "FEET"
            };

            var response = _service.ProcessMeasurement(request);

            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.CalculatedValue, Is.EqualTo(9.5).Within(1e-6));
        }

        [TestCase(MeasurementAction.Add)]
        [TestCase(MeasurementAction.Subtract)]
        [TestCase(MeasurementAction.Divide)]
        public void Arithmetic_InvalidUnit_ReturnsFailedDto(MeasurementAction op)
        {
            var request = new MeasurementRequestDTO
            {
                MeasurementCategory = "Length",
                OperationType = op,
                MeasurementValue1 = 1.0,
                MeasurementUnit1 = "NOT_A_REAL_UNIT",
                MeasurementValue2 = 12.0,
                MeasurementUnit2 = "INCH",
                TargetMeasurementUnit = "FEET"
            };

            var response = _service.ProcessMeasurement(request);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.ErrorMessage, Is.EqualTo("Invalid unit provided."));
        }
    }
}