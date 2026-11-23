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
        public MercadoPagoSettings MercadoPago { get; set; } = new();
        public PlatformAdminSettings PlatformAdmin { get; set; } = new();
    }

    /// <summary>
    /// Admin da plataforma. CompanyId identifica a "Admin Company" (<= 0 = não configurado: ninguém
    /// passa na política PlatformAdmin). InitialPassword só é usada para criar o admin quando ele não
    /// existe; deve vir de variável de ambiente (AppSettings__PlatformAdmin__InitialPassword) ou da
    /// configuração da VPS, nunca de arquivo versionado, e nunca é logada.
    /// </summary>
    public class PlatformAdminSettings
    {
        public int CompanyId { get; set; } = 1;
        public string InitialPassword { get; set; } = string.Empty;
    }

    public class MinioSettings
    {
        public string Endpoint { get; set; } = string.Empty;
        public string AccessKey { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public bool UseSSL { get; set; }
    }

    public class MercadoPagoSettings
    {
        public int DefaultExpirationMinutes { get; set; } = 3;
    }
}