using System;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using System.Collections.Generic;
using static Newtonsoft.Json.JsonConvert;

// NB: All Properties prefixed with '_' are for internal use or placeholders and can be safely ignored //

namespace SaasafrasApiReference
{
	
	public class SaasafrasApi
	{
		public const string X_SAASAFRAS_JWT = "";
		public RestClient client;

		public SaasafrasApi()
		{
			client = new RestClient("https://8bdw7ju091.execute-api.us-east-2.amazonaws.com/dev/api/");
			client.AddDefaultHeader("x-saasafras-jwt", X_SAASAFRAS_JWT);
		}

		public static int Main()
		{
			var saas = new SaasafrasApi();

			var myClientIds = saas.GetClients().ClientIds;
			var someClientId = "november";
			var clientDetails = saas.GetClientDetails(someClientId);

			return 0;
		}

		/// <summary>
		/// Returns <br/> 
		/// 1. ids of all your clients
		/// </summary>
		public GetClientsResponse GetClients()
		{
			Console.WriteLine($"calling GET/clients");
			var request = new RestRequest($"clients", Method.GET);
			var response = client.Execute(request);
			var result = DeserializeObject<GetClientsResponse>(response.Content);
			Console.WriteLine($"You have access to {result.ClientIds.Length} clients");
			return result;
		}

		/// <summary>
		/// Returns <br/>
		/// 1. Information about the client <br/>
		/// 2. Information about the client's environments <br/>
		/// 3. Information about any solutions deployed to the client
		/// </summary>
		/// <param name="clientId">From GetClients or CreateClient</param>
		public GetClientResponse GetClientDetails(string clientId)
		{
			Console.WriteLine($"calling GET/client/{clientId}");
			var request = new RestRequest($"client/{clientId}");
			var response = client.Execute(request);
			var result = DeserializeObject<GetClientResponse>(response.Content);
			var instances = (from envs in result.Environments
							 select envs.Deployments.Count).Sum();
			Console.WriteLine($"Client '{result.Name}' has {result.Environments.Count} Envs and {instances} Solution Instances");
			return result;
		}

		/// <summary>
		/// Returns <br/> 
		/// 1. name/id/version of all your solutions
		/// </summary>
		public GetSolutionResponse[] GetSolutions()
		{
			Console.WriteLine($"calling GET/apps");
			var request = new RestRequest($"apps", Method.GET);
			var response = client.Execute(request);
			var result = DeserializeObject<GetSolutionResponse[]>(response.Content);
			Console.WriteLine($"You have access to {result.Length} solutions");
			return result;
		}

		/// <summary>
		/// Returns <br/>
		/// 1. the entire solution
		/// Not for the faint of heart.
		/// </summary>
		public GetSolutionResponse GetSolutionDetails(string solutionId, string version = "0.0")
		{
			Console.WriteLine($"calling GET/app/{solutionId}?version={version}");
			var request = new RestRequest($"app/{solutionId}", Method.GET);
			request.AddParameter("version", version);
			var response = client.Execute(request);
			var result = DeserializeObject<GetSolutionResponse[]>(response.Content)[0] ?? throw new Exception("Solution/Version not found");
			var applications = (from workspace in result.Workspaces
							 select workspace.Apps.Count).Sum();
			Console.WriteLine($"Solution {result.Name} has {result.Workspaces.Count} Workspaces and {applications} Applications");
			return result;
		}

		public CreateSolutionResponse CreateSolution(string clientId)
		{
			Console.WriteLine($"calling GET/client/{clientId}");
			var request = new RestRequest($"client/{clientId}");
			var response = client.Execute(request);
			var result = DeserializeObject<GetClientResponse>(response.Content);
			var instances = (from envs in result.Environments
							 select envs.Deployments.Count).Sum();
			Console.WriteLine($"Client '{result.Name}' has {result.Environments.Count} Envs and {instances} Solution Instances");
			return result;
		}


	}

	
}
