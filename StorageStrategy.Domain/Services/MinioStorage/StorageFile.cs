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
    private const string BucketName = "storage-strategy";
    private MinioSettings _minio;
    public StorageFile(IOptions<AppSettings> options)
    {
        _minio = options.Value.Minio;

        _client = new MinioClient()
            .WithEndpoint(_minio.Endpoint)
            .WithCredentials(_minio.AccessKey, _minio.SecretKey)
            .WithSSL(_minio.UseSSL)
            .Build();
    }

    public async Task<string> UploadProductPhotoAsync(string photo, string nameCompany, int productId)
    {
        nameCompany = nameCompany.Replace(" ", "-").ToLower();
        photo = photo.Split("base64,")[1];
        byte[] imageBytes = Convert.FromBase64String(photo);
        using var stream = new MemoryStream(imageBytes);
        
        await CreateBucketAsync(BucketName);
        
        var objectName = $"company-{nameCompany}-product-{Guid.NewGuid()}.jpeg";
        
        await UploadAsync(
            $"{BucketName}",
            objectName,
            stream,
            "image/jpeg"
        );
        
        return $"https://{_minio.Endpoint}/{BucketName}/{objectName}";
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