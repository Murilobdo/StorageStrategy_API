using StorageStrategy.Printer.EscPos;

namespace StorageStrategy.Printer;

/// <summary>
/// Conferência de conta para papel de 58mm (contrato: specs/006-order-thermal-print/contracts/receipt-layout.md).
/// </summary>
public sealed class ReceiptRenderer : IReceiptRenderer
{
    private sealed record ReceiptLine(string Text, bool Emphasis = false);

    public string RenderText(ReceiptDocument document, ReceiptOptions options)
    {
        var lines = BuildLines(document, options.Columns).Select(l => l.Text);
        var text = string.Join('\n', lines);
        return options.Encoding == ReceiptEncoding.AsciiSemAcentos ? TextLayout.RemoveAccents(text) : text;
    }

    public byte[] RenderEscPos(ReceiptDocument document, ReceiptOptions options)
    {
        var builder = new EscPosBuilder(options.Encoding)
            .Initialize()
            .SelectCodePage();

        foreach (var line in BuildLines(document, options.Columns))
        {
            if (line.Emphasis)
            {
                builder.Bold(true).DoubleHeight(true).Line(line.Text).DoubleHeight(false).Bold(false);
            }
            else
            {
                builder.Line(line.Text);
            }
        }

        return builder.Feed(options.FeedLinesAtEnd).ToArray();
    }

    private static List<ReceiptLine> BuildLines(ReceiptDocument doc, int columns)
    {
        var lines = new List<ReceiptLine>();
        var separator = new ReceiptLine(new string('-', columns));

        void Add(IEnumerable<string> texts) => lines.AddRange(texts.Select(t => new ReceiptLine(t)));
        void Centered(string text) => Add(TextLayout.Wrap(text, columns).Select(t => TextLayout.Center(t, columns)));

        // Cabeçalho
        if (!string.IsNullOrWhiteSpace(doc.CompanyName))
            Centered(doc.CompanyName.Trim());
        Centered(doc.Title);
        if (doc.Copy > 1)
            Centered($"{doc.Copy}ª VIA");
        lines.Add(separator);

        // Identificação
        var comanda = $"Comanda: #{doc.CommandCode}";
        if (!string.IsNullOrWhiteSpace(doc.CommandName))
            comanda += $" - {doc.CommandName.Trim()}";
        Add(TextLayout.Wrap(comanda, columns));
        if (!string.IsNullOrWhiteSpace(doc.ClientName))
            Add(TextLayout.Wrap($"Cliente: {doc.ClientName.Trim()}", columns));
        if (!string.IsNullOrWhiteSpace(doc.EmployeeName))
            Add(TextLayout.Wrap($"Atendente: {doc.EmployeeName.Trim()}", columns));
        Add(new[] { $"Aberta em: {doc.OpenedAt:dd/MM/yyyy HH:mm}" });
        lines.Add(separator);

        // Itens: "2x NOME DO PRODUTO        12,00" (nome longo continua alinhado abaixo do nome)
        foreach (var item in doc.Items)
            Add(TextLayout.ItemLines(item.Qtd, item.Name.Trim(), TextLayout.Money(item.Total), columns));
        lines.Add(separator);

        // Totais
        Add(TextLayout.LeftRight("Subtotal", TextLayout.Money(doc.Subtotal), columns));
        if (doc.Discount > 0)
            Add(TextLayout.LeftRight("Desconto", "-" + TextLayout.Money(doc.Discount), columns));
        if (doc.Increase > 0)
            Add(TextLayout.LeftRight("Acréscimo", "+" + TextLayout.Money(doc.Increase), columns));
        lines.AddRange(TextLayout.LeftRight("TOTAL", "R$ " + TextLayout.Money(doc.Total), columns)
            .Select(t => new ReceiptLine(t, Emphasis: true)));
        lines.Add(separator);

        // Rodapé
        if (!string.IsNullOrWhiteSpace(doc.Footer))
            Centered(doc.Footer);

        return lines;
    }
}
