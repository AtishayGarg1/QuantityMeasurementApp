using System;
using QuantityMeasurementModel;

namespace QuantityMeasurementService
{
    // Orchestrates business logic and routes requests safely
    public class QuantityMeasurementServices : IQuantityMeasurementService
    {
        // Validates floats
        public void ValidateValue(double checkValue)
        {
            if (double.IsNegative(checkValue) || double.IsInfinity(checkValue))
                throw new InvalidMeasurementException($"The measurement value {checkValue} is invalid.");
        }

        public MeasurementResponseDTO ProcessMeasurement(MeasurementRequestDTO request)
        {
            try
            {
                // Route to the correct generic method based on the category string
                return request.MeasurementCategory.ToLower() switch
                {
                    "length" => ProcessCategory<LengthUnit>(request, LengthConverter.Instance),
                    "volume" => ProcessCategory<VolumeUnit>(request, VolumeConverter.Instance),
                    "weight" => ProcessCategory<WeightUnit>(request, WeightConverter.Instance),
                    "temperature" => ProcessCategory<TemperatureUnit>(request, TemperatureConverter.Instance),
                    _ => throw new ArgumentException("Invalid category")
                };
            }
            catch (Exception ex)
            {
                return new MeasurementResponseDTO { IsSuccess = false, ErrorMessage = ex.Message };
            }
        }

        // Parses generic boundaries and executes math
        private MeasurementResponseDTO ProcessCategory<TUnit>(MeasurementRequestDTO req, IMeasurable<TUnit> converter) where TUnit : struct, Enum
        {
            ValidateValue(req.MeasurementValue1);
            ValidateValue(req.MeasurementValue2);

            if (!Enum.TryParse(req.MeasurementUnit1, true, out TUnit u1) || !Enum.TryParse(req.MeasurementUnit2, true, out TUnit u2))
                throw new ArgumentException("Invalid unit provided.");

            var q1 = new Quantity<TUnit>(req.MeasurementValue1, u1, converter);
            var q2 = new Quantity<TUnit>(req.MeasurementValue2, u2, converter);

            if (req.OperationType == MeasurementAction.Compare)
            {
                return new MeasurementResponseDTO
                {
                    IsSuccess = true,
                    IsComparison = true,
                    AreEqual = q1.Equals(q2)
                };
            }

            // If it's Arithmetic, parse the Target Unit
            if (!Enum.TryParse(req.TargetMeasurementUnit, true, out TUnit targetUnit))
            {
                throw new ArgumentException("Invalid target unit provided.");
            }

            if (typeof(TUnit) == typeof(TemperatureUnit))
                throw new InvalidMeasurementException("Arithmetic operations (Add/Subtract/Divide) are not supported for Temperature.");

            Quantity<TUnit> result = req.OperationType switch
            {
                MeasurementAction.Add => q1.Add(q2, targetUnit),
                MeasurementAction.Subtract => q1.Subtract(q2, targetUnit),
                MeasurementAction.Divide => q1.Division(q2, targetUnit),
                _ => throw new ArgumentException("Invalid operation")
            };

            string symbol = req.OperationType switch { MeasurementAction.Add => "+", MeasurementAction.Subtract => "-", _ => "/" };

            return new MeasurementResponseDTO
            {
                IsSuccess = true,
                IsComparison = false,
                CalculatedValue = result.ConvertTo(targetUnit),
                FormattedMessage = $"{req.MeasurementValue1} {u1} {symbol} {req.MeasurementValue2} {u2} = {result.ConvertTo(targetUnit)} {targetUnit}"
            };
        }
    }
}