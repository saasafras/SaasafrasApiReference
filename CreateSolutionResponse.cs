using Newtonsoft.Json;

namespace SaasafrasApiReference
{
	public class CreateSolutionResponse
	{
		[JsonProperty("message")]
		public string Message { get; set; }

		[JsonProperty("resource")]
		public string SolutionId { get; set; }
	}
}