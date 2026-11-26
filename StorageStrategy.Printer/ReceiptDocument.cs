namespace StorageStrategy.Printer;

/// <summary>Codificação de acentos enviada à impressora (research R6).</summary>
public enum ReceiptEncoding
{
    /// <summary>Página de código 860 (português): <c>ESC t 3</c>.</summary>
    Cp860 = 1,

    /// <summary>Página de código 850 (multilíngue): <c>ESC t 2</c>.</summary>
    Cp850 = 2,

    /// <summary>Remove os acentos. Funciona em qualquer firmware.</summary>
    AsciiSemAcentos = 3
}

public sealed record ReceiptItem(string Name, int Qtd, decimal UnitPrice)
{
    public decimal Total => Qtd * UnitPrice;
}

/// <summary>Dados da conferência de conta, sem dependência de entidades ou banco.</summary>
public sealed record ReceiptDocument
{
    public string CompanyName { get; init; } = string.Empty;
    public string Title { get; init; } = "CONFERÊNCIA DE CONTA";
    public string CommandCode { get; init; } = string.Empty;
    public string CommandName { get; init; } = string.Empty;
    public string? ClientName { get; init; }
    public string EmployeeName { get; init; } = string.Empty;

    /// <summary>Data e hora em que a comanda foi aberta (impressa no cabeçalho).</summary>
    public DateTime OpenedAt { get; init; }

    /// <summary>Número da via; maior que 1 imprime "2ª VIA", "3ª VIA"...</summary>
    public int Copy { get; init; } = 1;

    public IReadOnlyList<ReceiptItem> Items { get; init; } = Array.Empty<ReceiptItem>();
    public decimal Discount { get; init; }
    public decimal Increase { get; init; }
    public string Footer { get; init; } = "Não é documento fiscal";

    public decimal Subtotal => Items.Sum(i => i.Total);
    public decimal Total => Subtotal - Discount + Increase;
}

public sealed record ReceiptOptions
{
    /// <summary>Papel de 58mm na fonte A: 32 colunas.</summary>
    public int Columns { get; init; } = 32;

    public ReceiptEncoding Encoding { get; init; } = ReceiptEncoding.Cp860;

    /// <summary>Linhas em branco no final (a MP58C7 não tem guilhotina).</summary>
    public int FeedLinesAtEnd { get; init; } = 4;
}

public interface IReceiptRenderer
{
    /// <summary>Texto do papel (para testes e pré-visualização), linhas separadas por '\n'.</summary>
    string RenderText(ReceiptDocument document, ReceiptOptions options);

    /// <summary>Bytes ESC/POS prontos para a impressora.</summary>
    byte[] RenderEscPos(ReceiptDocument document, ReceiptOptions options);
}
