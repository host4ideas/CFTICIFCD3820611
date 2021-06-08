using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace az204_cosmosdemo
{
    class Program
    {
        private static readonly string EndpointUri = "https://felixcosmosdb.documents.azure.com:443/";
        private static readonly string PrimaryKey = "UrgJedpt1CD4USei3Nrj1twOQLJ5Jte6i2JPWsDXV8dtsA2QynP9XDu81VE150fbDbxMxJvXtHnK7uNXHfN6FQ==";

        private CosmosClient cosmosClient;

        private Database database;

        private Container container;

        private string databaseId = "az204Database";
        private string containerId = "az204Container";

        public async Task CosmosDemoAsync()
        {
            this.cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);

            // Runs the CreateDatabaseAsync method
            await this.CreateDatabaseAsync();

            // Run the CreateContainerAsync method
            await this.CreateContainerAsync();
        }
        public static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine("Beginning operations...\n");
                Program p = new Program();
                await p.CosmosDemoAsync();

            }
            catch (CosmosException de)
            {
                Exception baseException = de.GetBaseException();
                Console.WriteLine("{0} error occurred: {1}", de.StatusCode, de);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e);
            }
            finally
            {
                Console.WriteLine("End of demo, press any key to exit.");
                Console.ReadKey();
            }
        }

        private async Task CreateDatabaseAsync()
        {
            // Create a new database
            this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
            Console.WriteLine("Created Database: {0}\n", this.database.Id);
        }

        private async Task CreateContainerAsync()
        {
            // Create a new container
            this.container = await this.database.CreateContainerIfNotExistsAsync(containerId, "/LastName");
            Console.WriteLine("Created Container: {0}\n", this.container.Id);
        }
    }
}
