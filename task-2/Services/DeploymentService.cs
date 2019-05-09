// Requires the following Azure NuGet packages and related dependencies:
// package id="Microsoft.Azure.Management.Authorization" version="2.0.0"
// package id="Microsoft.Azure.Management.ResourceManager" version="1.4.0-preview"
// package id="Microsoft.Rest.ClientRuntime.Azure.Authentication" version="2.2.8-preview"
  
using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Azure.Management.ResourceManager.Models;
using Microsoft.Rest.Azure.Authentication;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using task_2.Models;
using Microsoft.Extensions.Configuration;
using task_2.Contracts;
using Microsoft.Extensions.FileProviders;
using Microsoft.Azure.Management.Fluent;

namespace task_2.Services
{
    /// <summary>
    /// This is a helper class for deploying an Azure Resource Manager template
    /// More info about template deployments can be found here https://go.microsoft.com/fwLink/?LinkID=733371
    /// </summary>
    public class DeploymentService : IDeploymentService
    {
        private string _subscriptionId;
        private string _clientSecret;
        private string _clientId;
        private string _pathToTemplateFile;
        private string _pathToParameterFile;
        private string _tenantId;
        private string _resourceGroupLocation;

        public DeploymentService(IConfiguration config)
        {
            this._subscriptionId = config.GetValue<string>("SubscriptionId");
            this._tenantId = config.GetValue<string>("TennantId");
            this._clientId = config.GetValue<string>("ClientId");
            this._clientSecret = config.GetValue<string>("ClientSecret");
            this._resourceGroupLocation = config.GetValue<string>("ResourceGroupLocation");
        }
        
        public async void RunAsync(ArmTemplateParams tParams)
        {
            // Try to obtain the service credentials
            var serviceCreds = await ApplicationTokenProvider.LoginSilentAsync(_tenantId, _clientId, _clientSecret);

            // Read the template and parameter file contents
            dynamic templateFileContents = GetJsonFileContents("./ArmConfig\\template.json");
            dynamic parameterFileContents = GetJsonFileContents("./ArmConfig\\parameters.json");

            dynamic tags = new JObject();
            tags.ServiceOwner =  tParams.ServiceOwner;
            tags.Environment =  tParams.EnvironmentType;
            tags.WarsawApiUrl    =  tParams.WarsawApiUrl;
            parameterFileContents.tags = tags;
            parameterFileContents.parameters.sku = tParams.HostingPlanName;
            // parameterFileContents.parameters.hostingPlanName holds something like ASP-test01-a832
            
            // todo: resource group name
            // todo: resource name

            // Create or check that resource group exists
            var resourceManagementClient = new ResourceManagementClient(serviceCreds);
            resourceManagementClient.SubscriptionId = this._subscriptionId;
            EnsureResourceGroupExists(resourceManagementClient, tParams.ResourceGroup, this._resourceGroupLocation);          

            // Start a deployment
            // todo: deployment name
            DeployTemplate(resourceManagementClient, tParams.ResourceGroup, "deployment01", templateFileContents, parameterFileContents);
        }

        /// <summary>
        /// Reads a JSON file from the specified path
        /// </summary>
        /// <param name="pathToJson">The full path to the JSON file</param>
        /// <returns>The JSON file contents</returns>
        private JObject GetJsonFileContents(string pathToJson)
        {
            JObject templatefileContent = new JObject();
            using (StreamReader file = File.OpenText(pathToJson))
            {
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    templatefileContent = (JObject)JToken.ReadFrom(reader);
                    return templatefileContent;
                }
            }
        }

        /// <summary>
        /// Ensures that a resource group with the specified name exists. If it does not, will attempt to create one.
        /// </summary>
        /// <param name="resourceManagementClient">The resource manager client.</param>
        /// <param name="resourceGroupName">The name of the resource group.</param>
        /// <param name="resourceGroupLocation">The resource group location. Required when creating a new resource group.</param>
        private static void EnsureResourceGroupExists(ResourceManagementClient resourceManagementClient, string resourceGroupName, string resourceGroupLocation)
        {
            if (resourceManagementClient.ResourceGroups.CheckExistence(resourceGroupName) != true)
            {
                Console.WriteLine(string.Format("Creating resource group '{0}' in location '{1}'", resourceGroupName, resourceGroupLocation));
                var resourceGroup = new ResourceGroup();
                resourceGroup.Location = resourceGroupLocation;
                resourceManagementClient.ResourceGroups.CreateOrUpdate(resourceGroupName, resourceGroup);
            }
            else
            {
                Console.WriteLine(string.Format("Using existing resource group '{0}'", resourceGroupName));
            }
        }

        /// <summary>
        /// Starts a template deployment.
        /// </summary>
        /// <param name="resourceManagementClient">The resource manager client.</param>
        /// <param name="resourceGroupName">The name of the resource group.</param>
        /// <param name="deploymentName">The name of the deployment.</param>
        /// <param name="templateFileContents">The template file contents.</param>
        /// <param name="parameterFileContents">The parameter file contents.</param>
        private static void DeployTemplate(ResourceManagementClient resourceManagementClient, string resourceGroupName, string deploymentName, JObject templateFileContents, JObject parameterFileContents)
        {
            Console.WriteLine(string.Format("Starting template deployment '{0}' in resource group '{1}'", deploymentName, resourceGroupName));
            var deployment = new Deployment();

            deployment.Properties = new DeploymentProperties
            {
                Mode = DeploymentMode.Incremental,
                Template = templateFileContents,
                Parameters = parameterFileContents["parameters"].ToObject<JObject>()
            };

            var deploymentResult = resourceManagementClient.Deployments.CreateOrUpdate(resourceGroupName, deploymentName, deployment);
            Console.WriteLine(string.Format("Deployment status: {0}", deploymentResult.Properties.ProvisioningState));
        }
    }
}