using System;
using QuantityMeasurementModel;
using QuantityMeasurementService;

namespace QuantityMeasurementApp
{
    // Controller class that handles the user interface and console interaction
    // Acts as the presentation layer in the N-Tier architecture
    public class QuantityMeasurementController
    {
        // Service reference injected via constructor for business logic processing
        private readonly IQuantityMeasurementService _service;

        // Dependency Injection via constructor to decouple controller from service
        public QuantityMeasurementController(IQuantityMeasurementService service)
        {
            _service = service;
        }

        // Initializes the application and displays the main menu to the user
        public void InitializeApplication()
        {
            Console.WriteLine("Welcome to Quantity Measurement!");
            Console.WriteLine("Type Of Units Available : \n1. Weight\n2. Length\n3. Volume\n4. Temperature");

            // Read user choice for measurement category
            int choice;
            bool validChoice = int.TryParse(Console.ReadLine(), out choice);
            if (!validChoice)
            {
                return;
            }

            // Create the request DTO and set the measurement category
            MeasurementRequestDTO request = new MeasurementRequestDTO();

            // Map the numeric choice to the category string using if-else
            if (choice == 1)
            {
                request.MeasurementCategory = "Weight";
            }
            else if (choice == 2)
            {
                request.MeasurementCategory = "Length";
            }
            else if (choice == 3)
            {
                request.MeasurementCategory = "Volume";
            }
            else if (choice == 4)
            {
                request.MeasurementCategory = "Temperature";
            }
            else
            {
                request.MeasurementCategory = "Unknown";
            }

            // Exit if the category is not recognized
            if (request.MeasurementCategory == "Unknown")
            {
                return;
            }

            // Display available operations for the selected category
            Console.WriteLine("\n" + request.MeasurementCategory + " Operations");
            if (choice != 4)
            {
                Console.WriteLine("1. Compare\n2. Add\n3. Subtract\n4. Divide");
            }
            else
            {
                Console.WriteLine("1. Compare\n2. Add\n3. Subtract");
            }

            // Read and parse the selected operation type
            Console.Write("\nSelect Operation: ");
            int operationChoice = int.Parse(Console.ReadLine());
            request.OperationType = (MeasurementAction)operationChoice;

            // Read the first measurement value and unit
            Console.Write("Enter First Value: ");
            request.MeasurementValue1 = double.Parse(Console.ReadLine());
            Console.Write("Enter First Unit (e.g., FEET, KILOGRAM): ");
            request.MeasurementUnit1 = Console.ReadLine();

            // Read the second measurement value and unit
            Console.Write("Enter Second Value: ");
            request.MeasurementValue2 = double.Parse(Console.ReadLine());
            Console.Write("Enter Second Unit (e.g., INCH, GRAM): ");
            request.MeasurementUnit2 = Console.ReadLine();

            // Read the target unit only for arithmetic operations
            if (request.OperationType != MeasurementAction.Compare)
            {
                Console.Write("Enter Target Unit: ");
                request.TargetMeasurementUnit = Console.ReadLine();
            }

            // Ship the DTO to the Service Layer for processing
            MeasurementResponseDTO response = _service.ProcessMeasurement(request);

            // Display the Result based on the response type
            if (!response.IsSuccess)
            {
                Console.WriteLine("\nError: " + response.ErrorMessage);
            }
            else if (response.IsComparison)
            {
                Console.WriteLine("\nEquality Result: " + response.AreEqual);
            }
            else
            {
                Console.WriteLine("\nRESULT: " + response.FormattedMessage);
            }
        }
    }
}