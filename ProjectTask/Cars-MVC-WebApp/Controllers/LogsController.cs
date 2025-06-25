using Cars_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

public class LogsController : Controller
{
    private readonly HttpClient _httpClient;

    public LogsController(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient("api");
    }

    public async Task<IActionResult> Index(int n = 25)
    {
        var token = HttpContext.Session.GetString("JWToken");
        if (string.IsNullOrEmpty(token))
            return RedirectToAction("Login", "Auth");

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.GetAsync($"logs/get/{n}");
        if (!response.IsSuccessStatusCode)
            return Unauthorized();

        var json = await response.Content.ReadAsStringAsync();
        var logs = JsonSerializer.Deserialize<List<LogEntry>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        ViewBag.LogCount = n;
        return View(logs);
    }
}
