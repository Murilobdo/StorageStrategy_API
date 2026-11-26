using StorageStrategy.Models;
using StorageStrategy.Printer;

namespace StorageStrategy.Domain.Services.Printing;

/// <summary>Converte a comanda (entidades) nos dados do papel (classlib sem dependências).</summary>
public static class ReceiptDocumentFactory
{
    public static ReceiptDocument Create(CommandEntity command, int copy)
    {
        return new ReceiptDocument
        {
            CompanyName = command.Company?.Name ?? string.Empty,
            CommandCode = command.Code ?? command.CommandId.ToString(),
            CommandName = command.Name ?? string.Empty,
            ClientName = command.Client?.Name,
            EmployeeName = command.Employee?.Name ?? string.Empty,
            OpenedAt = command.InitialDate,
            Copy = copy,
            Discount = command.Discount,
            Increase = command.Increase,
            Items = command.Items
                .OrderBy(i => i.CommandItemId)
                .Select(i => new ReceiptItem(ProductName(i.Name), i.Qtd, i.Price))
                .ToList()
        };
    }

    /// <summary>
    /// O item da comanda guarda "CATEGORIA | PRODUTO"; no papel sai só o produto.
    /// </summary>
    public static string ProductName(string itemName)
    {
        if (string.IsNullOrWhiteSpace(itemName))
            return string.Empty;

        var separator = itemName.IndexOf('|');
        var name = separator >= 0 ? itemName[(separator + 1)..].Trim() : itemName.Trim();
        return name.Length > 0 ? name : itemName.Trim();
    }

    public static ReceiptOptions Options(PrintStationEntity station) => new()
    {
        Encoding = station.Encoding switch
        {
            PrintEncodingEnum.Cp850 => ReceiptEncoding.Cp850,
            PrintEncodingEnum.AsciiSemAcentos => ReceiptEncoding.AsciiSemAcentos,
            _ => ReceiptEncoding.Cp860
        }
    };
}

/// <summary>Relógio da fila de impressão: relógio local do servidor, mesmo padrão do resto da API.</summary>
public static class PrintClock
{
    public static DateTime Now(TimeProvider time) => time.GetLocalNow().DateTime;
}
