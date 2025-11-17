namespace StorageStrategy.Domain.Commands.Products;

public record GenerateCodeCommand(int CompanyId) : CommandBase
{
    public string NewCode { get; set; } = string.Empty;
}