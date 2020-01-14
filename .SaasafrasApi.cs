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

	public partial class SaasafrasApi
	{
		public static string X_SAASAFRAS_JWT = ""; // Required for all calls - https://www.youtube.com/watch?v=iThtELZvfPs
		public static int[] WORKSPACE_IDS = { 0, 1, 2 }; // The Workspace(s) you want to use for your solution - Required for CreateSolution
		public static int ORG_ID = 3456; // The Organization you want to deploy an instance of your solution into - Required for CreateInstance & DeployInstance
		private RestClient client;

		public SaasafrasApi()
		{
			client = new RestClient("https://8bdw7ju091.execute-api.us-east-2.amazonaws.com/dev/api/");
			client.AddDefaultHeader("x-saasafras-jwt", X_SAASAFRAS_JWT);
			client.AddDefaultHeader("content-type", "application/json");
		}


		/// <summary>
		/// An example case demonstrating how to perform a deployment with no pre-existing Solutions or Clients.
		/// </summary>
		public static void Main()
		{
			if (X_SAASAFRAS_JWT == null) throw new Exception("JWT Required");
			var saas = new SaasafrasApi();

			// Create a new Solution, then take a peek to validate it.
			var newSolution = saas.CreateSolution(name: "my_solution",workspaceIds: WORKSPACE_IDS);
			var mySolutions = saas.GetSolutions();
			var newSolutionDetails = saas.GetSolutionDetails(solutionId: newSolution.SolutionId);

			// Create a new Client, then take a peek to validate it.
			var newClient = saas.CreateClient(name: "smitty");
			var myClientIds = saas.GetClients().ClientIds;
			var newClientDetails = saas.GetClientDetails(clientId: newClient.ClientId);

			// Create a new Instance of the Solution for the Client
			var myInstance = saas.CreateInstance(
				clientId: newClientDetails.Id,
				envId: newClientDetails.Environments[0].EnvironmentId,
				solutionId: newSolutionDetails.SolutionId,
				version: newSolutionDetails.Version,
				orgId: ORG_ID);

			// Try to go ahead and start Building out the instance in Podio
			var myInstanceInstallationConfirmation = saas.DeployInstance(
				newClientDetails.Id, newClientDetails.Environments[0].EnvironmentId, newSolutionDetails.SolutionId, newSolutionDetails.Version, ORG_ID, myInstance.InstanceId);
			
			return;
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
		/// Creates a new client <br/>
		/// Returns <br/>
		/// 1. A message indicating sucesss/failure
		/// 2. The new client's id, or null
		/// </summary>
		/// <param name="info">optional client properties</param>
		/// <param name="environments">optional default environment names</param>
		public CreateClientResponse CreateClient(string name, Info info = null, string[] environments = null)
		{
			Console.WriteLine($"calling POST/clients");
			var request = new RestRequest($"clients", Method.POST);
			request.AddJsonBody(new
			{
				name,
				info,
				environments
			});
			var response = client.Execute(request);
			var result = DeserializeObject<CreateClientResponse>(response.Content);
			Console.WriteLine($"Created Client with id '{result.ClientId}'");
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

		/// <summary>
		/// Creates a new solution from the provided workspacesIds. <br/>
		/// Returns <br/>
		/// 1. A message indicating sucesss/failure
		/// 2. The new solution's id, or null
		/// </summary>
		public CreateSolutionResponse CreateSolution(string name, int[] workspaceIds)
		{
			Console.WriteLine($"calling POST/apps");
			var request = new RestRequest($"apps", Method.POST);
			request.AddJsonBody(new
			{
				name,
				workspaceIds
			});
			var response = client.Execute(request);
			var result = DeserializeObject<CreateSolutionResponse>(response.Content);
			if (result.SolutionId == null) throw new Exception("JWT is invalid, or you do not have admin permissions for those Workspaces");
			Console.WriteLine($"Created Solution with id '{result.SolutionId}'");
			return result;
		}

		/// <summary>
		/// Defines a new instance of a solution into a client's environment, but does not make changes to Podio.<br/>
		/// Each environment can only have one instance of a given solution.
		/// </summary>
		/// <param name="orgId">The organization to deploy INTO</param>
		public CreateInstanceResponse CreateInstance(string clientId, string envId, string solutionId, string version, long orgId)
		{
			Console.WriteLine($"calling POST/client/{clientId}/env/{envId}");
			var request = new RestRequest($"client/{clientId}/env/{envId}", Method.POST);
			var body = new CreateInstanceRequest
			{
				Deploy = new Deploy {
					AppId = solutionId,
					Version = version
				},
				OrgId = orgId
			};
			request.AddJsonBody(body);
			var response = client.Execute(request);
			if (response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout) throw new Exception($"Instance likely created, retry request or run GET client/{clientId} to confirm");
			var result = DeserializeObject<CreateInstanceResponse>(response.Content);
			Console.WriteLine($"Created Instance with id '{result.InstanceId}'");
			return result;
		}

		/// <summary>
		/// Installs the Solution's Podio workpaces into the client's environment.<br/>
		/// NB: The API will likely time out before the Instance is fully installed - Check Podio for the result.
		/// </summary>
		/// <param name="orgId">The organization to deploy INTO</param>
		public DeployInstanceResponse DeployInstance(string clientId, string envId, string solutionId, string version, long orgId, string deploymentId)
		{
			Console.WriteLine($"calling POST/client/{clientId}/env/{envId}");
			var request = new RestRequest($"client/{clientId}/env/{envId}", Method.POST);
			var body = new DeployInstanceRequest
			{
				Command = "run-deploy-steps",
				DeploymentId = deploymentId,
				Deploy = new Deploy {
					AppId = solutionId,
					Version = version
				},
				OrgId = orgId
			};
			request.AddJsonBody(body);
			var response = client.Execute(request);
			if (response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout) throw new Exception($"Instance likely deployed, check Podio to confirm");
			var result = DeserializeObject<DeployInstanceResponse>(response.Content);
			Console.WriteLine($"{result.Message}");
			return result;
		}



	}
}
