using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurement.Library.Model;
using System;

namespace QuantityMeasurement.Tests
{
    [TestClass]
    public class QuantityWeightTests
    {
        private const double TOLERANCE = 1e-6;

        [TestMethod]
        public void OneKilogram_Equals_ThousandGrams()
        {
            var kg = new QuantityWeight(1, WeightUnit.Kilogram);
            var g = new QuantityWeight(1000, WeightUnit.Gram);

            Assert.IsTrue(kg.Equals(g));
        }

        [TestMethod]
        public void OneKilogram_Equals_TwoPointTwoZeroFourSixTwoPounds()
        {
            var kg = new QuantityWeight(1, WeightUnit.Kilogram);
            var lb = new QuantityWeight(2.20462, WeightUnit.Pound);

            Assert.IsTrue(kg.Equals(lb));
        }

        [TestMethod]
        public void DifferentWeights_ShouldNotBeEqual()
        {
            var kg = new QuantityWeight(1, WeightUnit.Kilogram);
            var g = new QuantityWeight(500, WeightUnit.Gram);

            Assert.IsFalse(kg.Equals(g));
        }

        [TestMethod]
        public void Convert_Kilogram_To_Gram()
        {
            var kg = new QuantityWeight(1, WeightUnit.Kilogram);
            var result = kg.ConvertTo(WeightUnit.Gram);

            Assert.AreEqual(1000, result.Value, TOLERANCE);
        }

        [TestMethod]
        public void Convert_Gram_To_Kilogram()
        {
            var g = new QuantityWeight(1000, WeightUnit.Gram);
            var result = g.ConvertTo(WeightUnit.Kilogram);

            Assert.AreEqual(1, result.Value, TOLERANCE);
        }

        [TestMethod]
        public void Convert_Pound_To_Kilogram()
        {
            var lb = new QuantityWeight(1, WeightUnit.Pound);
            var result = lb.ConvertTo(WeightUnit.Kilogram);

            Assert.AreEqual(0.453592, result.Value, TOLERANCE);
        }

        [TestMethod]
        public void StaticConvert_Kilogram_To_Pound()
        {
            double result = QuantityWeight.Convert(
                1, WeightUnit.Kilogram,
                WeightUnit.Pound);

            Assert.AreEqual(2.20462, result, 1e-5);
        }

        [TestMethod]
        public void Add_Kilogram_And_Gram()
        {
            var kg = new QuantityWeight(1, WeightUnit.Kilogram);
            var g = new QuantityWeight(500, WeightUnit.Gram);

            var result = kg.Add(g);

            Assert.AreEqual(1.5, result.Value, TOLERANCE);
            Assert.AreEqual(WeightUnit.Kilogram, result.Unit);
        }

        [TestMethod]
        public void Add_WithTargetUnit_Pound()
        {
            var kg = new QuantityWeight(1, WeightUnit.Kilogram);
            var g = new QuantityWeight(1000, WeightUnit.Gram);

            var result = QuantityWeight.Add(
                kg,
                g,
                WeightUnit.Pound);

            Assert.AreEqual(4.40924, result.Value, 1e-5);
            Assert.AreEqual(WeightUnit.Pound, result.Unit);
        }

        [TestMethod]
        public void Addition_ShouldBeCommutative()
        {
            var a = new QuantityWeight(1, WeightUnit.Kilogram);
            var b = new QuantityWeight(1000, WeightUnit.Gram);

            var result1 = a.Add(b);
            var result2 = b.Add(a);

            Assert.IsTrue(result1.Equals(result2));
        }

        [TestMethod]
        public void Add_ZeroWeight()
        {
            var a = new QuantityWeight(5, WeightUnit.Kilogram);
            var zero = new QuantityWeight(0, WeightUnit.Kilogram);

            var result = a.Add(zero);

            Assert.AreEqual(5, result.Value, TOLERANCE);
        }

        [TestMethod]
        public void Add_NegativeWeight()
        {
            var a = new QuantityWeight(5, WeightUnit.Kilogram);
            var b = new QuantityWeight(-2, WeightUnit.Kilogram);

            var result = a.Add(b);

            Assert.AreEqual(3, result.Value, TOLERANCE);
        }

        [TestMethod]
        public void Weight_ShouldNotEqual_Length()
        {
            var weight = new QuantityWeight(1, WeightUnit.Kilogram);
            var length = new QuantityLength(1, LengthUnit.Feet);

            Assert.IsFalse(weight.Equals(length));
        }
    }
}