using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using GeoTagger.Model;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace GeoTagger.Services
{
    public class EntryService
    {
        readonly string enrtyTable = "GeoTaggerEntries";
        //string AzureConnectionString = "";
        readonly string AzureConnectionString = "DefaultEndpointsProtocol=https;AccountName=mug2018xamarin;AccountKey=CHAR2bKmTymbHCZyue+e8sU6yHOw9i53+kvB4a/8v0ABzs2GrKG5fG2jMmnDZSQgfCSv4A1U3aEMaI/JyrW4WQ==;EndpointSuffix=core.windows.net";

        string nombreImagen; 

        public async Task<bool> SaveEntryAsync(GeoTaggerEntry entity)
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(AzureConnectionString);

                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                CloudTable table = tableClient.GetTableReference(enrtyTable);

                // Define the insert operation
                var operation = TableOperation.InsertOrReplace(entity);

                // Execute the insert against the table
                var result = await table.ExecuteAsync(operation);

                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<string> SaveImage(Stream image)
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(AzureConnectionString);
                var client = storageAccount.CreateCloudBlobClient();

                // Gets a reference to the images container
                var container = client.GetContainerReference("images");

                // Creates the container if it does not exist
                await container.CreateIfNotExistsAsync();

                // Uses a random name for the new images
                var name = DateTime.Now.Ticks.ToString()+".jpg";

                nombreImagen = name; //valerio

                // Uploads the image the blob storage
                var imageBlob = container.GetBlockBlobReference(name);
                await imageBlob.UploadFromStreamAsync(image);

                return "https://mug2018xamarin.blob.core.windows.net/images/" + name;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<List<GeoTaggerEntry>> GetLastEntries()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(AzureConnectionString);

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(enrtyTable);

            // Create the table query.
            TableQuery<GeoTaggerEntry> rangeQuery = new TableQuery<GeoTaggerEntry>();

            // Execute the query
            TableContinuationToken continueToken = null;
            var s = await table.ExecuteQuerySegmentedAsync(rangeQuery, continueToken);
            continueToken = s.ContinuationToken;

            return s.Results;
        }
    }
}
