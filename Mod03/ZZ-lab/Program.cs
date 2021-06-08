using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Threading.Tasks;
using static System.Console;

public class Program
{
    private const string blobServiceEndpoint = "https://mediastorfelixmb.blob.core.windows.net/";
    private const string storageAccountName = "mediastorfelixmb";
    private const string storageAccountKey = "RRQ9GU0chDXFTwdx11sgxkWQSlkMhQycqeWIIeIsMPRanlhe06PP0RZrN1xIibYxhhIAb0NrmBHqjRV1Rafniw==";

    public static async Task Main(string[] args)
    {
        StorageSharedKeyCredential accountCredentials = new StorageSharedKeyCredential(storageAccountName, storageAccountKey);

        BlobServiceClient serviceClient = new BlobServiceClient(new Uri(blobServiceEndpoint), accountCredentials);

        AccountInfo info = await serviceClient.GetAccountInfoAsync();

        await Out.WriteLineAsync($"Connected to Azure Storage Account");
        await Out.WriteLineAsync($"Account name:\t{storageAccountName}");
        await Out.WriteLineAsync($"Account kind:\t{info?.AccountKind}");
        await Out.WriteLineAsync($"Account sku:\t{info?.SkuName}");

        await EnumerateContainersAsync(serviceClient);

        string existingContainerName = "raster-graphics";

        await EnumerateBlobsAsync(serviceClient, existingContainerName);

        string newContainerName = "vector-graphics";

        BlobContainerClient containerClient = await GetContainerAsync(serviceClient, newContainerName);

        string uploadedBlobName = "graph.svg";

        BlobClient blobClient = await GetBlobAsync(containerClient, uploadedBlobName);

        await Console.Out.WriteLineAsync($"Blob Url:\t{blobClient.Uri}");

    }

    private static async Task EnumerateContainersAsync(BlobServiceClient client)
    {
        await foreach (BlobContainerItem container in client.GetBlobContainersAsync())
        {
            await Console.Out.WriteLineAsync($"Container:\t{container.Name}");
        }
    }

    private static async Task EnumerateBlobsAsync(BlobServiceClient client, string containerName)
    {
        BlobContainerClient container = client.GetBlobContainerClient(containerName);

        await Console.Out.WriteLineAsync($"Searching:\t{container.Name}");

        await foreach (BlobItem blob in container.GetBlobsAsync())
        {
            await Console.Out.WriteLineAsync($"Existing Blob:\t{blob.Name}");
        }
    }

    private static async Task<BlobContainerClient> GetContainerAsync(BlobServiceClient client, string containerName)
    {
        BlobContainerClient container = client.GetBlobContainerClient(containerName);
        await container.CreateIfNotExistsAsync(PublicAccessType.Blob);

        await Console.Out.WriteLineAsync($"New Container:\t{container.Name}");

        return container;
    }

    private static async Task<BlobClient> GetBlobAsync(BlobContainerClient client, string blobName)
    {
        BlobClient blob = client.GetBlobClient(blobName);

        await Console.Out.WriteLineAsync($"Blob Found:\t{blob.Name}");

        return blob;

    }

}
