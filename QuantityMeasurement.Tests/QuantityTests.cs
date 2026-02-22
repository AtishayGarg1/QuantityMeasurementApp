using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurement.Library.Model;

namespace QuantityMeasurement.Tests
{
    [TestClass]
    public class QuantityTests
    {
        private const double TOLERANCE = 1e-6;

        [TestMethod]
        public void Length_Equality_CrossUnit()
        {
            var a = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(12, LengthUnit.Inch);

            Assert.IsTrue(a.Equals(b));
        }

        [TestMethod]
        public void Weight_Equality_CrossUnit()
        {
            var a = new Quantity<WeightUnit>(1, WeightUnit.Kilogram);
            var b = new Quantity<WeightUnit>(1000, WeightUnit.Gram);

            Assert.IsTrue(a.Equals(b));
        }

        [TestMethod]
        public void Length_Addition()
        {
            var a = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(12, LengthUnit.Inch);

            var result = a.Add(b);

            Assert.AreEqual(2, result.Value, TOLERANCE);
        }

        [TestMethod]
        public void Weight_Addition()
        {
            var a = new Quantity<WeightUnit>(1, WeightUnit.Kilogram);
            var b = new Quantity<WeightUnit>(1000, WeightUnit.Gram);

            var result = a.Add(b);

            Assert.AreEqual(2, result.Value, TOLERANCE);
        }

        [TestMethod]
        public void Conversion_Length()
        {
            var length = new Quantity<LengthUnit>(3, LengthUnit.Feet);
            var result = length.ConvertTo(LengthUnit.Yard);

            Assert.AreEqual(1, result.Value, TOLERANCE);
        }

        [TestMethod]
        public void Conversion_Weight()
        {
            var weight = new Quantity<WeightUnit>(1, WeightUnit.Kilogram);
            var result = weight.ConvertTo(WeightUnit.Pound);

            Assert.AreEqual(2.20462, result.Value, 1e-5);
        }
    }
}