namespace StorageStrategy.Utils.Services
{
    public class AppSettings
    {
        public AppSettings()
        {
            
        }
        public string JwtKey { get; set; } = string.Empty;
        public string Database { get; set; } = string.Empty;
    }
}