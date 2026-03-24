using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using QuantityMeasurementModel;
using QuantityMeasurementModel.Entities;
using QuantityMeasurementService.Core;

namespace QuantityMeasurementApp.Controllers
{
    // REST controller for measurement operations
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class MeasurementsController : ControllerBase
    {
        private readonly IQuantityMeasurementService _service;

        public MeasurementsController(IQuantityMeasurementService service)
        {
            _service = service;
        }

        // POST /api/measurements/calculate
        [HttpPost("calculate")]
        public IActionResult Calculate([FromBody] MeasurementRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BuildValidationError();
            }

            MeasurementResponseDTO result = _service.ProcessMeasurement(request);
            if (!result.IsSuccess)
            {
                return BadRequest(ApiErrorResponse.Create(400, "Measurement failed", result.ErrorMessage));
            }
            return Ok(result);
        }

        // POST /api/measurements/compare
        [HttpPost("compare")]
        public IActionResult Compare([FromBody] MeasurementRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BuildValidationError();
            }

            request.OperationType = MeasurementAction.Compare;
            MeasurementResponseDTO result = _service.ProcessMeasurement(request);

            if (!result.IsSuccess)
            {
                return BadRequest(ApiErrorResponse.Create(400, "Comparison failed", result.ErrorMessage));
            }
            return Ok(result);
        }

        // GET /api/measurements/history
        [HttpGet("history")]
        public IActionResult GetHistory()
        {
            List<MeasurementEntity> records = _service.GetMeasurementHistory();
            return Ok(records);
        }

        [HttpGet]
        public IActionResult GetAll(){
            return Ok("Hello");
        }

        // GET /api/measurements/history/{id}
        [HttpGet("history/{id}")]
        public IActionResult GetById(int id)
        {
            MeasurementEntity entity = _service.GetMeasurementById(id);
            if (entity == null)
            {
                return NotFound(ApiErrorResponse.Create(404, "Not found", "No measurement with ID " + id));
            }
            return Ok(entity);
        }

        // DELETE /api/measurements/history/{id}
        [HttpDelete("history/{id}")]
        public IActionResult Delete(int id)
        {
            bool deleted = _service.DeleteMeasurement(id);
            if (!deleted)
            {
                return NotFound(ApiErrorResponse.Create(404, "Not found", "No measurement with ID " + id));
            }
            return NoContent();
        }

        // GET /api/measurements/history/category/{category}
        [HttpGet("history/category/{category}")]
        public IActionResult GetByCategory(string category)
        {
            List<MeasurementEntity> records = _service.GetMeasurementsByCategory(category);
            return Ok(records);
        }

        // GET /api/measurements/units
        [HttpGet("units")]
        public IActionResult GetSupportedUnits()
        {
            var units = new
            {
                Length = new string[] { "INCH", "FEET", "YARD", "CENTIMETRE" },
                Weight = new string[] { "KILOGRAM", "GRAM", "POUND" },
                Volume = new string[] { "LITRE", "MILLILITRE", "GALLON" },
                Temperature = new string[] { "CELSIUS", "FAHRENHEIT", "KELVIN" }
            };
            return Ok(units);
        }

        // Build 400 response from validation errors
        private IActionResult BuildValidationError()
        {
            string errors = string.Join("; ", GetModelErrors());
            return BadRequest(ApiErrorResponse.Create(400, "Validation failed", errors));
        }

        // Extract error messages from ModelState
        private List<string> GetModelErrors()
        {
            List<string> errorList = new List<string>();
            foreach (var entry in ModelState)
            {
                foreach (var error in entry.Value.Errors)
                {
                    errorList.Add(error.ErrorMessage);
                }
            }
            return errorList;
        }
    }
}
