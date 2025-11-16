using Microsoft.Extensions.Options;
using Minio;
using Minio.ApiEndpoints;
using Minio.DataModel.Args;
using Minio.Exceptions;
using StorageStrategy.Utils.Services;

namespace StorageStrategy.Domain.Services.MinioStorage;

public class StorageFile : IStorageFile
{
    private readonly IMinioClient _client;

    public StorageFile(IOptions<AppSettings> options)
    {
        var cfg = options.Value.Minio;

        _client = new MinioClient()
            .WithEndpoint(cfg.Endpoint)
            .WithCredentials(cfg.AccessKey, cfg.SecretKey)
            .WithSSL(cfg.UseSSL)
            .Build();
    }

    public async Task<bool> BucketExistsAsync(string bucketName)
    {
        return await _client.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName));
    }

    public async Task CreateBucketAsync(string bucketName)
    {
        if (!await BucketExistsAsync(bucketName))
        {
            await _client.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName));
        }
    }

    public async Task UploadAsync(string bucketName, string objectName, Stream data, string contentType)
    {
        await _client.PutObjectAsync(new PutObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName)
            .WithStreamData(data)
            .WithObjectSize(data.Length)
            .WithContentType(contentType)
        );
    }

    public async Task<Stream> DownloadAsync(string bucketName, string objectName)
    {
        var ms = new MemoryStream();
        await _client.GetObjectAsync(new GetObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName)
            .WithCallbackStream(stream => stream.CopyTo(ms))
        );
        ms.Position = 0;
        return ms;
    }

    public async Task DeleteAsync(string bucketName, string objectName)
    {
        await _client.RemoveObjectAsync(new RemoveObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName)
        );
    }

    public async Task<bool> ExistsAsync(string bucketName, string objectName)
    {
        try
        {
            await _client.StatObjectAsync(new StatObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
            );
            return true;
        }
        catch (MinioException)
        {
            return false;
        }
    }

    public async Task<IEnumerable<string>> ListAsync(string bucketName, string? prefix = null)
    {
        var results = new List<string>();

        var args = new ListObjectsArgs()
            .WithBucket(bucketName)
            .WithPrefix(prefix ?? string.Empty)
            .WithRecursive(true);

        // await foreach (var obj in _client.ListObjectsAsync(args))
        // {
        //     results.Add(obj.Key);
        // }

        return results;
    }

    public async Task<string> PresignedGetUrlAsync(string bucketName, string objectName, int expiresInSeconds = 3600)
    {
        return await _client.PresignedGetObjectAsync(new PresignedGetObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName)
            .WithExpiry(expiresInSeconds)
        );
    }

    public async Task<string> PresignedPutUrlAsync(string bucketName, string objectName, int expiresInSeconds = 3600)
    {
        return await _client.PresignedPutObjectAsync(new PresignedPutObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName)
            .WithExpiry(expiresInSeconds)
        );
    }
}