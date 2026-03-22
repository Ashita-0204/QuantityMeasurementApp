using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuantityMeasurementBusinessLayer;
using QuantityMeasurementModel.DTOs;
using QuantityMeasurementRepository;

namespace QuantityMeasurementApi.Controllers
{

    /// All quantity operations require JWT (Authorization: Bearer token).
    /// Every operation is automatically saved to SQL Server via EF Core.

    [ApiController]
    [Route("api/v1/quantities")]
    [Authorize]
    [Tags("Quantity Measurements")]
    public class QuantityMeasurementController : ControllerBase
    {
        private readonly IQuantityMeasurementService _service;
        private readonly IQuantityMeasurementRepository _repository;

        public QuantityMeasurementController(
            IQuantityMeasurementService service,
            IQuantityMeasurementRepository repository)
        {
            _service = service;
            _repository = repository;
        }

        // Compare two quantities. 
        [HttpPost("compare")]
        public IActionResult Compare([FromBody] QuantityInputDTO input)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (input.Operand2 == null) return BadRequest("Operand2 is required.");
            var r = _service.Compare(input.Operand1, input.Operand2);
            return r.Success ? Ok(r) : BadRequest(r);
        }

        // Convert a quantity. Set Operand2.Unit as the target unit. 
        [HttpPost("convert")]
        public IActionResult Convert([FromBody] QuantityInputDTO input)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (input.Operand2 == null) return BadRequest("Operand2.Unit (target unit) is required.");
            var r = _service.Convert(input.Operand1, input.Operand2.Unit);
            return r.Success ? Ok(r) : BadRequest(r);
        }

        // Add two quantities. Optionally set TargetUnit for the result. 
        [HttpPost("add")]
        public IActionResult Add([FromBody] QuantityInputDTO input)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (input.Operand2 == null) return BadRequest("Operand2 is required.");
            var r = _service.Add(input.Operand1, input.Operand2, input.TargetUnit);
            return r.Success ? Ok(r) : BadRequest(r);
        }

        // Subtract Operand2 from Operand1. 
        [HttpPost("subtract")]
        public IActionResult Subtract([FromBody] QuantityInputDTO input)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (input.Operand2 == null) return BadRequest("Operand2 is required.");
            var r = _service.Subtract(input.Operand1, input.Operand2, input.TargetUnit);
            return r.Success ? Ok(r) : BadRequest(r);
        }

        // Divide Operand1 by Operand2. Returns a dimensionless scalar ratio. 
        [HttpPost("divide")]
        public IActionResult Divide([FromBody] QuantityInputDTO input)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (input.Operand2 == null) return BadRequest("Operand2 is required.");
            var r = _service.Divide(input.Operand1, input.Operand2);
            return r.Success ? Ok(r) : BadRequest(r);
        }

        // All stored measurement records (newest first). 
        [HttpGet("history")]
        public IActionResult GetAll() => Ok(_repository.GetAll());

        // Filter by operation: Compare / Convert / Add / Subtract / Divide 
        [HttpGet("history/operation/{operation}")]
        public IActionResult GetByOperation(string operation) => Ok(_repository.GetByOperation(operation));

        // Filter by category: Length / Weight / Volume / Temperature 
        [HttpGet("history/category/{category}")]
        public IActionResult GetByCategory(string category) => Ok(_repository.GetByCategory(category));

        // N most recent records. 
        [HttpGet("history/recent/{count:int}")]
        public IActionResult GetRecent(int count) => Ok(_repository.GetRecent(count));

        // Total number of stored records. 
        [HttpGet("count")]
        public IActionResult GetCount() => Ok(new { total = _repository.GetTotalCount() });

        // Delete all stored records. 
        [HttpDelete("clear")]
        public IActionResult Clear() { _repository.DeleteAll(); return Ok(new { message = "All records deleted." }); }
    }
}