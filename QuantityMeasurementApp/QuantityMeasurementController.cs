using System;
using QuantityMeasurementModel;
using QuantityMeasurementService;

namespace QuantityMeasurementApp
{
    public class QuantityMeasurementController
    {
        private readonly IQuantityMeasurementService _service;

        // Dependency Injection via constructor
        public QuantityMeasurementController(IQuantityMeasurementService service)
        {
            _service = service;
        }

        public void InitializeApplication()
        {
            Console.WriteLine("Welcome to Quantity Measurement!");
            Console.WriteLine("Type Of Units Available : \n1. Weight\n2. Length\n3. Volume\n4. Temperature");

            if (!int.TryParse(Console.ReadLine(), out int choice)) return;

            var request = new MeasurementRequestDTO();
            request.MeasurementCategory = choice switch { 1 => "Weight", 2 => "Length", 3 => "Volume", 4 => "Temperature", _ => "Unknown" };
            if (request.MeasurementCategory == "Unknown") return;

            Console.WriteLine($"\n{request.MeasurementCategory} Operations");
            Console.WriteLine("1. Compare\n2. Add\n3. Subtract" + (choice != 4 ? "\n4. Divide" : ""));
            Console.Write("\nSelect Operation: ");
            request.OperationType = (MeasurementAction)int.Parse(Console.ReadLine());

            Console.Write("Enter First Value: ");
            request.MeasurementValue1 = double.Parse(Console.ReadLine());
            Console.Write("Enter First Unit (e.g., FEET, KILOGRAM): ");
            request.MeasurementUnit1 = Console.ReadLine();

            Console.Write("Enter Second Value: ");
            request.MeasurementValue2 = double.Parse(Console.ReadLine());
            Console.Write("Enter Second Unit (e.g., INCH, GRAM): ");
            request.MeasurementUnit2 = Console.ReadLine();

            if (request.OperationType != MeasurementAction.Compare)
            {
                Console.Write("Enter Target Unit: ");
                request.TargetMeasurementUnit = Console.ReadLine();
            }

            // Ship the DTO to the Service Layer
            var response = _service.ProcessMeasurement(request);

            // Display the Result
            if (!response.IsSuccess)
            {
                Console.WriteLine($"\nError: {response.ErrorMessage}");
            }
            else if (response.IsComparison)
            {
                Console.WriteLine($"\nEquality Result: {response.AreEqual}");
            }
            else
            {
                Console.WriteLine($"\nRESULT: {response.FormattedMessage}");
            }
        }
    }
}