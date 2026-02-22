using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurement.Library.Model;
using System;

namespace QuantityMeasurement.Tests
{
    [TestClass]
    public class QuantityLengthTests
    {
        private const double TOLERANCE = 0.00001;

        [TestMethod]
        public void OneFoot_Equals_TwelveInches()
        {
            var foot = new QuantityLength(1, LengthUnit.Feet);
            var inches = new QuantityLength(12, LengthUnit.Inch);

            Assert.IsTrue(foot.Equals(inches));
        }

        [TestMethod]
        public void OneYard_Equals_ThreeFeet()
        {
            var yard = new QuantityLength(1, LengthUnit.Yard);
            var feet = new QuantityLength(3, LengthUnit.Feet);

            Assert.IsTrue(yard.Equals(feet));
        }

        [TestMethod]
        public void TwoPointFiveFourCm_Equals_OneInch()
        {
            var cm = new QuantityLength(2.54, LengthUnit.Centimeters);
            var inch = new QuantityLength(1, LengthUnit.Inch);

            Assert.IsTrue(cm.Equals(inch));
        }

        [TestMethod]
        public void DifferentValues_ShouldNotBeEqual()
        {
            var foot = new QuantityLength(1, LengthUnit.Feet);
            var inches = new QuantityLength(10, LengthUnit.Inch);

            Assert.IsFalse(foot.Equals(inches));
        }

        [TestMethod]
        public void Convert_Feet_To_Inches()
        {
            var foot = new QuantityLength(1, LengthUnit.Feet);
            var result = foot.ConvertTo(LengthUnit.Inch);

            Assert.AreEqual(12, result.Value, TOLERANCE);
        }

        [TestMethod]
        public void Convert_Yard_To_Feet()
        {
            var yard = new QuantityLength(1, LengthUnit.Yard);
            var result = yard.ConvertTo(LengthUnit.Feet);

            Assert.AreEqual(3, result.Value, TOLERANCE);
        }

        [TestMethod]
        public void Convert_Feet_To_Centimeters()
        {
            var foot = new QuantityLength(1, LengthUnit.Feet);
            var result = foot.ConvertTo(LengthUnit.Centimeters);

            Assert.AreEqual(30.48, result.Value, TOLERANCE);
        }

        [TestMethod]
        public void StaticConvert_Feet_To_Yard()
        {
            double result = QuantityLength.Convert(
                3, LengthUnit.Feet,
                LengthUnit.Yard);

            Assert.AreEqual(1, result, TOLERANCE);
        }

        [TestMethod]
        public void Add_OneFoot_And_TwelveInches()
        {
            var foot = new QuantityLength(1, LengthUnit.Feet);
            var inches = new QuantityLength(12, LengthUnit.Inch);

            var result = foot.Add(inches);

            Assert.AreEqual(2, result.Value, TOLERANCE);
            Assert.AreEqual(LengthUnit.Feet, result.Unit);
        }

        [TestMethod]
        public void Add_Yard_And_Feet()
        {
            var yard = new QuantityLength(1, LengthUnit.Yard);
            var feet = new QuantityLength(3, LengthUnit.Feet);

            var result = yard.Add(feet);

            Assert.AreEqual(2, result.Value, TOLERANCE);
        }

        [TestMethod]
        public void Add_Zero_Length()
        {
            var a = new QuantityLength(5, LengthUnit.Feet);
            var zero = new QuantityLength(0, LengthUnit.Feet);

            var result = a.Add(zero);

            Assert.AreEqual(5, result.Value, TOLERANCE);
        }

        [TestMethod]
        public void Add_Negative_Length()
        {
            var a = new QuantityLength(5, LengthUnit.Feet);
            var b = new QuantityLength(-2, LengthUnit.Feet);

            var result = a.Add(b);

            Assert.AreEqual(3, result.Value, TOLERANCE);
        }

        [TestMethod]
        public void Add_WithTargetUnit_Yards()
        {
            var foot = new QuantityLength(1, LengthUnit.Feet);
            var inches = new QuantityLength(12, LengthUnit.Inch);

            var result = QuantityLength.Add(
                foot,
                inches,
                LengthUnit.Yard);

            Assert.AreEqual(2.0 / 3.0, result.Value, 0.00001);
            Assert.AreEqual(LengthUnit.Yard, result.Unit);
        }

        [TestMethod]
        public void Add_WithTargetUnit_Centimeters()
        {
            var yard = new QuantityLength(1, LengthUnit.Yard);
            var feet = new QuantityLength(3, LengthUnit.Feet);

            var result = QuantityLength.Add(
                yard,
                feet,
                LengthUnit.Centimeters);

            Assert.AreEqual(182.88, result.Value, 0.00001);
            Assert.AreEqual(LengthUnit.Centimeters, result.Unit);
        }

        [TestMethod]
        public void Add_WithTargetUnit_ShouldBeCommutative()
        {
            var a = new QuantityLength(1, LengthUnit.Feet);
            var b = new QuantityLength(12, LengthUnit.Inch);

            var result1 = QuantityLength.Add(a, b, LengthUnit.Feet);
            var result2 = QuantityLength.Add(b, a, LengthUnit.Feet);

            Assert.IsTrue(result1.Equals(result2));
        }

        
    }
}