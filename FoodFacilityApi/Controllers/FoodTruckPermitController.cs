using FoodFacilityApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;
using System.Text.Json;

namespace FoodFacilityApi.Controllers
{
	[Produces("application/json")]
	[Route("food-truck-permits")]
	[ApiController]
	public class FoodTruckPermitController : Controller
	{
		private readonly JsonSerializerOptions _options = new() { NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString };

		/// <summary>
		/// Get food truck permits with optional filtering
		/// </summary>
		[HttpGet]
		[Produces(typeof(List<FoodTruckPermit>))]
		public async Task<IActionResult> GetFoodTruckPermits([FromQuery] FoodTruckPermitSearchParams searchParams) {
			// TODO: make base URL configurable
			var uri = $"https://data.sfgov.org/resource/rqzj-sfat.json{searchParams?.ToQueryString() ?? ""}";

			using HttpClient client = new();
			var response = await client.GetAsync(uri);

			if ( response == null) {
				return StatusCode(500, new { Error = "Null response from data source." });
			} 

			var body = await response.Content.ReadAsStringAsync();

			response.EnsureSuccessStatusCode();

			await using var stream = await response.Content.ReadAsStreamAsync();
			var permits = await JsonSerializer.DeserializeAsync<List<FoodTruckPermit>>(stream, _options);
			if ( permits == null ) {
				return StatusCode(500, new { Error = "Unable to deserialize response from data source." });
			}

			if ( searchParams?.SortByDistance() ?? false ) {
				permits = permits
					.OrderBy(p => p.DistanceFrom(searchParams.latitude ?? 0, searchParams.longitude ?? 0))
					.ToList();
			}

			return new OkObjectResult(permits.Take(searchParams?.max_return ?? int.MaxValue).ToList());
		}

		[HttpGet("ping")]
		public IActionResult GetPing() {
			return new OkObjectResult("ping");
		}

	}
}
