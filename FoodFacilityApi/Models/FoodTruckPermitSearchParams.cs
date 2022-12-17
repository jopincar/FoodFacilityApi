using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace FoodFacilityApi.Models
{
	public class FoodTruckPermitSearchParams
	{
		public string? applicant { get; set; }
		public string? status { get; set; } // TODO: use enum
		public string? address { get; set; }
		public double? latitude { get; set; }
		public double? longitude { get; set; }
		public int? distance_in_meters { get; set; }

		internal string ToQueryString()
		{
			var ret = "";

			if ( !string.IsNullOrWhiteSpace(applicant) ) {
				ret += $"&applicant={applicant}";
			}

			if (!string.IsNullOrWhiteSpace(status)) {
				ret += $"&status={status}";
			}

			if (!string.IsNullOrWhiteSpace(address)) {
				ret += $"&$where=address%20like%20%27%25{address}%25%27";
			}

			if ( latitude.HasValue && longitude.HasValue ) {
				ret += $"&$where=within_circle(Location,%20{latitude},%20{longitude},%20{distance_in_meters})";
			}

			if ( ret.Length > 0 ) {
				ret = "?" + ret.Substring(1);
			}

			return ret;
		}
	}
}
