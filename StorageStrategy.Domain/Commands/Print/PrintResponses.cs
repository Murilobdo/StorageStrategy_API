using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Print;

public record PrintJobResponse(
    int PrintJobId,
    int CommandId,
    int Copy,
    PrintJobStatusEnum Status,
    string StatusName,
    string? FailureReason,
    DateTime CreatedAt,
    DateTime? CompletedAt,
    bool StationOnline)
{
    public static PrintJobResponse From(PrintJobEntity job, bool stationOnline) => new(
        job.PrintJobId,
        job.CommandId,
        job.Copy,
        job.Status,
        job.Status.ToString(),
        job.FailureReason,
        job.CreatedAt,
        job.CompletedAt,
        stationOnline);
}

public record ClaimedPrintJobResponse(int PrintJobId, Guid ClaimId, string PayloadBase64, int Copy);

public enum PrintResultOutcome
{
    Accepted = 1,
    NotFound = 2,
    Conflict = 3
}

public record PrintStationResponse
{
    public bool IsConfigured { get; init; }
    public int? PrintStationId { get; init; }
    public string? Name { get; init; }
    public int? PaperWidthMm { get; init; }
    public PrintEncodingEnum? Encoding { get; init; }
    public string? KeyLastChars { get; init; }
    public bool IsOnline { get; init; }
    public int? SecondsSinceLastSeen { get; init; }

    /// <summary>Só preenchida na criação ou regeneração; nunca é armazenada em texto puro.</summary>
    public string? StationKey { get; init; }

    public static PrintStationResponse NotConfigured() => new() { IsConfigured = false };

    public static PrintStationResponse From(PrintStationEntity station, DateTime now, string? stationKey = null) => new()
    {
        IsConfigured = true,
        PrintStationId = station.PrintStationId,
        Name = station.Name,
        PaperWidthMm = station.PaperWidthMm,
        Encoding = station.Encoding,
        KeyLastChars = station.KeyLastChars,
        IsOnline = station.IsOnline(now),
        SecondsSinceLastSeen = station.LastSeenAt is null ? null : (int)Math.Max(0, (now - station.LastSeenAt.Value).TotalSeconds),
        StationKey = stationKey
    };
}

public record PrintStationPingResponse(int PrintStationId, string StationName, int CompanyId, string CompanyName);
