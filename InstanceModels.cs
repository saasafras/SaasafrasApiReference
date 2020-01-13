// NB: All Properties prefixed with '_' are for internal use or placeholders and can be safely ignored //

using Newtonsoft.Json;

namespace SaasafrasApiReference
{

	public class CreateInstanceRequest
	{
		[JsonProperty("deploy")]
		public Deploy Deploy { get; set; }

		[JsonProperty("orgId")]
		public long OrgId { get; set; }
	}

	public class DeployInstanceRequest : CreateInstanceRequest
	{
		[JsonProperty("command")]
		public string Command { get; set; }
		[JsonProperty("deploymentId")]
		public string DeploymentId { get; set; }
	}

	public class Deploy
	{
		/// <summary>
		/// Actually correspondes to "SolutionId"
		/// </summary>
		[JsonProperty("appId")]
		public string AppId { get; set; }

		[JsonProperty("version")]
		public string Version { get; set; }
	}
	
	public class CreateInstanceResponse
	{
		[JsonProperty("message")]
		public string Message { get; set; }

		[JsonProperty("resource")]
		public string InstanceId { get; set; }
	}

	public class DeployInstanceResponse
	{
		[JsonProperty("message")]
		public string Message { get; set; }

		[JsonProperty("resource")]
		public string _Resource { get; set; }
	}


}

