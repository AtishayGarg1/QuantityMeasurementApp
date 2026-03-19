using System;
using QuantityMeasurementModel;
using QuantityMeasurementModel.Entities;
using QuantityMeasurementRepository.Interfaces;

namespace QuantityMeasurementService
{
    // Orchestrates business logic and routes measurement requests safely
    // Acts as the service layer between the controller and repository
    public class QuantityMeasurementServices : IQuantityMeasurementService
    {
        // Repository reference injected via constructor for data persistence
        private readonly IQuantityMeasurementRepository _repository;

        // Constructor accepts a repository implementation via dependency injection
        public QuantityMeasurementServices(IQuantityMeasurementRepository repository)
        {
            _repository = repository;
        }

        // Validates that the measurement value is not negative or infinite
        public void ValidateValue(double checkValue)
        {
            if (double.IsNegative(checkValue) || double.IsInfinity(checkValue))
            {
                throw new InvalidMeasurementException("The measurement value " + checkValue + " is invalid.");
            }
        }

        // Processes a measurement request and returns the response DTO
        // Routes to the correct category handler based on the measurement category string
        public MeasurementResponseDTO ProcessMeasurement(MeasurementRequestDTO request)
        {
            MeasurementResponseDTO response = null;
            try
            {
                // Determine the measurement category and route to the correct processor
                string category = request.MeasurementCategory.ToLower();

                if (category == "length")
                {
                    response = ProcessCategory<LengthUnit>(request, LengthConverter.Instance);
                }
                else if (category == "volume")
                {
                    response = ProcessCategory<VolumeUnit>(request, VolumeConverter.Instance);
                }
                else if (category == "weight")
                {
                    response = ProcessCategory<WeightUnit>(request, WeightConverter.Instance);
                }
                else if (category == "temperature")
                {
                    response = ProcessCategory<TemperatureUnit>(request, TemperatureConverter.Instance);
                }
                else
                {
                    throw new ArgumentException("Invalid category");
                }
            }
            catch (Exception ex)
            {
                // Wrap any exception into a failed response DTO
                response = new MeasurementResponseDTO();
                response.IsSuccess = false;
                response.ErrorMessage = ex.Message;
            }

            // Build the measurement entity from request and response data
            MeasurementEntity entity = new MeasurementEntity();
            entity.MeasurementCategory = request.MeasurementCategory;
            entity.OperationType = request.OperationType.ToString();
            entity.MeasurementUnit1 = request.MeasurementUnit1;
            entity.MeasurementValue1 = request.MeasurementValue1;
            entity.MeasurementUnit2 = request.MeasurementUnit2;
            entity.MeasurementValue2 = request.MeasurementValue2;
            entity.TargetMeasurementUnit = request.TargetMeasurementUnit;
            entity.IsSuccess = response.IsSuccess;
            entity.ErrorMessage = response.ErrorMessage;
            entity.IsComparison = response.IsComparison;
            entity.AreEqual = response.AreEqual;
            entity.CalculatedValue = response.CalculatedValue;
            entity.FormattedMessage = response.FormattedMessage;
            entity.CreatedAt = DateTime.Now;

            // Attempt to save the entity to the repository (database or cache)
            try
            {
                _repository.SaveMeasurement(entity);
            }
            catch (Exception dbEx)
            {
                // Log warning but do not fail the operation if save fails
                Console.WriteLine("Warning: Could not save to repository. " + dbEx.Message);
            }

            return response;
        }

        // Generic method that parses units and executes the requested operation
        // Works with any unit type (Length, Volume, Weight, Temperature) via generics
        private MeasurementResponseDTO ProcessCategory<TUnit>(MeasurementRequestDTO req, IMeasurable<TUnit> converter) where TUnit : struct, Enum
        {
            // Validate both input measurement values
            ValidateValue(req.MeasurementValue1);
            ValidateValue(req.MeasurementValue2);

            // Parse the unit strings into their enum equivalents
            TUnit u1;
            TUnit u2;
            bool parsed1 = Enum.TryParse(req.MeasurementUnit1, true, out u1);
            bool parsed2 = Enum.TryParse(req.MeasurementUnit2, true, out u2);

            if (!parsed1 || !parsed2)
            {
                throw new ArgumentException("Invalid unit provided.");
            }

            // Create Quantity objects for both measurements
            Quantity<TUnit> q1 = new Quantity<TUnit>(req.MeasurementValue1, u1, converter);
            Quantity<TUnit> q2 = new Quantity<TUnit>(req.MeasurementValue2, u2, converter);

            // Handle comparison operation
            if (req.OperationType == MeasurementAction.Compare)
            {
                MeasurementResponseDTO compareResponse = new MeasurementResponseDTO();
                compareResponse.IsSuccess = true;
                compareResponse.IsComparison = true;
                compareResponse.AreEqual = q1.Equals(q2);
                return compareResponse;
            }

            // Parse the target unit for arithmetic operations
            TUnit targetUnit;
            bool parsedTarget = Enum.TryParse(req.TargetMeasurementUnit, true, out targetUnit);
            if (!parsedTarget)
            {
                throw new ArgumentException("Invalid target unit provided.");
            }

            // Temperature does not support arithmetic operations
            if (typeof(TUnit) == typeof(TemperatureUnit))
            {
                throw new InvalidMeasurementException("Arithmetic operations (Add/Subtract/Divide) are not supported for Temperature.");
            }

            // Perform the arithmetic operation based on the operation type
            Quantity<TUnit> result;
            if (req.OperationType == MeasurementAction.Add)
            {
                result = q1.Add(q2, targetUnit);
            }
            else if (req.OperationType == MeasurementAction.Subtract)
            {
                result = q1.Subtract(q2, targetUnit);
            }
            else if (req.OperationType == MeasurementAction.Divide)
            {
                result = q1.Division(q2, targetUnit);
            }
            else
            {
                throw new ArgumentException("Invalid operation");
            }

            // Determine the arithmetic symbol for the formatted message
            string symbol;
            if (req.OperationType == MeasurementAction.Add)
            {
                symbol = "+";
            }
            else if (req.OperationType == MeasurementAction.Subtract)
            {
                symbol = "-";
            }
            else
            {
                symbol = "/";
            }

            // Build and return the arithmetic response DTO
            MeasurementResponseDTO arithmeticResponse = new MeasurementResponseDTO();
            arithmeticResponse.IsSuccess = true;
            arithmeticResponse.IsComparison = false;
            arithmeticResponse.CalculatedValue = result.ConvertTo(targetUnit);
            arithmeticResponse.FormattedMessage = req.MeasurementValue1 + " " + u1 + " " + symbol + " " + req.MeasurementValue2 + " " + u2 + " = " + result.ConvertTo(targetUnit) + " " + targetUnit;

            return arithmeticResponse;
        }
    }
}