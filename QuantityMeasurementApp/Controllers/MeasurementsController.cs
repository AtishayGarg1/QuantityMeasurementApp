using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuantityMeasurementModel;
using QuantityMeasurementModel.Entities;
using QuantityMeasurementService;

namespace QuantityMeasurementApp.Controllers
{
    // REST controller for measurement operations
    [Authorize]
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

        [AllowAnonymous]
        [HttpPost("calculate")]
        public IActionResult RunCalculation([FromBody] MeasurementRequestDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(ApiErrorResponse.Create(400, "Validation failed", errors));
            }

            var res = _service.ProcessMeasurement(dto);
            return res.IsSuccess ? Ok(res) : BadRequest(ApiErrorResponse.Create(400, "Operation Failed", res.ErrorMessage));
        }

        [AllowAnonymous]
        [HttpPost("compare")]
        public IActionResult DoComparison([FromBody] MeasurementRequestDTO req)
        {
            if (!ModelState.IsValid) return BadRequest("Input is invalid.");

            req.OperationType = MeasurementAction.Compare;
            var result = _service.ProcessMeasurement(req);

            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);
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

        [AllowAnonymous]
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

        private List<string> GetModelErrors()
        {
            var msg = new List<string>();
            foreach (var state in ModelState.Values)
            {
                foreach (var err in state.Errors)
                {
                    msg.Add(err.ErrorMessage);
                }
            }
            return msg;
        }
    }
}
