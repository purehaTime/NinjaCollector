using DbService.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using ILogger = Serilog.ILogger;

namespace DbService.Repositories
{
    public class GridFsRepository : IGridFsRepository
    {
        private readonly ILogger _logger;
        private readonly IMongoClient _mongoClient;
        private readonly string _dbName;

        public GridFsRepository(IMongoClient client, string dbName, ILogger logger)
        {
            _logger = logger;
            _mongoClient = client;
            _dbName = dbName;
        }
        public async Task<byte[]> GetFileAsBytes(ObjectId id, GridFSDownloadOptions options, CancellationToken cToken)
        {
            try
            {
                var grid = InitGridFs();
                var result = await grid.DownloadAsBytesAsync(id, options, cToken);

                return result;
            }
            catch (Exception err)
            {
                _logger.Error(err, $"Error in GridFs.GetFileAsBytes. Details: {err.Message}");
            }

            return null!;
        }

        public async Task<MemoryStream> GetFileAsStream(ObjectId id, GridFSDownloadOptions options, CancellationToken cToken)
        {
            try
            {
                var grid = InitGridFs();
                var result = new MemoryStream();
                await grid.DownloadToStreamAsync(id, result, options, cToken);

                return result;
            }
            catch (Exception err)
            {
                _logger.Error(err, $"Error in GridFs.GetFileAsStream. Details: {err.Message}");
            }

            return null!;
        }

        public async Task<ObjectId?> AddFileAsBytes(byte[] fileBytes, string fileName, GridFSUploadOptions options, CancellationToken cToken)
        {
            try
            {
                var grid = InitGridFs();
                var result = await grid.UploadFromBytesAsync(fileName, fileBytes, options, cToken);

                return result;
            }
            catch (Exception err)
            {
                _logger.Error(err, $"Error in GridFs.AddFileAsBytes. Details: {err.Message}");
            }

            return null;
        }

        public async Task<ObjectId?> AddFileAsStream(MemoryStream fileStream, string fileName, GridFSUploadOptions options, CancellationToken cToken)
        {
            try
            {
                var grid = InitGridFs();
                var result = await grid.UploadFromStreamAsync(fileName, fileStream, options, cToken);

                return result;
            }
            catch (Exception err)
            {
                _logger.Error(err, $"Error in GridFs.AddFileAsStream. Details: {err.Message}");
            }

            return null;
        }

        protected GridFSBucket InitGridFs()
        {
            var db = _mongoClient
                .GetDatabase(_dbName);

            return new GridFSBucket(db);
        }
    }
}
