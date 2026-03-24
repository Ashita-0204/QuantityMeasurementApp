using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QuantityMeasurementModel.DTOs.Auth;
using QuantityMeasurementModel.Entities;
using QuantityMeasurementRepository.Data;

namespace QuantityMeasurementBusinessLayer.Services.Auth
{
    /// <summary>
    /// UC17: Handles registration (BCrypt salted hash) and login (JWT issuance).
    /// Lives in BusinessLayer — depends on Repository (AppDbContext) and Model (DTOs).
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        public AuthResponseDTO Register(RegisterRequestDTO request)
        {
            if (_db.Users.Any(u => u.Username == request.Username))
                throw new InvalidOperationException($"Username '{request.Username}' is already taken.");

            if (_db.Users.Any(u => u.Email == request.Email))
                throw new InvalidOperationException($"Email '{request.Email}' is already registered.");

            // BCrypt generates a unique random salt and embeds it in the hash string
            string hash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new UserEntity
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = hash,
                CreatedAt = DateTime.UtcNow
            };

            _db.Users.Add(user);
            _db.SaveChanges();

            return BuildResponse(user, "Registration successful.");
        }

        public AuthResponseDTO Login(LoginRequestDTO request)
        {
            var user = _db.Users.FirstOrDefault(u => u.Username == request.Username)
                ?? throw new UnauthorizedAccessException("Invalid username or password.");

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid username or password.");

            return BuildResponse(user, "Login successful.");
        }

        private AuthResponseDTO BuildResponse(UserEntity user, string message)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiresAt = DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:ExpiryMinutes"]!));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,   user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name,  user.Username),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,   Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expiresAt,
                signingCredentials: creds);

            return new AuthResponseDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Username = user.Username,
                Email = user.Email,
                ExpiresAt = expiresAt,
                Message = message
            };
        }
    }
}