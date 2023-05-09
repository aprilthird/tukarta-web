using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TUKARTA.PE.SERVICE.Services.CloudStorage;

namespace TUKARTA.PE.SERVICE.Services.Interfaces
{
    public interface IBlobService
    {
        Task<BlobElementInfo> GetBlobAsync(string blobName, string container);
        Task<IEnumerable<string>> ListBlobsAsync(string container);
        Task<Uri> UploadFileBlobAsync(Stream stream, string blobName, string container);
        Task DeleteBlobAsync(string blobName, string container);
        Task DeleteBlobAsync(Uri uri);
    }
}
