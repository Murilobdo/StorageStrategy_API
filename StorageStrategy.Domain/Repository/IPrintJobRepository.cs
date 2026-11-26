using StorageStrategy.Models;

namespace StorageStrategy.Domain.Repository;

public interface IPrintJobRepository : IRepositoryBase<PrintJobEntity>
{
    /// <summary>Comanda com itens, cliente, funcionário e empresa, filtrada pela empresa.</summary>
    Task<CommandEntity?> GetCommandForPrintAsync(int commandId, int companyId);

    Task<PrintJobEntity?> GetByIdForCompanyAsync(int printJobId, int companyId);
    Task<PrintJobEntity?> GetLatestByCommandAsync(int commandId, int companyId);

    /// <summary>Job da mesma comanda e funcionário criado a partir de <paramref name="since"/> (duplo toque).</summary>
    Task<PrintJobEntity?> GetRecentDuplicateAsync(int commandId, int employeeId, int companyId, DateTime since);

    Task<int> CountPrintedAsync(int commandId, int companyId);

    /// <summary>
    /// Reserva de forma atômica o job Aguardando mais antigo da empresa (Aguardando → Imprimindo).
    /// Duas chamadas simultâneas nunca recebem o mesmo job.
    /// </summary>
    Task<PrintJobEntity?> ClaimNextAsync(int companyId, int printStationId, Guid claimId, DateTime now);

    Task<List<PrintJobEntity>> GetWaitingCreatedBeforeAsync(DateTime limit);
    Task<List<PrintJobEntity>> GetClaimedBeforeAsync(DateTime limit);
    Task<int> DeleteCreatedBeforeAsync(DateTime limit);
}
