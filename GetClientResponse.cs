using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SaasafrasApiReference
{

	public class GetClientResponse
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("company")]
		public string Company { get; set; }

		[JsonProperty("contact")]
		public string Contact { get; set; }

		[JsonProperty("email")]
		public string Email { get; set; }

		[JsonProperty("notes")]
		public string Notes { get; set; }

		[JsonProperty("environments")]
		public List<Environment> Environments { get; set; }

		public class Environment
		{
			[JsonProperty("Deployments")]
			public List<Deployment> Deployments { get; set; }

			[JsonProperty("EnvironmentId")]
			public string EnvironmentId { get; set; }

			[JsonProperty("Name")]
			public string Name { get; set; }

			[JsonProperty("OwnerId")]
			public object _OwnerId { get; set; }
		}

		public class Deployment
		{
			[JsonProperty("SolutionId")]
			public string SolutionId { get; set; }

			[JsonProperty("Version")]
			public string Version { get; set; }

			[JsonProperty("Date")]
			public string Date { get; set; }

			[JsonProperty("Backup")]
			public string _Backup { get; set; }

			[JsonProperty("Status")]
			public string _Status { get; set; }
		}
	}
}
