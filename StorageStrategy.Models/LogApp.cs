namespace StorageStrategy.Models;

public class LogApp
{
    public int LogId { get; set; }
    public string JsonData { get; set; }
    public DateTime CreateAt { get; set; }
    public int EmployeeId { get; set; }
    public EmployeeEntity Employee { get; set; }
}