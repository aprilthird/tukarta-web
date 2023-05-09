using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TUKARTA.PE.SERVICE.Services.CloudStorage;
using TUKARTA.PE.SERVICE.Extensions;
using TUKARTA.PE.SERVICE.Services.Interfaces;

namespace TUKARTA.PE.SERVICE.Services.Implementations
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<BlobElementInfo> GetBlobAsync(string blobName, string container)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(container);
            var blobClient = containerClient.GetBlobClient(blobName);
            var blobDownloadInfo = await blobClient.DownloadAsync();
            return new BlobElementInfo(blobDownloadInfo.Value.Content, blobDownloadInfo.Value.ContentType);
        }

        public async Task<IEnumerable<string>> ListBlobsAsync(string container)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(container);
            var items = new List<string>();
            await foreach(var blobItem in containerClient.GetBlobsAsync())
            {
                items.Add(blobItem.Name);
            }
            return items;
        }

        public async Task<Uri> UploadFileBlobAsync(Stream stream, string blobName, string container)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(container);
            if(!await containerClient.ExistsAsync())
            {
                await containerClient.CreateIfNotExistsAsync();
                await containerClient.SetAccessPolicyAsync(PublicAccessType.Blob);
            }
            var newBlobName = $"{Guid.NewGuid()}{Path.GetExtension(blobName)}";
            var blobClient = containerClient.GetBlobClient(newBlobName);
            //stream.Seek(0, SeekOrigin.Begin);
            await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = blobName.GetContentType() });
            return blobClient.Uri;
        }

        public async Task DeleteBlobAsync(string blobName, string container)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(container);
            var blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync();
        }

        public async Task DeleteBlobAsync(Uri uri)
        {
            var parts = uri.LocalPath.Split("/");
            var blob = parts.LastOrDefault();
            var container = parts.ElementAtOrDefault(parts.Count() - 2);
            var containerClient = _blobServiceClient.GetBlobContainerClient(container);
            var blobClient = containerClient.GetBlobClient(blob);
            await blobClient.DeleteIfExistsAsync();
        }
    }
}
