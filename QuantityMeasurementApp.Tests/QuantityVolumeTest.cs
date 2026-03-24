using NUnit.Framework;
using QuantityMeasurementModel;
using QuantityMeasurementService.Core;

namespace QuantityMeasurementApp.Tests
{
    [TestFixture]
    public class QuantityMeasurementServiceVolumeTests
    {
        private IQuantityMeasurementService _service = null!;
        private const double Epsilon = 1e-5;

        [SetUp]
        public void Setup()
        {
            _service = new QuantityMeasurementService.Core.QuantityMeasurementService(new MockRepository());
        }

        [Test]
        public void Compare_LitreAndMillilitre_SameMagnitude_ReturnsTrue()
        {
            var request = new MeasurementRequestDTO
            {
                MeasurementCategory = "Volume",
                OperationType = MeasurementAction.Compare,
                MeasurementValue1 = 1.0,
                MeasurementUnit1 = "LITRE",
                MeasurementValue2 = 1000.0,
                MeasurementUnit2 = "MILLILITRE"
            };

            var response = _service.ProcessMeasurement(request);

            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.IsComparison, Is.True);
            Assert.That(response.AreEqual, Is.True);
        }

        [Test]
        public void Compare_GallonAndLitre_EquivalentValues_ReturnsTrue()
        {
            var request = new MeasurementRequestDTO
            {
                MeasurementCategory = "Volume",
                OperationType = MeasurementAction.Compare,
                MeasurementValue1 = 1.0,
                MeasurementUnit1 = "GALLON",
                MeasurementValue2 = 3.78541,
                MeasurementUnit2 = "LITRE"
            };
            var response = _service.ProcessMeasurement(request);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.IsComparison, Is.True);
            Assert.That(response.AreEqual, Is.True);
        }

        [Test]
        public void Add_LitreAndMillilitre_ReturnsSumInLitres()
        {
            var request = new MeasurementRequestDTO
            {
                MeasurementCategory = "Volume",
                OperationType = MeasurementAction.Add,
                MeasurementValue1 = 1.0,
                MeasurementUnit1 = "LITRE",
                MeasurementValue2 = 1000.0,
                MeasurementUnit2 = "MILLILITRE",
                TargetMeasurementUnit = "LITRE"
            };
            var response = _service.ProcessMeasurement(request);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.CalculatedValue, Is.EqualTo(2.0).Within(Epsilon));
        }

        [Test]
        public void Subtract_Litres_ReturnsCorrectRemainingVolume()
        {
            var request = new MeasurementRequestDTO
            {
                MeasurementCategory = "Volume",
                OperationType = MeasurementAction.Subtract,
                MeasurementValue1 = 10.0,
                MeasurementUnit1 = "LITRE",
                MeasurementValue2 = 3.0,
                MeasurementUnit2 = "LITRE",
                TargetMeasurementUnit = "LITRE"
            };

            var response = _service.ProcessMeasurement(request);

            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.CalculatedValue, Is.EqualTo(7.0).Within(Epsilon));
        }

        [Test]
        public void Division_ByZeroValue_ReturnsInfinity()
        {
            var request = new MeasurementRequestDTO
            {
                MeasurementCategory = "Volume",
                OperationType = MeasurementAction.Divide,
                MeasurementValue1 = 10.0,
                MeasurementUnit1 = "LITRE",
                MeasurementValue2 = 0.0,
                MeasurementUnit2 = "LITRE",
                TargetMeasurementUnit = "LITRE"
            };
            var response = _service.ProcessMeasurement(request);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(double.IsInfinity(response.CalculatedValue), Is.True);
        }

        [Test]
        public void Validate_NegativeVolume_ReturnsFailedDtoWithError()
        {
            var request = new MeasurementRequestDTO
            {
                MeasurementCategory = "Volume",
                OperationType = MeasurementAction.Add,
                MeasurementValue1 = -5.0,
                MeasurementUnit1 = "LITRE",
                MeasurementValue2 = 2.0,
                MeasurementUnit2 = "LITRE",
                TargetMeasurementUnit = "LITRE"
            };
            var response = _service.ProcessMeasurement(request);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.ErrorMessage, Does.Contain("invalid"));
        }
    }
}