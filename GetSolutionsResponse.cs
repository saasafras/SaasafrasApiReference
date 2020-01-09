using System.Collections.Generic;
using Newtonsoft.Json;

namespace SaasafrasApiReference
{
	public class GetSolutionResponse
	{
		[JsonProperty("workspaces")]
		public List<Workspace> Workspaces { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("solutionId")]
		public string SolutionId { get; set; }

		[JsonProperty("version")]
		public string Version { get; set; }

		public class Workspace
		{
			[JsonProperty("workspaceName")]
			public string WorkspaceName { get; set; }

			[JsonProperty("workspaceId")]
			public long WorkspaceId { get; set; }

			/// <summary>
			/// Refer to Podio API documentation for Applications & Fields
			/// </summary>
			[JsonProperty("apps")]
			public List<object> Apps { get; set; }

			/// <summary>
			/// Refer to Podio API documentation for Workspaces
			/// </summary>
			[JsonProperty("PodioSpace")]
			public object PodioSpace { get; set; }
		}
	}
}