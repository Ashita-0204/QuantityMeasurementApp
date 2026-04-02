using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuantityMeasurementBusinessLayer.Interfaces;
using QuantityMeasurementModel.DTOs;
using QuantityMeasurementModel.Entities;
using QuantityMeasurementRepository.Data;
using System.Security.Claims;

namespace QuantityMeasurementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuantityMeasurementController : ControllerBase
    {
        private readonly IQuantityMeasurementService _service;
        private readonly AppDbContext _db;

        // ── Request DTOs ────────────────────────────────────────────────────

        public class TwoQuantityRequest
        {
            public required QuantityRequest Quantity1 { get; set; }
            public required QuantityRequest Quantity2 { get; set; }
            public string? TargetUnit { get; set; }
        }

        public class QuantityRequest
        {
            public double Value { get; set; }
            public required string Unit { get; set; }
            public required string Category { get; set; }
        }

        public class ConversionRequest
        {
            public double Value { get; set; }
            public required string FromUnit { get; set; }
            public required string ToUnit { get; set; }
            public required string Category { get; set; }
        }

        // ── Save DTOs ───────────────────────────────────────────────────────

        public class SaveOperationRequest
        {
            public required string Operation { get; set; }
            public required SaveRequestData Data { get; set; }
            public required SaveResultData Result { get; set; }
        }

        public class SaveRequestData
        {
            public double? Value { get; set; }
            public string? FromUnit { get; set; }
            public string? ToUnit { get; set; }
            public string? Category { get; set; }
            public SaveQuantityData? Quantity1 { get; set; }
            public SaveQuantityData? Quantity2 { get; set; }
        }

        public class SaveQuantityData
        {
            public double Value { get; set; }
            public string Unit { get; set; } = string.Empty;
            public string Category { get; set; } = string.Empty;
        }

        public class SaveResultData
        {
            public bool Success { get; set; }
            public string Operation { get; set; } = string.Empty;
            public SaveQuantityData? Operand1 { get; set; }
            public SaveQuantityData? Operand2 { get; set; }
            public SaveQuantityData? Result { get; set; }
            public bool? BoolResult { get; set; }
            public double? ScalarResult { get; set; }
        }

        // ── History response ────────────────────────────────────────────────

        public class HistoryItemResponse
        {
            public Guid Id { get; set; }
            public DateTime Timestamp { get; set; }
            public string Operation { get; set; } = string.Empty;
            public double? Operand1Value { get; set; }
            public string? Operand1Unit { get; set; }
            public string? Operand1Category { get; set; }
            public double? Operand2Value { get; set; }
            public string? Operand2Unit { get; set; }
            public double? ResultValue { get; set; }
            public string? ResultUnit { get; set; }
            public bool? BoolResult { get; set; }
            public double? ScalarResult { get; set; }
        }

        public QuantityMeasurementController(IQuantityMeasurementService service, AppDbContext db)
        {
            _service = service;
            _db = db;
        }

        // ── Compute endpoints — AllowAnonymous ──────────────────────────────

        [HttpPost("add")]
        [AllowAnonymous]
        public ActionResult<QuantityResponseDTO> Add([FromBody] TwoQuantityRequest r)
        {
            try
            {
                var a = new QuantityDTO(r.Quantity1.Value, r.Quantity1.Unit, r.Quantity1.Category);
                var b = new QuantityDTO(r.Quantity2.Value, r.Quantity2.Unit, r.Quantity2.Category);
                var resp = _service.Add(a, b, r.TargetUnit);
                return resp.Success ? Ok(resp) : BadRequest(resp);
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPost("subtract")]
        [AllowAnonymous]
        public ActionResult<QuantityResponseDTO> Subtract([FromBody] TwoQuantityRequest r)
        {
            try
            {
                var a = new QuantityDTO(r.Quantity1.Value, r.Quantity1.Unit, r.Quantity1.Category);
                var b = new QuantityDTO(r.Quantity2.Value, r.Quantity2.Unit, r.Quantity2.Category);
                var resp = _service.Subtract(a, b, r.TargetUnit);
                return resp.Success ? Ok(resp) : BadRequest(resp);
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPost("divide")]
        [AllowAnonymous]
        public ActionResult<QuantityResponseDTO> Divide([FromBody] TwoQuantityRequest r)
        {
            try
            {
                var a = new QuantityDTO(r.Quantity1.Value, r.Quantity1.Unit, r.Quantity1.Category);
                var b = new QuantityDTO(r.Quantity2.Value, r.Quantity2.Unit, r.Quantity2.Category);
                var resp = _service.Divide(a, b);
                return resp.Success ? Ok(resp) : BadRequest(resp);
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPost("compare")]
        [AllowAnonymous]
        public ActionResult<QuantityResponseDTO> Compare([FromBody] TwoQuantityRequest r)
        {
            try
            {
                var a = new QuantityDTO(r.Quantity1.Value, r.Quantity1.Unit, r.Quantity1.Category);
                var b = new QuantityDTO(r.Quantity2.Value, r.Quantity2.Unit, r.Quantity2.Category);
                var resp = _service.Compare(a, b);
                return resp.Success ? Ok(resp) : BadRequest(resp);
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPost("convert")]
        [AllowAnonymous]
        public ActionResult<QuantityResponseDTO> Convert([FromBody] ConversionRequest r)
        {
            try
            {
                var input = new QuantityDTO(r.Value, r.FromUnit, r.Category);
                var resp = _service.Convert(input, r.ToUnit);
                return resp.Success ? Ok(resp) : BadRequest(resp);
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        // ── Save / History — Authorize ──────────────────────────────────────

        [HttpPost("save")]
        [Authorize]
        public async Task<ActionResult> Save([FromBody] SaveOperationRequest req)
        {
            try
            {
                if (!TryGetUserId(out var userId))
                    return Unauthorized(new { message = "Not authenticated" });

                _db.Measurements.Add(BuildRecord(userId, req));
                await _db.SaveChangesAsync();
                return Ok(new { message = "Saved" });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPost("save-batch")]
        [Authorize]
        public async Task<ActionResult> SaveBatch([FromBody] List<SaveOperationRequest> reqs)
        {
            try
            {
                if (!TryGetUserId(out var userId))
                    return Unauthorized(new { message = "Not authenticated" });

                _db.Measurements.AddRange(reqs.Select(r => BuildRecord(userId, r)));
                await _db.SaveChangesAsync();
                return Ok(new { message = $"{reqs.Count} saved", count = reqs.Count });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpGet("history")]
        [Authorize]
        public async Task<ActionResult<List<HistoryItemResponse>>> GetHistory()
        {
            try
            {
                if (!TryGetUserId(out var userId))
                    return Unauthorized(new { message = "Not authenticated" });

                var rows = await _db.Measurements
                    .Where(m => m.UserId == userId)
                    .OrderByDescending(m => m.Timestamp)
                    .Select(m => new HistoryItemResponse
                    {
                        Id = m.Id,
                        Timestamp = m.Timestamp,
                        Operation = m.Operation,
                        Operand1Value = m.Operand1Value,
                        Operand1Unit = m.Operand1Unit,
                        Operand1Category = m.Operand1Category,
                        Operand2Value = m.Operand2Value,
                        Operand2Unit = m.Operand2Unit,
                        ResultValue = m.ResultValue,
                        ResultUnit = m.ResultUnit,
                        BoolResult = m.BoolResult,
                        ScalarResult = m.ScalarResult,
                    })
                    .ToListAsync();

                return Ok(rows);
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        // NOTE: ClearHistory MUST be declared before DeleteHistoryItem.
        // ASP.NET would parse "clear" as a Guid and return 404 otherwise.
        [HttpDelete("history/clear")]
        [Authorize]
        public async Task<ActionResult> ClearHistory()
        {
            try
            {
                if (!TryGetUserId(out var userId))
                    return Unauthorized(new { message = "Not authenticated" });

                _db.Measurements.RemoveRange(
                    _db.Measurements.Where(m => m.UserId == userId));
                await _db.SaveChangesAsync();
                return Ok(new { message = "History cleared" });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpDelete("history/{id:guid}")]
        [Authorize]
        public async Task<ActionResult> DeleteHistoryItem(Guid id)
        {
            try
            {
                if (!TryGetUserId(out var userId))
                    return Unauthorized(new { message = "Not authenticated" });

                var rec = await _db.Measurements
                    .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);
                if (rec == null) return NotFound(new { message = "Not found" });

                _db.Measurements.Remove(rec);
                await _db.SaveChangesAsync();
                return Ok(new { message = "Deleted" });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        // ── Helpers ─────────────────────────────────────────────────────────

        private bool TryGetUserId(out int userId)
            => int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out userId);

        private static MeasurementRecord BuildRecord(int userId, SaveOperationRequest req)
        {
            var op = req.Operation.ToLower();
            var r = req.Result;
            var d = req.Data;

            var rec = new MeasurementRecord
            {
                UserId = userId,   // int? — assigned from verified JWT claim
                Operation = req.Operation,
                Timestamp = DateTime.UtcNow,
                BoolResult = r.BoolResult,
                ScalarResult = r.ScalarResult,
            };

            // Operand 1
            if (r.Operand1 != null)
            {
                rec.Operand1Value = r.Operand1.Value;
                rec.Operand1Unit = r.Operand1.Unit;
                rec.Operand1Category = r.Operand1.Category;
            }
            else if (op == "convert" && d.Value.HasValue)
            {
                rec.Operand1Value = d.Value;
                rec.Operand1Unit = d.FromUnit;
                rec.Operand1Category = d.Category;
            }

            // Operand 2
            if (r.Operand2 != null)
            {
                rec.Operand2Value = r.Operand2.Value;
                rec.Operand2Unit = r.Operand2.Unit;
            }

            // Result quantity
            if (r.Result != null)
            {
                rec.ResultValue = r.Result.Value;
                rec.ResultUnit = r.Result.Unit;
                rec.ResultCategory = r.Result.Category;
            }

            return rec;
        }
    }
}