using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurement.Library.Model;
using System;

namespace QuantityMeasurement.Tests
{
    [TestClass]
    public class QuantityTests
    {
        private const double TOLERANCE = 1e-6;

        // LENGTH TESTS

        [TestMethod]
        public void Length_Equality_CrossUnit()
        {
            var a = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(12, LengthUnit.Inch);

            Assert.IsTrue(a.Equals(b));
        }

        [TestMethod]
        public void Length_Conversion()
        {
            var length = new Quantity<LengthUnit>(3, LengthUnit.Feet);
            var result = length.ConvertTo(LengthUnit.Yard);

            Assert.AreEqual(1, result.Value, TOLERANCE);
        }

        [TestMethod]
        public void Length_Addition()
        {
            var a = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(12, LengthUnit.Inch);

            var result = a.Add(b);

            Assert.AreEqual(2, result.Value, TOLERANCE);
        }


        // WEIGHT TESTS

        [TestMethod]
        public void Weight_Equality_CrossUnit()
        {
            var a = new Quantity<WeightUnit>(1, WeightUnit.Kilogram);
            var b = new Quantity<WeightUnit>(1000, WeightUnit.Gram);

            Assert.IsTrue(a.Equals(b));
        }

        [TestMethod]
        public void Weight_Conversion()
        {
            var weight = new Quantity<WeightUnit>(1, WeightUnit.Kilogram);
            var result = weight.ConvertTo(WeightUnit.Pound);

            Assert.AreEqual(2.20462, result.Value, 1e-5);
        }

        [TestMethod]
        public void Weight_Addition()
        {
            var a = new Quantity<WeightUnit>(1, WeightUnit.Kilogram);
            var b = new Quantity<WeightUnit>(1000, WeightUnit.Gram);

            var result = a.Add(b);

            Assert.AreEqual(2, result.Value, TOLERANCE);
        }

        // VOLUME TESTS

        [TestMethod]
        public void Volume_Equality_Litre_Millilitre()
        {
            var a = new Quantity<VolumeUnit>(1, VolumeUnit.Litre);
            var b = new Quantity<VolumeUnit>(1000, VolumeUnit.Millilitre);

            Assert.IsTrue(a.Equals(b));
        }

        [TestMethod]
        public void Volume_Equality_Litre_Gallon()
        {
            var a = new Quantity<VolumeUnit>(3.78541, VolumeUnit.Litre);
            var b = new Quantity<VolumeUnit>(1, VolumeUnit.Gallon);

            Assert.IsTrue(a.Equals(b));
        }

        [TestMethod]
        public void Volume_Conversion()
        {
            var gallon = new Quantity<VolumeUnit>(1, VolumeUnit.Gallon);
            var result = gallon.ConvertTo(VolumeUnit.Litre);

            Assert.AreEqual(3.78541, result.Value, TOLERANCE);
        }

        [TestMethod]
        public void Volume_Addition()
        {
            var litre = new Quantity<VolumeUnit>(1, VolumeUnit.Litre);
            var ml = new Quantity<VolumeUnit>(1000, VolumeUnit.Millilitre);

            var result = litre.Add(ml);

            Assert.AreEqual(2, result.Value, TOLERANCE);
        }

        // MATHEMATICAL PROPERTIES

        [TestMethod]
        public void Commutativity_Volume()
        {
            var a = new Quantity<VolumeUnit>(1, VolumeUnit.Litre);
            var b = new Quantity<VolumeUnit>(1000, VolumeUnit.Millilitre);

            var r1 = a.Add(b);
            var r2 = b.Add(a);

            Assert.IsTrue(r1.Equals(r2));
        }

        [TestMethod]
        public void Transitive_Property_Volume()
        {
            var a = new Quantity<VolumeUnit>(1, VolumeUnit.Litre);
            var b = new Quantity<VolumeUnit>(1000, VolumeUnit.Millilitre);
            var c = new Quantity<VolumeUnit>(1, VolumeUnit.Litre);

            Assert.IsTrue(a.Equals(b));
            Assert.IsTrue(b.Equals(c));
            Assert.IsTrue(a.Equals(c));
        }

        [TestMethod]
        public void RoundTrip_Volume()
        {
            var original = new Quantity<VolumeUnit>(1.5, VolumeUnit.Litre);
            var converted = original
                .ConvertTo(VolumeUnit.Millilitre)
                .ConvertTo(VolumeUnit.Litre);

            Assert.AreEqual(original.Value, converted.Value, TOLERANCE);
        }

        // EDGE CASES

        [TestMethod]
        public void Zero_Value()
        {
            var a = new Quantity<VolumeUnit>(0, VolumeUnit.Litre);
            var b = new Quantity<VolumeUnit>(0, VolumeUnit.Millilitre);

            Assert.IsTrue(a.Equals(b));
        }

        [TestMethod]
        public void Negative_Value()
        {
            var a = new Quantity<VolumeUnit>(-1, VolumeUnit.Litre);
            var b = new Quantity<VolumeUnit>(-1000, VolumeUnit.Millilitre);

            Assert.IsTrue(a.Equals(b));
        }

        [TestMethod]
        public void Large_Value_Addition()
        {
            var a = new Quantity<VolumeUnit>(1_000_000, VolumeUnit.Litre);
            var b = new Quantity<VolumeUnit>(1_000_000, VolumeUnit.Litre);

            var result = a.Add(b);

            Assert.AreEqual(2_000_000, result.Value, TOLERANCE);
        }

        // CROSS CATEGORY SAFETY

        [TestMethod]
        public void Volume_NotEqual_Length()
        {
            var volume = new Quantity<VolumeUnit>(1, VolumeUnit.Litre);
            var length = new Quantity<LengthUnit>(1, LengthUnit.Feet);

            Assert.IsFalse(volume.Equals(length));
        }

        [TestMethod]
        public void Volume_NotEqual_Weight()
        {
            var volume = new Quantity<VolumeUnit>(1, VolumeUnit.Litre);
            var weight = new Quantity<WeightUnit>(1, WeightUnit.Kilogram);

            Assert.IsFalse(volume.Equals(weight));
        }

        // NULL HANDLING
        
        [TestMethod]
        public void Equals_Null_ReturnsFalse()
        {
            var a = new Quantity<VolumeUnit>(1, VolumeUnit.Litre);

            Assert.IsFalse(a.Equals(null));
        }
    }
}