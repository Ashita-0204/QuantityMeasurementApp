using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace ApiGateway.Controllers
{
    /// <summary>
    /// Gateway proxy for the Auth Service.
    /// All /api/auth/* requests are forwarded to auth-service.
    /// </summary>
    [ApiController]
    [Route("api/auth")]
    public class AuthProxyController : ControllerBase
    {
        private readonly IHttpClientFactory _factory;

        public AuthProxyController(IHttpClientFactory factory) => _factory = factory;

        // POST /api/auth/login → auth-service
        [HttpPost("login")]
        [AllowAnonymous]
        public Task<IActionResult> Login() => Forward("AuthService", "POST", "api/auth/login");

        // POST /api/auth/register → auth-service
        [HttpPost("register")]
        [AllowAnonymous]
        public Task<IActionResult> Register() => Forward("AuthService", "POST", "api/auth/register");

        // GET /api/auth/google-login → auth-service
        [HttpGet("google-login")]
        [AllowAnonymous]
        public Task<IActionResult> GoogleLogin() => Forward("AuthService", "GET", "api/auth/google-login");

        // GET /api/auth/google-callback → auth-service
        [HttpGet("google-callback")]
        [AllowAnonymous]
        public Task<IActionResult> GoogleCallback() => Forward("AuthService", "GET", $"api/auth/google-callback?{Request.QueryString.Value}");

        // POST /api/auth/refresh-token → auth-service (requires JWT)
        [HttpPost("refresh-token")]
        [Authorize]
        public Task<IActionResult> RefreshToken() => ForwardWithAuth("AuthService", "POST", "api/auth/refresh-token");

        // ── Generic proxy helpers ────────────────────────────────────────────

        private async Task<IActionResult> Forward(string clientName, string method, string path)
        {
            var client = _factory.CreateClient(clientName);
            var req = await BuildRequest(method, path);
            var resp = await client.SendAsync(req);
            return await ToActionResult(resp);
        }

        private async Task<IActionResult> ForwardWithAuth(string clientName, string method, string path)
        {
            var client = _factory.CreateClient(clientName);
            var req = await BuildRequest(method, path);
            if (Request.Headers.TryGetValue("Authorization", out var auth))
                req.Headers.Authorization = AuthenticationHeaderValue.Parse(auth!);
            var resp = await client.SendAsync(req);
            return await ToActionResult(resp);
        }

        private async Task<HttpRequestMessage> BuildRequest(string method, string path)
        {
            var req = new HttpRequestMessage(new HttpMethod(method), path);
            if (Request.ContentLength > 0 || Request.ContentType != null)
            {
                var body = await new StreamReader(Request.Body).ReadToEndAsync();
                req.Content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");
            }
            return req;
        }

        private async Task<IActionResult> ToActionResult(HttpResponseMessage resp)
        {
            var body = await resp.Content.ReadAsStringAsync();
            return StatusCode((int)resp.StatusCode, body);
        }
    }
}
