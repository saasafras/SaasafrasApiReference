using Newtonsoft.Json;

namespace SaasafrasApiReference
{

	public class CreateClientResponse
	{
		[JsonProperty("message")]
		public string Message { get; set; }

		[JsonProperty("resource")]
		public string ClientId { get; set; }
	}



	public class CreateClientRequest
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		/// <summary>
		/// Optional Properties
		/// </summary>
		[JsonProperty("info")]
		public Info Info { get; set; }

		/// <summary>
		/// Optional Default Environment names
		/// </summary>
		[JsonProperty("environments")]
		public string[] Environments { get; set; }

	}

	/// <summary>
	/// Optional Properties to include
	/// </summary>
	public class Info
	{
		[JsonProperty("email")]
		public string Email { get; set; }

		[JsonProperty("company")]
		public string Company { get; set; }

		[JsonProperty("contact")]
		public string Contact { get; set; }

		[JsonProperty("notes")]
		public string Notes { get; set; }
	}



}