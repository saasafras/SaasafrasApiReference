using Newtonsoft.Json;

namespace SaasafrasApiReference
{
	public class GetClientsResponse
	{
		[JsonProperty("clientIds")]
		public string[] ClientIds { get; set; }
	}
}
