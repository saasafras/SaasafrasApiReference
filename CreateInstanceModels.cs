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
		public class Deploy
		{
			[JsonProperty("appId")]
			public string SolutionId { get; set; }

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
}

