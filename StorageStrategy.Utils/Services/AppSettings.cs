namespace StorageStrategy.Utils.Services
{
    public class AppSettings
    {
        public AppSettings()
        {
            
        }
        public string JwtKey { get; set; } = string.Empty;
        public string Database { get; set; } = string.Empty;
        public MinioSettings Minio { get; set; } = new();
    }
    
    public class MinioSettings
    {
        public string Endpoint { get; set; } = string.Empty;
        public string AccessKey { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public bool UseSSL { get; set; }
    }
}