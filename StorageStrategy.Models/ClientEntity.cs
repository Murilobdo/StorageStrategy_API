namespace StorageStrategy.Models;

public class ClientEntity
{
    public ClientEntity()
    {
        
    }
    
    public ClientEntity(int companyId, string name)
    {
        ClientId = 0;
        CompanyId = companyId;
        Name = name;
        CreateAt = DateTime.Now;
    }

    public int ClientId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreateAt { get; set; }
    public bool Active { get; set; }
    public int CompanyId { get; set; }
    public CompanyEntity Company { get; set; }
    public List<CommandEntity> Commands { get; set; } = new();

   
}