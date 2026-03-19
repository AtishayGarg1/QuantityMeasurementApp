using System;
using QuantityMeasurementService;
using QuantityMeasurementRepository.Repositories;

namespace QuantityMeasurementApp
{
    // Entry point of the Quantity Measurement Application
    // Configures the repository (cache or database) and starts the controller
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Select Repository Mode:");
            Console.WriteLine("1. Cache (In-Memory)");
            Console.WriteLine("2. Database (SQL Server)");
            Console.Write("Enter choice: ");

            // Read user selection for storage mode
            string input = Console.ReadLine();
            int repositoryChoice;
            bool parsed = int.TryParse(input, out repositoryChoice);

            if (!parsed)
            {
                Console.WriteLine("Invalid input. Defaulting to Cache mode.");
                repositoryChoice = 1;
            }

            // Declare the repository interface reference
            QuantityMeasurementRepository.Interfaces.IQuantityMeasurementRepository repository;

            // Choose between cache and database repository based on user input
            if (repositoryChoice == 2)
            {
                // Set up SQL Server connection string for database persistence
                string connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=QuantityMeasurementDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";
                repository = new QuantityMeasurementDatabaseRepository(connectionString);
                Console.WriteLine("Using Database Repository (SQL Server).\n");
            }
            else
            {
                // Use in-memory cache repository for lightweight storage
                repository = new QuantityMeasurementCacheRepository();
                Console.WriteLine("Using Cache Repository (In-Memory).\n");
            }

            // Create the service layer with the selected repository
            IQuantityMeasurementService appService = new QuantityMeasurementServices(repository);

            // Create the controller and initialize the application
            QuantityMeasurementController applicationController = new QuantityMeasurementController(appService);
            applicationController.InitializeApplication();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}