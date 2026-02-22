using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurement.Library.Model;
using QuantityMeasurement.Library.Service;

namespace QuantityMeasurement.Tests
{
    [TestClass]
    public class LengthTests
    {
        private readonly QuantityMeasurementService service =
            new QuantityMeasurementService();

        [TestMethod]
        public void OneYard_Equals_ThreeFeet()
        {
            var yard = new QuantityLength(1, LengthUnit.Yard);
            var feet = new QuantityLength(3, LengthUnit.Feet);

            Assert.IsTrue(service.CompareLength(yard, feet));
        }

        [TestMethod]
        public void OneYard_Equals_ThirtySixInches()
        {
            var yard = new QuantityLength(1, LengthUnit.Yard);
            var inch = new QuantityLength(36, LengthUnit.Inch);

            Assert.IsTrue(service.CompareLength(yard, inch));
        }

        [TestMethod]
        public void TwoPointFiveFourCm_Equals_OneInch()
        {
            var cm = new QuantityLength(2.54, LengthUnit.Centimeters);
            var inch = new QuantityLength(1, LengthUnit.Inch);

            Assert.IsTrue(service.CompareLength(cm, inch));
        }

        [TestMethod]
        public void DifferentValues_ReturnFalse()
        {
            var yard = new QuantityLength(1, LengthUnit.Yard);
            var feet = new QuantityLength(2, LengthUnit.Feet);

            Assert.IsFalse(service.CompareLength(yard, feet));
        }
    }
}