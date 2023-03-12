using MongoDB.Bson;
using MongoDB.Driver.GridFS;

namespace DbService.Interfaces
{
    public interface IGridFsService
    {
        Task<byte[]> GetFileAsBytes(ObjectId id, GridFSDownloadOptions options, CancellationToken cToken);
        Task<MemoryStream> GetFileAsStream(ObjectId id, GridFSDownloadOptions options, CancellationToken cToken);
        Task<ObjectId?> AddFileAsBytes(byte[] fileBytes, string fileName, GridFSUploadOptions options, CancellationToken cToken);
        Task<ObjectId?> AddFileAsStream(MemoryStream fileStream, string fileName, GridFSUploadOptions options, CancellationToken cToken);
    }
}
