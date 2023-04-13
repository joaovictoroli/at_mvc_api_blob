using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Domain.Services
{
    public class BlobAzure
    {
        private const string connectionString = "DefaultEndpointsProtocol=https;AccountName=atpbzure;AccountKey=0U1KRu6g7dMsyKUvkc6P8fC0+CPBzhS7N8JcUDSSe7N0PT0ChX4U09jyNYFDaU+f/BuUECSxeGBh+ASts2Ob9A==;EndpointSuffix=core.windows.net";
        private const string containerName = "images";

        public static async Task<string> UploadImage(IFormFile imageFile)
        {
            var reader = imageFile.OpenReadStream();
            var cloundStorageAccount = CloudStorageAccount.Parse(connectionString);
            var blobClient = cloundStorageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(containerName);
            await container.CreateIfNotExistsAsync();
            string path = Guid.NewGuid().ToString();
            CloudBlockBlob blob = container.GetBlockBlobReference(path);
            await blob.UploadFromStreamAsync(reader);
            return blob.Uri.ToString();
        }

        public static void DeletePhoto(string Url)
        {
            if (Url != null)
            {
                try
                {
                    string nomeArquivo = Url.Split("/" + containerName + "/")[1];
                    var blobClient = new BlobClient(connectionString, containerName, nomeArquivo);
                    blobClient.Delete();
                }
                catch { }
            }
        }

    }
}
