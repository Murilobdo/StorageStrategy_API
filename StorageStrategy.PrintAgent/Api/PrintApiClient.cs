using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;

namespace StorageStrategy.PrintAgent.Api;

public record ClaimedJob(int PrintJobId, Guid ClaimId, string PayloadBase64, int Copy);

public enum ClaimStatus
{
    Job,
    Empty,
    Unauthorized,
    RateLimited,
    Error
}

public record ClaimResult(ClaimStatus Status, ClaimedJob? Job = null, TimeSpan? RetryAfter = null);

/// <summary>Cliente HTTP do agente: só HTTPS de saída, autenticado com a chave da estação (contracts/print-agent.md).</summary>
public class PrintApiClient
{
    private const int ResultAttempts = 5;

    private readonly HttpClient _http;
    private readonly ILogger<PrintApiClient> _logger;

    public PrintApiClient(HttpClient http, IOptions<PrintAgentOptions> options, ILogger<PrintApiClient> logger)
    {
        _http = http;
        _logger = logger;

        var baseUrl = options.Value.ApiBaseUrl.EndsWith('/') ? options.Value.ApiBaseUrl : options.Value.ApiBaseUrl + "/";
        _http.BaseAddress = new Uri(baseUrl);
        _http.Timeout = TimeSpan.FromSeconds(30);
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("PrintStation", options.Value.StationKey);
    }

    public async Task<ClaimResult> ClaimAsync(CancellationToken ct)
    {
        try
        {
            using var response = await _http.PostAsync("print/agent/claim", content: null, ct);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    var job = await response.Content.ReadFromJsonAsync<ClaimedJob>(cancellationToken: ct);
                    return job is null ? new ClaimResult(ClaimStatus.Error) : new ClaimResult(ClaimStatus.Job, job);
                case HttpStatusCode.NoContent:
                    return new ClaimResult(ClaimStatus.Empty);
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.Forbidden:
                    return new ClaimResult(ClaimStatus.Unauthorized);
                case HttpStatusCode.TooManyRequests:
                    return new ClaimResult(ClaimStatus.RateLimited, RetryAfter: RetryAfter(response));
                default:
                    _logger.LogWarning("API respondeu {StatusCode} ao buscar a fila", (int)response.StatusCode);
                    return new ClaimResult(ClaimStatus.Error);
            }
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException && !ct.IsCancellationRequested)
        {
            _logger.LogWarning("Sem conexão com a API: {Message}", ex.Message);
            return new ClaimResult(ClaimStatus.Error);
        }
    }

    /// <summary>Informa o resultado; repete até 5 vezes com o mesmo ClaimId (a API é idempotente).</summary>
    public async Task<bool> ReportAsync(ClaimedJob job, bool success, string? failureReason, CancellationToken ct)
    {
        var body = new { claimId = job.ClaimId, success, failureReason };
        var delay = TimeSpan.FromSeconds(2);

        for (var attempt = 1; attempt <= ResultAttempts; attempt++)
        {
            try
            {
                using var response = await _http.PostAsJsonAsync($"print/agent/jobs/{job.PrintJobId}/result", body, ct);
                if (response.IsSuccessStatusCode)
                    return true;

                if (response.StatusCode is HttpStatusCode.Conflict or HttpStatusCode.NotFound)
                {
                    _logger.LogWarning("Resultado do job {PrintJobId} recusado pela API ({StatusCode})", job.PrintJobId, (int)response.StatusCode);
                    return false;
                }

                if (response.StatusCode == HttpStatusCode.TooManyRequests)
                    delay = RetryAfter(response) ?? delay;
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException && !ct.IsCancellationRequested)
            {
                _logger.LogWarning("Falha ao enviar resultado do job {PrintJobId} (tentativa {Attempt}): {Message}", job.PrintJobId, attempt, ex.Message);
            }

            if (attempt < ResultAttempts)
            {
                await Task.Delay(delay, ct);
                delay = TimeSpan.FromSeconds(Math.Min(delay.TotalSeconds * 2, 60));
            }
        }

        _logger.LogError("Não foi possível informar o resultado do job {PrintJobId}; a API devolverá o job à fila em 2 min", job.PrintJobId);
        return false;
    }

    private static TimeSpan? RetryAfter(HttpResponseMessage response) =>
        response.Headers.RetryAfter?.Delta
        ?? (response.Headers.RetryAfter?.Date is { } date ? date - DateTimeOffset.UtcNow : null);
}
