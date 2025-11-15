using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Command;

public record ListCommandsQuery : CommandBase
{
    public bool HaveEndDate { get; set; }
    public int CompanyId { get; set; }

    public ListCommandsQuery()
    {

    }

    public ListCommandsQuery(bool haveEndDate, int companyId)
    {
        HaveEndDate = haveEndDate;
        CompanyId = companyId;
    }
    
}
