using System.Text.Json.Serialization;

namespace FoodFacilityApi.Models
{
	public class FoodTruckPermit
	{
		[JsonInclude]
		public string? objectid { get; set; }
		[JsonInclude]
		public string? applicant { get; set; }
		[JsonInclude]
		public string? facilitytype { get; set; }
		[JsonInclude]
		public string? locationdescription { get; set; }
		[JsonInclude]
		public string? address { get; set; }
		[JsonInclude]
		public string? blocklot { get; set; }
		[JsonInclude]
		public string? block { get; set; }
		[JsonInclude]
		public string? lot { get; set; }
		[JsonInclude]
		public string? permit { get; set; }
		[JsonInclude]
		public string? status { get; set; }
		[JsonInclude]
		public string? fooditems { get; set; }
		[JsonInclude]
		public double? x { get; set; }
		[JsonInclude]
		public double? y { get; set; }
		[JsonInclude]
		public double? latitude { get; set; }
		[JsonInclude]
		public double? longitude { get; set; }
	}
}
