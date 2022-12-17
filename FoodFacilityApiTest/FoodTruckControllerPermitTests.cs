using FoodFacilityApi.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Threading;

namespace FoodFacilityApiTest
{
	public class FoodTruckPermitControllerTests : IClassFixture<WebApplicationFactory<FoodFacilityApi.Program>>
	{
		[Fact]
		public async Task GetPermitsByApplicantAndStatus()
		{
			await using var application = new WebApplicationFactory<FoodFacilityApi.Program>();
			using var client = application.CreateClient();

			var applicant = "Bonito Poke";
			var response = await client.GetAsync($"food-truck-permits?applicant={applicant}&status=APPROVED");
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.NotNull(response);
			var responseBody = await response.Content.ReadAsStringAsync();
			var permits = JsonConvert.DeserializeObject<List<FoodTruckPermit>>(responseBody);
			permits.ForEach(permit =>
			{
				Assert.NotNull(permit);
				Assert.NotNull(permit.status);
				Assert.Equal("APPROVED", permit.status);
				Assert.NotNull(permit.applicant);
				Assert.Equal(applicant, permit.applicant);
			});

		}

		[Fact]
		public async Task GetPermitsByAddress()
		{
			await using var application = new WebApplicationFactory<FoodFacilityApi.Program>();
			using var client = application.CreateClient();

			var response = await client.GetAsync("food-truck-permits?address=CALI");
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.NotNull(response);
			var responseBody = await response.Content.ReadAsStringAsync();
			var permits = JsonConvert.DeserializeObject<List<FoodTruckPermit>>(responseBody);
			permits.ForEach(permit =>
			{
				Assert.NotNull(permit);
				Assert.NotNull(permit.address);
				Assert.Contains("CALI", permit.address);
			});

		}

		[Fact]
		public async Task GetPermitsByDistance()
		{
			await using var application = new WebApplicationFactory<FoodFacilityApi.Program>();
			using var client = application.CreateClient();

			double latitude = 37.793262206923096;
			double longitude = -122.4017890913628;
			double distanceInMeters = 1000;

			var response = await client.GetAsync($"food-truck-permits?latitude={latitude}&longitude={longitude}&distance_in_meters={distanceInMeters}&max_return=5");
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.NotNull(response);

			var responseBody = await response.Content.ReadAsStringAsync();
			var permits = JsonConvert.DeserializeObject<List<FoodTruckPermit>>(responseBody);
			double lastDistance = 0;
			for (int i = 0; i < permits.Count; i++) {
				var permit = permits[i];
				var distance = permit.DistanceFrom(latitude, longitude);
				Assert.NotNull(permit);
				Assert.NotNull(permit.latitude);
				Assert.NotNull(permit.longitude);
				Assert.True(distance < distanceInMeters);
				Assert.True(distance >= lastDistance);
			}
		}

		[Fact]
		public async Task GetPing()
		{
			await using var application = new WebApplicationFactory<FoodFacilityApi.Program>();
			using var client = application.CreateClient();
			var response = await client.GetAsync("food-truck-permits/ping");
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.NotNull(response);
		}
	}
}
