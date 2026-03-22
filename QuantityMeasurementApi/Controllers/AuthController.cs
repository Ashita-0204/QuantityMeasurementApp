using Microsoft.AspNetCore.Mvc;
using QuantityMeasurementBusinessLayer.Services.Auth;
using QuantityMeasurementModel.DTOs.Auth;

namespace QuantityMeasurementApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [Tags("Authentication")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService) => _authService = authService;

        //Register a new user. Password is stored as a BCrypt salted hash
        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Register([FromBody] RegisterRequestDTO request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return StatusCode(201, _authService.Register(request));
        }

        //Login and receive a JWT. Use it as: Authorization
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Login([FromBody] LoginRequestDTO request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(_authService.Login(request));
        }
    }
}