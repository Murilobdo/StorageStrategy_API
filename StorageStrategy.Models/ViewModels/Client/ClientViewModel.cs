namespace StorageStrategy.Models.ViewModels.Client;

public record ClientViewModel
{
    public int ClientId { get; set; }
    public string Name { get; set; }
    public DateTime CreateAt { get; set; }
    public int TotalCommands { get; set; }
    public bool Active { get; set; }
}