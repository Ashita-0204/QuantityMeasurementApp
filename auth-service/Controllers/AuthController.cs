using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using AuthService.Data;
using AuthService.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Controllers
{
    /// <summary>
    /// Merged Auth + User Management Controller
    /// Handles: authentication (login/register/OAuth), user profile, account deletion
    /// </summary>
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly AuthDbContext _db;

        public class LoginRequest
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        public class RegisterRequest
        {
            public string Username { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        public AuthController(IConfiguration config, AuthDbContext db)
        {
            _config = config;
            _db = db;
        }

        // ═══════════════════════════════════════════════════════════════════
        // AUTHENTICATION ENDPOINTS
        // ═══════════════════════════════════════════════════════════════════

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<object>> Login([FromBody] LoginRequest req)
        {
            try
            {
                if (string.IsNullOrEmpty(req.Email) || string.IsNullOrEmpty(req.Password))
                    return BadRequest(new { message = "Email and password are required" });

                var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == req.Email);
                if (user == null || !BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
                    return BadRequest(new { message = "Invalid email or password" });

                var token = MakeToken(user);
                return Ok(new { token, username = user.Username });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<object>> Register([FromBody] RegisterRequest req)
        {
            try
            {
                if (string.IsNullOrEmpty(req.Username) || string.IsNullOrEmpty(req.Email) || string.IsNullOrEmpty(req.Password))
                    return BadRequest(new { message = "All fields are required" });

                if (req.Password.Length < 8)
                    return BadRequest(new { message = "Password must be at least 8 characters" });

                if (await _db.Users.AnyAsync(u => u.Email == req.Email))
                    return BadRequest(new { message = "Email already registered" });

                if (await _db.Users.AnyAsync(u => u.Username == req.Username))
                    return BadRequest(new { message = "Username already taken" });

                var user = new UserEntity
                {
                    Username = req.Username,
                    Email = req.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password),
                    CreatedAt = DateTime.UtcNow
                };
                _db.Users.Add(user);
                await _db.SaveChangesAsync();

                var token = MakeToken(user);
                return Ok(new { token, username = user.Username });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpGet("google-login")]
        [AllowAnonymous]
        public IActionResult GoogleLogin()
        {
            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(GoogleCallback), "Auth", null, Request.Scheme, Request.Host.Value)
            };
            return Challenge(props, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-callback")]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleCallback()
        {
            var frontendUrl = _config["Frontend:BaseUrl"] ?? "http://localhost:4200";

            try
            {
                var auth = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                if (!auth.Succeeded)
                    return Redirect($"{frontendUrl}/auth?error=google_auth_failed");

                var email = auth.Principal!.FindFirstValue(ClaimTypes.Email) ?? "";
                var name = auth.Principal!.FindFirstValue(ClaimTypes.Name) ?? "User";

                var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    var baseUn = name.Replace(" ", "_");
                    var un = baseUn;
                    var n = 1;
                    while (await _db.Users.AnyAsync(u => u.Username == un))
                        un = $"{baseUn}{n++}";

                    user = new UserEntity
                    {
                        Username = un,
                        Email = email,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString()),
                        CreatedAt = DateTime.UtcNow
                    };
                    _db.Users.Add(user);
                    await _db.SaveChangesAsync();
                }

                var token = MakeToken(user);
                return Redirect(
                    $"{frontendUrl}/" +
                    $"?token={Uri.EscapeDataString(token)}" +
                    $"&username={Uri.EscapeDataString(user.Username)}");
            }
            catch (Exception ex)
            {
                return Redirect($"{frontendUrl}/auth?error={Uri.EscapeDataString(ex.Message)}");
            }
        }

        [HttpPost("refresh-token")]
        [Authorize]
        public ActionResult<object> RefreshToken()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();
            var username = User.FindFirstValue(ClaimTypes.Name) ?? "";
            var email = User.FindFirstValue(ClaimTypes.Email) ?? "";
            var user = new UserEntity { Id = userId, Username = username, Email = email, PasswordHash = "" };
            return Ok(new { token = MakeToken(user) });
        }

        // ═══════════════════════════════════════════════════════════════════
        // USER MANAGEMENT ENDPOINTS (merged from user-service)
        // ═══════════════════════════════════════════════════════════════════

        [HttpGet("profile")]
        [Authorize]
        public async Task<ActionResult<object>> GetProfile()
        {
            try
            {
                if (!TryGetUserId(out var userId))
                    return Unauthorized(new { message = "User not authenticated" });

                var user = await _db.Users.FindAsync(userId);
                if (user == null)
                    return NotFound(new { message = "User not found" });

                // Note: We can't get totalOperations/savedResults here as measurements 
                // are in qma-service. The frontend can call both services if needed.
                return Ok(new
                {
                    username = user.Username,
                    email = user.Email,
                    createdAt = user.CreatedAt
                });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpDelete("account")]
        [Authorize]
        public async Task<ActionResult> DeleteAccount()
        {
            try
            {
                if (!TryGetUserId(out var userId))
                    return Unauthorized(new { message = "User not authenticated" });

                var user = await _db.Users.FindAsync(userId);
                if (user == null)
                    return NotFound(new { message = "User not found" });

                // NOTE: This only deletes the user from auth-service.
                // The qma-service should handle cleanup of measurements separately
                // or via a background job/event system in production.
                _db.Users.Remove(user);
                await _db.SaveChangesAsync();

                return Ok(new { message = "Account deleted" });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        // ═══════════════════════════════════════════════════════════════════
        // HELPERS
        // ═══════════════════════════════════════════════════════════════════

        private bool TryGetUserId(out int userId)
            => int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out userId);

        private string MakeToken(UserEntity user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name,           user.Username),
                new Claim(ClaimTypes.Email,          user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
