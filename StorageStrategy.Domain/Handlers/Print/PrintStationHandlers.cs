using MediatR;
using Microsoft.Extensions.Logging;
using StorageStrategy.Domain.Commands.Print;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Domain.Services.Printing;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Handlers.Print;

public class GetPrintStationHandler : HandlerBase, IRequestHandler<GetPrintStationQuery, Result>
{
    private readonly IPrintStationRepository _stations;
    private readonly TimeProvider _time;

    public GetPrintStationHandler(IPrintStationRepository stations, TimeProvider time)
    {
        _stations = stations;
        _time = time;
    }

    public async Task<Result> Handle(GetPrintStationQuery request, CancellationToken cancellationToken)
    {
        var station = await _stations.GetActiveByCompanyIdAsync(request.CompanyId);
        var response = station is null
            ? PrintStationResponse.NotConfigured()
            : PrintStationResponse.From(station, PrintClock.Now(_time));

        return CreateResponse(response, "Busca realizada");
    }
}

/// <summary>Cria a estação (com chave nova, mostrada uma vez) ou atualiza nome/papel/codificação sem mudar a chave.</summary>
public class SavePrintStationHandler : HandlerBase, IRequestHandler<SavePrintStationCommand, Result>
{
    private readonly IPrintStationRepository _stations;
    private readonly TimeProvider _time;
    private readonly ILogger<SavePrintStationHandler> _logger;

    public SavePrintStationHandler(IPrintStationRepository stations, TimeProvider time, ILogger<SavePrintStationHandler> logger)
    {
        _stations = stations;
        _time = time;
        _logger = logger;
    }

    public async Task<Result> Handle(SavePrintStationCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid())
            return CreateError(request.GetErros(), "Dados inválidos");

        var now = PrintClock.Now(_time);
        var station = await _stations.GetActiveByCompanyIdAsync(request.CompanyId);
        string? key = null;

        if (station is null)
        {
            station = new PrintStationEntity
            {
                CompanyId = request.CompanyId,
                IsActive = true,
                CriadoEm = now
            };
            Apply(station, request, now);
            await _stations.AddAsync(station);
            await _stations.SaveAsync(); // gera o PrintStationId, que faz parte da chave

            key = PrintStationKey.Generate(station.PrintStationId, out var hash);
            station.KeyHash = hash;
            station.KeyLastChars = PrintStationKey.LastChars(key);
            _stations.Update(station);
            await _stations.SaveAsync();

            _logger.LogInformation("Estação de impressão criada: empresa {CompanyId}, estação {PrintStationId}",
                station.CompanyId, station.PrintStationId);
        }
        else
        {
            Apply(station, request, now);
            _stations.Update(station);
            await _stations.SaveAsync();
        }

        return CreateResponse(PrintStationResponse.From(station, now, key), "Impressora salva com sucesso!");
    }

    private static void Apply(PrintStationEntity station, SavePrintStationCommand request, DateTime now)
    {
        station.Name = request.Name.Trim();
        station.PaperWidthMm = request.PaperWidthMm;
        station.Encoding = request.Encoding;
        station.AtualizadoEm = now;
        station.AtualizadoPorEmployeeId = request.EmployeeId;
    }
}

/// <summary>Gera uma chave nova; a anterior deixa de valer na hora.</summary>
public class RegeneratePrintStationKeyHandler : HandlerBase, IRequestHandler<RegeneratePrintStationKeyCommand, Result>
{
    private readonly IPrintStationRepository _stations;
    private readonly TimeProvider _time;
    private readonly ILogger<RegeneratePrintStationKeyHandler> _logger;

    public RegeneratePrintStationKeyHandler(IPrintStationRepository stations, TimeProvider time, ILogger<RegeneratePrintStationKeyHandler> logger)
    {
        _stations = stations;
        _time = time;
        _logger = logger;
    }

    public async Task<Result> Handle(RegeneratePrintStationKeyCommand request, CancellationToken cancellationToken)
    {
        var station = await _stations.GetActiveByCompanyIdAsync(request.CompanyId);
        if (station is null)
            return CreateError("Nenhuma impressora configurada");

        var now = PrintClock.Now(_time);
        var key = PrintStationKey.Generate(station.PrintStationId, out var hash);
        station.KeyHash = hash;
        station.KeyLastChars = PrintStationKey.LastChars(key);
        station.AtualizadoEm = now;
        station.AtualizadoPorEmployeeId = request.EmployeeId;
        _stations.Update(station);
        await _stations.SaveAsync();

        _logger.LogInformation("Chave da estação de impressão regenerada: empresa {CompanyId}, estação {PrintStationId}",
            station.CompanyId, station.PrintStationId);

        return CreateResponse(PrintStationResponse.From(station, now, key), "Nova chave gerada. Atualize o agente de impressão.");
    }
}
