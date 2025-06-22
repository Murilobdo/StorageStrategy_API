namespace StorageStrategy.Models;

public class ClientEntity
{
    public int ClientId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreateAt { get; set; }
    public int CompanyId { get; set; }
    public CompanyEntity Company { get; set; }
    public List<CommandEntity> Commands { get; set; } = new();
}