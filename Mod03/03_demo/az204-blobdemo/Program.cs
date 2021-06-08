using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.IO;
using System.Threading.Tasks;

using static System.Console;

namespace az204_blobdemo
{
    class Program
    {
        public static void Main()
        {
            WriteLine("Azure Blob Storage Demo\n");

            // Run the examples asynchronously, wait for the results before proceeding
            ProcessAsync().GetAwaiter().GetResult();

            WriteLine("Press enter to exit the sample application.");
            ReadLine();

        }

        private static async Task ProcessAsync()
        {
            // Copy the connection string from the portal in the variable below.
            string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=az204blobdemo68995;AccountKey=7iHPRhVk66ZiLFOTfNmEsFfTNwfnar+bCv4HqtmsW4UMwDjO5f7qjfiKeRcTVRO9l+1ydvpO3Mv/MB3dH47SZQ==;EndpointSuffix=core.windows.net";


            // Create a client that can authenticate with a connection string
            BlobServiceClient blobServiceClient = new BlobServiceClient(storageConnectionString);


            //Create a unique name for the container
            string containerName;
            containerName = $"demoblob{Guid.NewGuid().ToString()}";


            // Create the container and return a container client object
            BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);
            WriteLine("A container named '" + containerName + "' has been created. " +
                "\nTake a minute and verify in the portal." +
                "\nNext a file will be created and uploaded to the container.");
            WriteLine("Press 'Enter' to continue.");
            ReadLine();


            // Create a local file in the ./data/ directory for uploading and downloading
            string localPath = "./data/";
            string fileName = $"demofile{Guid.NewGuid().ToString()}.txt";
            string localFilePath = Path.Combine(localPath, fileName);


            // Write text to the file
            await File.WriteAllTextAsync(localFilePath, $"Hello World-{DateTime.Now}");


            // Get a reference to the blob
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);


            // Open the file and upload its data
            using FileStream uploadFileStream = File.OpenRead(localFilePath);
            await blobClient.UploadAsync(uploadFileStream, true);
            uploadFileStream.Close();

            WriteLine("\nThe file was uploaded. We'll verify by listing" +
                    " the blobs next.");
            WriteLine("Press 'Enter' to continue.");
            ReadLine();


            // List blobs in the container
            WriteLine("Listing blobs...");
            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
            {
                WriteLine("\t" + blobItem.Name);
            }

            WriteLine("\nYou can also verify by looking inside the " +
                    "container in the portal." +
                    "\nNext the blob will be downloaded with an altered file name.");
            WriteLine("Press 'Enter' to continue.");
            ReadLine();


            // Download the blob to a local file
            // Append the string "DOWNLOADED" before the .txt extension
            string downloadFilePath = localFilePath.Replace(".txt", "DOWNLOADED.txt");

            Console.WriteLine("\nDownloading blob to\n\t{0}\n", downloadFilePath);

            // Download the blob's contents and save it to a file
            BlobDownloadInfo download = await blobClient.DownloadAsync();

            using (FileStream downloadFileStream = File.OpenWrite(downloadFilePath))
            {
                await download.Content.CopyToAsync(downloadFileStream);
                downloadFileStream.Close();
            }
            WriteLine("\nLocate the local file to verify it was downloaded.");
            WriteLine("The next step is to delete the container and local files.");
            WriteLine("Press 'Enter' to continue.");
            ReadLine();


            // Delete the container and clean up local files created
            WriteLine("\n\nDeleting blob container...");
            await containerClient.DeleteAsync();


            WriteLine("Compruebe ficheros en data\tPress 'Enter' to continue:");
            ReadLine();
            WriteLine("Deleting the local source and downloaded files...");
            File.Delete(localFilePath);
            File.Delete(downloadFilePath);

            WriteLine("Finished cleaning up.");
        }
    }
}
