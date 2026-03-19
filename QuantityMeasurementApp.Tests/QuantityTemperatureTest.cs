using NUnit.Framework;
using QuantityMeasurementModel;   
using QuantityMeasurementService; 

namespace QuantityMeasurementApp.Tests
{
    [TestFixture]
    public class QuantityMeasurementServiceTemperatureTests
    {
        private IQuantityMeasurementService _service;

        [SetUp]
        public void Setup()
        {
            _service = new QuantityMeasurementServices(new MockRepository());
        }

        // Comparison & Equality Tests 
        [Test]
        public void Compare_CelsiusAndFahrenheit_FreezingPoint_ReturnsTrue()
        {
            var request = new MeasurementRequestDTO
            {
                MeasurementCategory = "Temperature",
                OperationType = MeasurementAction.Compare,
                MeasurementValue1 = 0.0,
                MeasurementUnit1 = "CELSIUS",
                MeasurementValue2 = 32.0,
                MeasurementUnit2 = "FAHRENHEIT"
            };

            var response = _service.ProcessMeasurement(request);

            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.IsComparison, Is.True);
            Assert.That(response.AreEqual, Is.True);
        }

        [Test]
        public void Compare_KelvinAndCelsius_AbsoluteZeroOffset_ReturnsTrue()
        {
            var request = new MeasurementRequestDTO
            {
                MeasurementCategory = "Temperature",
                OperationType = MeasurementAction.Compare,
                MeasurementValue1 = 273.15,
                MeasurementUnit1 = "KELVIN",
                MeasurementValue2 = 0.0,
                MeasurementUnit2 = "CELSIUS"
            };

            var response = _service.ProcessMeasurement(request);

            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.IsComparison, Is.True);
            Assert.That(response.AreEqual, Is.True);
        }

        [Test]
        public void Compare_FahrenheitAndCelsius_BoilingPoint_ReturnsTrue()
        {
            var request = new MeasurementRequestDTO
            {
                MeasurementCategory = "Temperature",
                OperationType = MeasurementAction.Compare,
                MeasurementValue1 = 212.0,
                MeasurementUnit1 = "FAHRENHEIT",
                MeasurementValue2 = 100.0,
                MeasurementUnit2 = "CELSIUS"
            };

            var response = _service.ProcessMeasurement(request);

            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.IsComparison, Is.True);
            Assert.That(response.AreEqual, Is.True);
        }
        [TestCase(MeasurementAction.Add)]
        [TestCase(MeasurementAction.Subtract)]
        [TestCase(MeasurementAction.Divide)]
        public void Arithmetic_Temperatures_ReturnsFailedDto(MeasurementAction action)
        {
            var request = new MeasurementRequestDTO
            {
                MeasurementCategory = "Temperature",
                OperationType = action,
                MeasurementValue1 = 100.0,
                MeasurementUnit1 = "CELSIUS",
                MeasurementValue2 = 50.0,
                MeasurementUnit2 = "CELSIUS",
                TargetMeasurementUnit = "CELSIUS"
            };

            var response = _service.ProcessMeasurement(request);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.ErrorMessage, Is.Not.Null.And.Not.Empty);
        }
    }
}