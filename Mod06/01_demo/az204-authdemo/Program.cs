using System;
using Microsoft.Identity.Client;
using System.Threading.Tasks;

namespace az204_authdemo
{
    class Program
    {

        private const string _clientId = "2839f402-c340-4894-bafa-16e1baf62ff4";
        private const string _tenantId = "ebc33cd3-ef97-4ba5-9043-8023d10664b1";
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var app = PublicClientApplicationBuilder
                .Create(_clientId)
                .WithAuthority(AzureCloudInstance.AzurePublic, _tenantId)
                .WithRedirectUri("http://localhost:8085")
                .Build();

            string[] scopes = { "user.read" };

            AuthenticationResult result = await app.AcquireTokenInteractive(scopes).ExecuteAsync();

            Console.WriteLine($"Token:\t{result.AccessToken}");
        }
    }
}
