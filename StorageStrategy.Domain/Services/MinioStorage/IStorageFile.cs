namespace StorageStrategy.Domain.Services.MinioStorage;

public interface IStorageFile
{
    Task<string> UploadProductPhotoAsync(string requestPhotoUrl, string nameCompany, int requestProductId);
    
    Task<bool> BucketExistsAsync(string bucketName);
    Task CreateBucketAsync(string bucketName);

    Task UploadAsync(string bucketName, string objectName, Stream file, string contentType);
    Task<Stream> DownloadAsync(string bucketName, string objectName);
    Task DeleteAsync(string bucketName, string objectName);

    Task<bool> ExistsAsync(string bucketName, string objectName);
    Task<IEnumerable<string>> ListAsync(string bucketName, string? prefix = null);

    Task<string> PresignedGetUrlAsync(string bucketName, string objectName, int expiresInSeconds = 3600);
    Task<string> PresignedPutUrlAsync(string bucketName, string objectName, int expiresInSeconds = 3600);
}