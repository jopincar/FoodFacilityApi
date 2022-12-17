using FoodFacilityApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net.Http.Headers;
using System.Text.Json;

namespace FoodFacilityApi.Controllers
{
	[Produces("application/json")]
	[Route("food-truck-permits")]
	[ApiController]
	public class FoodTruckController : Controller
	{
		private readonly ILogger<FoodTruckController> _logger;

		private JsonSerializerOptions _options = new JsonSerializerOptions { NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString };

		public FoodTruckController(ILogger<FoodTruckController> logger)
		{
			_logger = logger;
		}

		/// <summary>
		/// Get food truck permits with optional filtering
		/// </summary>
		[HttpGet]
		[Produces(typeof(List<FoodTruckPermit>))]
		public async Task<IActionResult> GetFoodTruckPermits([FromQuery] FoodTruckPermitSearchParams searchParams) {
			// TODO: make base URL configurable
			var uri = $"https://data.sfgov.org/resource/rqzj-sfat.json{searchParams?.ToQueryString() ?? ""}";
			_logger.LogTrace(uri);

			using HttpClient client = new();
			var response = await client.GetAsync(uri);

			if ( response == null) {
				return StatusCode(500, new { Error = "Null response from data source." });
			} 
			response.EnsureSuccessStatusCode();

			var body = await response.Content.ReadAsStringAsync();
			_logger.LogTrace(body);

			await using var stream = await response.Content.ReadAsStreamAsync();
			var permits = await JsonSerializer.DeserializeAsync<List<FoodTruckPermit>>(stream, _options);

			return new OkObjectResult(permits);
		}

	}
}
