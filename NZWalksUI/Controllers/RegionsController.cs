using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NZWalksUI.Models;

namespace NZWalksUI.Controllers;

public class RegionsController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly IHttpClientFactory _httpClientFactory;

    public RegionsController(IHttpClientFactory httpClientFactory, ILogger<HomeController> logger)
    {
      this._logger = logger;
      this._httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
      List<RegionDto> response = new List<RegionDto>();

      try
      {
        // Get All Regions from Web API
        var client = _httpClientFactory.CreateClient();

        var httpResponseMessage = await client.GetAsync("https://localhost:7174/api/Regions");
      
        httpResponseMessage.EnsureSuccessStatusCode();

        response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>());

        ViewBag.Response = response;
      }
      catch(Exception ex)
      {
        // Load the exception

      }
      return View(response);
    }
}
