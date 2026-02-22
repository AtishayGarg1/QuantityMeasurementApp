using System;
using QuantityMeasurement.Library.Model;

namespace QuantityMeasurement.App
{
    class Program
    {
        static void Main()
        {
            var length1 = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var length2 = new Quantity<LengthUnit>(12, LengthUnit.Inch);

            var lengthResult = length1.Add(length2);
            Console.WriteLine(lengthResult);

            var weight1 = new Quantity<WeightUnit>(1, WeightUnit.Kilogram);
            var weight2 = new Quantity<WeightUnit>(1000, WeightUnit.Gram);

            var weightResult = weight1.Add(weight2);
            Console.WriteLine(weightResult);
        }
    }
}