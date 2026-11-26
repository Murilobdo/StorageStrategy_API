using StorageStrategy.Domain.Validations.Print;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Print;

/// <summary>Funcionário pede a impressão da conferência de conta de uma comanda.</summary>
public record CreatePrintJobCommand : CommandBase, IValidation
{
    public int CommandId { get; set; }
    public int CompanyId { get; set; }
    public int EmployeeId { get; set; }

    public bool IsValid() => new CreatePrintJobValidation().Validate(this).IsValid;

    public List<Error> GetErros() => new CreatePrintJobValidation().Validate(this)
        .Errors.Select(p => new Error(p.ErrorMessage)).ToList();
}

public record GetLatestPrintJobQuery(int CommandId, int CompanyId) : CommandBase;

public record CancelPrintJobCommand(int PrintJobId, int CompanyId) : CommandBase;

/// <summary>Agente pega o próximo job da fila. CompanyId e PrintStationId vêm da chave da estação.</summary>
public record ClaimPrintJobCommand(int CompanyId, int PrintStationId) : CommandBase;

public record ReportPrintResultCommand : CommandBase
{
    public int PrintJobId { get; set; }
    public int CompanyId { get; set; }
    public Guid ClaimId { get; set; }
    public bool Success { get; set; }
    public string? FailureReason { get; set; }
}

public record SavePrintStationCommand : CommandBase, IValidation
{
    public int CompanyId { get; set; }
    public int EmployeeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int PaperWidthMm { get; set; } = PrintStationEntity.DefaultPaperWidthMm;
    public PrintEncodingEnum Encoding { get; set; } = PrintEncodingEnum.Cp860;

    public bool IsValid() => new SavePrintStationValidation().Validate(this).IsValid;

    public List<Error> GetErros() => new SavePrintStationValidation().Validate(this)
        .Errors.Select(p => new Error(p.ErrorMessage)).ToList();
}

public record RegeneratePrintStationKeyCommand(int CompanyId, int EmployeeId) : CommandBase;

public record GetPrintStationQuery(int CompanyId) : CommandBase;

/// <summary>Agente confirma que a chave funciona (instalador e diagnóstico). Atualiza o último contato.</summary>
public record PingPrintStationQuery(int CompanyId, int PrintStationId) : CommandBase;
