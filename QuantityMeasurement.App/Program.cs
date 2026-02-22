using System;
using QuantityMeasurement.Library.Model;
using QuantityMeasurement.Library.Service;

namespace QuantityMeasurement.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new QuantityMeasurementService();

            // take user input and assign the proper unit to them
            Console.Write("Enter first value: ");
            double value1 = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter unit (Feet/Inch/Yard/Centimeter): ");
            LengthUnit unit1 = Enum.Parse<LengthUnit>(Console.ReadLine(), true);

            Console.Write("Enter second value: ");
            double value2 = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter unit (Feet/Inch/Yard/Centimeter): ");
            LengthUnit unit2 = Enum.Parse<LengthUnit>(Console.ReadLine(), true);

            QuantityLength length1 = new QuantityLength(value1, unit1);
            QuantityLength length2 = new QuantityLength(value2, unit2);

            bool result = service.CompareLength(length1, length2);

            Console.WriteLine($"Equal: {result}");
        }
    }
}