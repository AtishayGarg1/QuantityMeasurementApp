using System;
using QuantityMeasurementService;

namespace QuantityMeasurementApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Create the service
            IQuantityMeasurementService appService = new QuantityMeasurementServices();
            QuantityMeasurementController applicationController = new QuantityMeasurementController(appService);
            // Start the application
            applicationController.InitializeApplication();
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}