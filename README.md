Quantity Measurement Application
Project Overview
The Quantity Measurement Application is a robust C# console application designed to handle, validate, and compare physical quantity measurements. Built with Clean Architecture and SOLID principles, the application ensures that measurements are mathematically valid and can be seamlessly compared across different units (e.g., checking if 1 Foot equals 12 Inches).

The core domain relies on Value Objects and centralized validation to guarantee that invalid states (like negative distances or infinite values) cannot exist within the system.

Features
Value Equality Comparison: Compares measurement objects based on their actual physical values rather than memory references.

Cross-Unit Conversion: Seamlessly compares quantities of different units using a scalable base-unit conversion architecture.

Strict Domain Validation: Automatically rejects invalid physical measurements, including negative numbers and mathematical Infinity.

Custom Exception Handling: Utilizes domain-specific exceptions (InvalidMeasurementException) for precise error reporting and easier debugging.

Open/Closed Design: New measurement units can be added with a single line of code without modifying existing comparison or validation logic.

Tech Stack
Language: C#

Framework: .NET (Console Application)

How to Run
Prerequisites: Ensure you have the .NET SDK installed on your machine.

Clone the repository:
    git clone <your-repository-url>
    cd QuantityMeasurementApp

Build the application:
    dotnet build

Run the application:
    dotnet run --project Com.Apps.QuantityMeasurement