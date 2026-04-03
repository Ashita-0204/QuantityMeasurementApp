using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuantityMeasurementModel.Entities;
using QuantityMeasurementRepository.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QuantityMeasurementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _db;

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

        public AuthController(IConfiguration config, AppDbContext db)
        {
            _config = config;
            _db = db;
        }

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

        // ── Google OAuth ─────────────────────────────────────────────────────

        [HttpGet("google-login")]
        [AllowAnonymous]
        public IActionResult GoogleLogin()
        {
            // RedirectUri is where the Google middleware sends the user AFTER it has
            // validated the OAuth state+code at /signin-google (the CallbackPath).
            // This must be a real controller route — which is GoogleCallback below.
            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(GoogleCallback), "Auth", null, Request.Scheme, Request.Host.Value)
                // Resolves to: http://localhost:5001/api/Auth/google-callback
            };
            return Challenge(props, GoogleDefaults.AuthenticationScheme);
        }

        // FIX: Add [HttpGet] so this is a proper controller route.
        // Flow:
        //   1. User hits /api/Auth/google-login  → Challenge redirects to Google
        //   2. Google redirects to /signin-google (CallbackPath) → middleware validates state+code,
        //      signs the user into the cookie scheme, then redirects to RedirectUri
        //   3. RedirectUri lands here → we read the cookie, upsert the user, issue a JWT
        [HttpGet("google-callback")]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleCallback()
        {
            try
            {
                // Read the identity the middleware wrote into the cookie after validating Google's response
                var auth = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                if (!auth.Succeeded)
                    return Redirect("http://localhost:4200/auth?error=google_auth_failed");

                var email = auth.Principal!.FindFirstValue(ClaimTypes.Email) ?? "";
                var name = auth.Principal!.FindFirstValue(ClaimTypes.Name) ?? "User";

                // Upsert user
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
                    $"http://localhost:4200/" +
                    $"?token={Uri.EscapeDataString(token)}" +
                    $"&username={Uri.EscapeDataString(user.Username)}");
            }
            catch (Exception ex)
            {
                return Redirect($"http://localhost:4200/auth?error={Uri.EscapeDataString(ex.Message)}");
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
            var user = new UserEntity { Id = userId, Username = username, Email = email };
            return Ok(new { token = MakeToken(user) });
        }

        // ── JWT factory ──────────────────────────────────────────────────────

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