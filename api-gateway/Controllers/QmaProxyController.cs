using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace ApiGateway.Controllers
{
    /// <summary>
    /// Gateway proxy for the QMA Service.
    /// Routes: add, subtract, divide, compare, convert, and history → qma-service
    /// All computation and history now handled by qma-service (merged from history-service)
    /// </summary>
    [ApiController]
    [Route("api/quantitymeasurement")]
    public class QmaProxyController : ControllerBase
    {
        private readonly IHttpClientFactory _factory;
        public QmaProxyController(IHttpClientFactory factory) => _factory = factory;

        // ── Compute operations (anonymous) ───────────────────────────────────
        [HttpPost("add")]       [AllowAnonymous] public Task<IActionResult> Add()       => Forward("QmaService", "POST", "api/qma/add");
        [HttpPost("subtract")]  [AllowAnonymous] public Task<IActionResult> Subtract()  => Forward("QmaService", "POST", "api/qma/subtract");
        [HttpPost("divide")]    [AllowAnonymous] public Task<IActionResult> Divide()    => Forward("QmaService", "POST", "api/qma/divide");
        [HttpPost("compare")]   [AllowAnonymous] public Task<IActionResult> Compare()   => Forward("QmaService", "POST", "api/qma/compare");
        [HttpPost("convert")]   [AllowAnonymous] public Task<IActionResult> Convert()   => Forward("QmaService", "POST", "api/qma/convert");

        // ── History operations (authorized) → now forwarded to qma-service ───
        [HttpPost("save")]            [Authorize] public Task<IActionResult> Save()         => ForwardWithAuth("QmaService", "POST",   "api/qma/history/save");
        [HttpPost("save-batch")]      [Authorize] public Task<IActionResult> SaveBatch()    => ForwardWithAuth("QmaService", "POST",   "api/qma/history/save-batch");
        [HttpGet("history")]          [Authorize] public Task<IActionResult> GetHistory()   => ForwardWithAuth("QmaService", "GET",    "api/qma/history");
        [HttpDelete("history/clear")] [Authorize] public Task<IActionResult> ClearHistory() => ForwardWithAuth("QmaService", "DELETE", "api/qma/history/clear");

        [HttpDelete("history/{id:guid}")]
        [Authorize]
        public Task<IActionResult> DeleteHistoryItem(Guid id)
            => ForwardWithAuth("QmaService", "DELETE", $"api/qma/history/{id}");

        // ── Helpers ──────────────────────────────────────────────────────────

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
