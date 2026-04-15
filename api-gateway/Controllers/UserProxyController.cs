using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace ApiGateway.Controllers
{
    /// <summary>
    /// Gateway proxy for User Management endpoints.
    /// Forwards /api/user/* → auth-service (merged from user-service).
    /// </summary>
    [ApiController]
    [Route("api/user")]
    [Authorize]
    public class UserProxyController : ControllerBase
    {
        private readonly IHttpClientFactory _factory;
        public UserProxyController(IHttpClientFactory factory) => _factory = factory;

        [HttpGet("profile")]
        public Task<IActionResult> GetProfile() => ForwardWithAuth("AuthService", "GET", "api/auth/profile");

        [HttpDelete("account")]
        public Task<IActionResult> DeleteAccount() => ForwardWithAuth("AuthService", "DELETE", "api/auth/account");

        private async Task<IActionResult> ForwardWithAuth(string clientName, string method, string path)
        {
            var client = _factory.CreateClient(clientName);
            var req = new HttpRequestMessage(new HttpMethod(method), path);
            if (Request.Headers.TryGetValue("Authorization", out var auth))
                req.Headers.Authorization = AuthenticationHeaderValue.Parse(auth!);
            var resp = await client.SendAsync(req);
            var body = await resp.Content.ReadAsStringAsync();
            return StatusCode((int)resp.StatusCode, body);
        }
    }
}
