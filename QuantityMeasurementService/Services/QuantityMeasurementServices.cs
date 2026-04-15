using System;
using System.Collections.Generic;
using QuantityMeasurementModel;
using QuantityMeasurementModel.Entities;
using QuantityMeasurementRepository.Interfaces;

namespace QuantityMeasurementService
{
    public class QuantityMeasurementServices : IQuantityMeasurementService
    {
        private readonly IQuantityMeasurementRepository _repository;
        private readonly Dictionary<string, IMeasurable> _converters;

        public QuantityMeasurementServices(IQuantityMeasurementRepository repository)
        {
            _repository = repository;
            _converters = new Dictionary<string, IMeasurable>(StringComparer.OrdinalIgnoreCase)
            {
                { "length", LengthConverter.Instance },
                { "volume", VolumeConverter.Instance },
                { "weight", WeightConverter.Instance },
                { "temperature", TemperatureConverter.Instance }
            };
        }

        public MeasurementResponseDTO ProcessMeasurement(MeasurementRequestDTO request, string userId)
        {
            var response = ExecuteMeasurement(request);
            PersistResult(request, response, userId);
            return response;
        }

        public void CheckValue(double val)
        {
            if (double.IsNegative(val) || double.IsInfinity(val))
            {
                throw new InvalidMeasurementException($"Value '{val}' is invalid for calculations.");
            }
        }

        public List<MeasurementEntity> GetMeasurementHistory(string userId) => _repository.GetAllMeasurements(userId);
        public MeasurementEntity GetMeasurementById(int id, string userId) => _repository.GetMeasurementById(id, userId);
        public bool DeleteMeasurement(int id, string userId) => _repository.DeleteMeasurement(id, userId);
        public List<MeasurementEntity> GetMeasurementsByCategory(string category, string userId) => _repository.GetByCategory(category, userId);

        private MeasurementResponseDTO ExecuteMeasurement(MeasurementRequestDTO request)
        {
            try
            {
                // Selection of operation handled below with appropriate value checks

                if (!_converters.TryGetValue(request.MeasurementCategory, out var converter))
                {
                    throw new ArgumentException("Invalid category");
                }

                if (request.OperationType == MeasurementAction.Compare)
                {
                    CheckValue(request.MeasurementValue1);
                    CheckValue(request.MeasurementValue2);
                    return PerformComparison(request, converter);
                }

                if (request.OperationType == MeasurementAction.Convert)
                {
                    CheckValue(request.MeasurementValue1);
                    return PerformConversion(request, converter);
                }

                CheckValue(request.MeasurementValue1);
                CheckValue(request.MeasurementValue2);
                return PerformArithmetic(request, converter);
            }
            catch (Exception ex)
            {
                return new MeasurementResponseDTO { IsSuccess = false, ErrorMessage = ex.Message };
            }
        }

        private MeasurementResponseDTO PerformComparison(MeasurementRequestDTO req, IMeasurable converter)
        {
            if (string.IsNullOrEmpty(req.MeasurementUnit2))
            {
                 throw new ArgumentException("Second unit is required for comparison.");
            }
            var v1 = converter.ToBaseUnit(req.MeasurementUnit1, req.MeasurementValue1);
            var v2 = converter.ToBaseUnit(req.MeasurementUnit2, req.MeasurementValue2);
            
            // Allow for small rounding drift
            bool areEqual = Math.Abs(v1 - v2) < 0.00001; 

            return new MeasurementResponseDTO
            {
                IsSuccess = true,
                IsComparison = true,
                AreEqual = areEqual
            };
        }

        private MeasurementResponseDTO PerformConversion(MeasurementRequestDTO req, IMeasurable converter)
        {
            double baseValue = converter.ToBaseUnit(req.MeasurementUnit1, req.MeasurementValue1);
            double convertedResult = converter.FromBaseUnit(req.TargetMeasurementUnit, baseValue);

            return new MeasurementResponseDTO
            {
                IsSuccess = true,
                IsComparison = false,
                CalculatedValue = convertedResult,
                FormattedMessage = $"{req.MeasurementValue1} {req.MeasurementUnit1} = {convertedResult} {req.TargetMeasurementUnit}"
            };
        }

        private MeasurementResponseDTO PerformArithmetic(MeasurementRequestDTO req, IMeasurable converter)
        {
            if (req.MeasurementCategory.Equals("Temperature", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidMeasurementException("Arithmetic operations (Add/Subtract/Divide) are not supported for Temperature.");
            }

            if (string.IsNullOrEmpty(req.MeasurementUnit2))
            {
                throw new ArgumentException("Second unit is required for arithmetic operations.");
            }

            double base1 = converter.ToBaseUnit(req.MeasurementUnit1, req.MeasurementValue1);
            double base2 = converter.ToBaseUnit(req.MeasurementUnit2, req.MeasurementValue2);

            double baseResult = 0;
            string symbol = "";

            if (req.OperationType == MeasurementAction.Add)
            {
                baseResult = base1 + base2;
                symbol = "+";
            }
            else if (req.OperationType == MeasurementAction.Subtract)
            {
                baseResult = base1 - base2;
                symbol = "-";
            }
            else if (req.OperationType == MeasurementAction.Divide)
            {
                baseResult = base1 / base2;
                symbol = "/";
            }
            else
            {
                throw new ArgumentException("Invalid operation");
            }

            double convertedResult = converter.FromBaseUnit(req.TargetMeasurementUnit, baseResult);

            return new MeasurementResponseDTO
            {
                IsSuccess = true,
                IsComparison = false,
                CalculatedValue = convertedResult,
                FormattedMessage = $"{req.MeasurementValue1} {req.MeasurementUnit1} {symbol} {req.MeasurementValue2} {req.MeasurementUnit2} = {convertedResult} {req.TargetMeasurementUnit}"
            };
        }

        private void PersistResult(MeasurementRequestDTO request, MeasurementResponseDTO response, string userId)
        {
            try
            {
                var entity = new MeasurementEntity
                {
                    UserId = userId,
                    MeasurementCategory = request.MeasurementCategory ?? "",
                    OperationType = request.OperationType.ToString(),
                    MeasurementUnit1 = request.MeasurementUnit1 ?? "",
                    MeasurementValue1 = request.MeasurementValue1,
                    MeasurementUnit2 = request.MeasurementUnit2 ?? "",
                    MeasurementValue2 = request.MeasurementValue2,
                    TargetMeasurementUnit = request.TargetMeasurementUnit ?? "",
                    IsSuccess = response.IsSuccess,
                    ErrorMessage = response.ErrorMessage ?? "",
                    IsComparison = response.IsComparison,
                    AreEqual = response.AreEqual,
                    CalculatedValue = response.CalculatedValue,
                    FormattedMessage = response.FormattedMessage ?? "",
                    CreatedAt = DateTime.UtcNow
                };
                _repository.SaveMeasurement(entity);
            }
            catch (Exception dbEx)
            {
                Console.WriteLine("Warning: Could not save to repository. " + dbEx.Message);
            }
        }
    }
}