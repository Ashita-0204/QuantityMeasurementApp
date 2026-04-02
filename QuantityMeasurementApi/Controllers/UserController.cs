using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuantityMeasurementRepository.Data;
using System.Security.Claims;

namespace QuantityMeasurementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _db;
        public UserController(AppDbContext db) { _db = db; }

        [HttpGet("profile")]
        public async Task<ActionResult<object>> GetProfile()
        {
            try
            {
                if (!TryGetUserId(out var userId))
                    return Unauthorized(new { message = "User not authenticated" });

                var user = await _db.Users.FindAsync(userId);
                if (user == null) return NotFound(new { message = "User not found" });

                var measurements = await _db.Measurements
                    .Where(m => m.UserId == userId).ToListAsync();

                var mostUsed = measurements
                    .GroupBy(m => m.Operation)
                    .OrderByDescending(g => g.Count())
                    .Select(g => g.Key)
                    .FirstOrDefault() ?? "N/A";

                return Ok(new
                {
                    username = user.Username,
                    email = user.Email,
                    createdAt = user.CreatedAt,
                    totalOperations = measurements.Count,
                    savedResults = measurements.Count,
                    mostUsedOperation = mostUsed
                });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpDelete("account")]
        public async Task<ActionResult> DeleteAccount()
        {
            try
            {
                if (!TryGetUserId(out var userId))
                    return Unauthorized(new { message = "User not authenticated" });

                var user = await _db.Users.FindAsync(userId);
                if (user == null) return NotFound(new { message = "User not found" });

                _db.Measurements.RemoveRange(_db.Measurements.Where(m => m.UserId == userId));
                _db.Users.Remove(user);
                await _db.SaveChangesAsync();
                return Ok(new { message = "Account deleted" });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        private bool TryGetUserId(out int userId)
            => int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out userId);
    }
}