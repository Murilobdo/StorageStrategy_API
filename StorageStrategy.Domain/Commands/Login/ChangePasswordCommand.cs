namespace StorageStrategy.Domain.Commands.Login
{
    public record class ChangePasswordCommand : CommandBase
    {
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public string NewPassword { get; set; } = string.Empty;
    }
}