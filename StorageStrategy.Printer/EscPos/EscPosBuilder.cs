using System.Globalization;
using System.Text;

namespace StorageStrategy.Printer.EscPos;

/// <summary>Monta a sequência de bytes ESC/POS para impressoras térmicas de 58mm.</summary>
public sealed class EscPosBuilder
{
    private const byte Esc = 0x1B;
    private const byte Gs = 0x1D;
    private const byte Lf = 0x0A;

    private readonly List<byte> _bytes = new();
    private readonly ReceiptEncoding _encoding;
    private readonly Encoding _textEncoding;

    static EscPosBuilder()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    public EscPosBuilder(ReceiptEncoding encoding)
    {
        _encoding = encoding;
        _textEncoding = encoding switch
        {
            ReceiptEncoding.Cp860 => Encoding.GetEncoding(860, new EncoderReplacementFallback("?"), DecoderFallback.ReplacementFallback),
            ReceiptEncoding.Cp850 => Encoding.GetEncoding(850, new EncoderReplacementFallback("?"), DecoderFallback.ReplacementFallback),
            _ => Encoding.GetEncoding("us-ascii", new EncoderReplacementFallback("?"), DecoderFallback.ReplacementFallback),
        };
    }

    /// <summary><c>ESC @</c>: reinicia a impressora (limpa estilos do documento anterior).</summary>
    public EscPosBuilder Initialize()
    {
        _bytes.AddRange(new byte[] { Esc, 0x40 });
        return this;
    }

    /// <summary><c>ESC t n</c>: seleciona a página de código.</summary>
    public EscPosBuilder SelectCodePage()
    {
        byte page = _encoding switch
        {
            ReceiptEncoding.Cp860 => 3,
            ReceiptEncoding.Cp850 => 2,
            _ => 0,
        };
        _bytes.AddRange(new byte[] { Esc, 0x74, page });
        return this;
    }

    /// <summary><c>ESC E n</c>: negrito.</summary>
    public EscPosBuilder Bold(bool on)
    {
        _bytes.AddRange(new byte[] { Esc, 0x45, (byte)(on ? 1 : 0) });
        return this;
    }

    /// <summary><c>GS ! n</c>: altura dupla (a largura continua a mesma, então as 32 colunas se mantêm).</summary>
    public EscPosBuilder DoubleHeight(bool on)
    {
        _bytes.AddRange(new byte[] { Gs, 0x21, (byte)(on ? 0x01 : 0x00) });
        return this;
    }

    public EscPosBuilder Line(string text)
    {
        _bytes.AddRange(_textEncoding.GetBytes(Normalize(text)));
        _bytes.Add(Lf);
        return this;
    }

    /// <summary><c>ESC d n</c>: avança n linhas.</summary>
    public EscPosBuilder Feed(int lines)
    {
        _bytes.AddRange(new byte[] { Esc, 0x64, (byte)Math.Clamp(lines, 0, 255) });
        return this;
    }

    public byte[] ToArray() => _bytes.ToArray();

    private string Normalize(string text) =>
        _encoding == ReceiptEncoding.AsciiSemAcentos ? TextLayout.RemoveAccents(text) : text;
}

/// <summary>Regras de texto compartilhadas pelo renderizador e pelo builder.</summary>
public static class TextLayout
{
    private static readonly CultureInfo PtBr = CultureInfo.GetCultureInfo("pt-BR");

    public static string Money(decimal value) => value.ToString("N2", PtBr);

    public static string RemoveAccents(string text)
    {
        var decomposed = text
            .Replace('ª', 'a')
            .Replace('º', 'o')
            .Normalize(NormalizationForm.FormD);

        var builder = new StringBuilder(decomposed.Length);
        foreach (var c in decomposed)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.NonSpacingMark)
                continue;
            builder.Append(c <= 0x7F ? c : '?');
        }

        return builder.ToString().Normalize(NormalizationForm.FormC);
    }

    /// <summary>Quebra por palavra; palavra maior que a linha é quebrada no limite. Nunca trunca.</summary>
    public static List<string> Wrap(string text, int columns)
    {
        var lines = new List<string>();
        var current = new StringBuilder();

        foreach (var rawWord in text.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            var word = rawWord;
            while (word.Length > columns)
            {
                if (current.Length > 0)
                {
                    lines.Add(current.ToString());
                    current.Clear();
                }
                lines.Add(word[..columns]);
                word = word[columns..];
            }

            if (current.Length == 0)
                current.Append(word);
            else if (current.Length + 1 + word.Length <= columns)
                current.Append(' ').Append(word);
            else
            {
                lines.Add(current.ToString());
                current.Clear().Append(word);
            }
        }

        if (current.Length > 0)
            lines.Add(current.ToString());

        if (lines.Count == 0)
            lines.Add(string.Empty);

        return lines;
    }

    public static string Center(string text, int columns)
    {
        if (text.Length >= columns)
            return text;

        var left = (columns - text.Length) / 2;
        return new string(' ', left) + text;
    }

    /// <summary>
    /// Item em uma linha: <c>{qtd}x {nome}</c> à esquerda e total à direita. Nome longo quebra por palavra e
    /// continua alinhado abaixo do nome; se nem o começo couber, o valor vai para a linha de baixo.
    /// </summary>
    public static IEnumerable<string> ItemLines(int qtd, string name, string total, int columns)
    {
        var prefix = $"{qtd}x ";
        var firstWidth = columns - prefix.Length - total.Length - 1;

        if (firstWidth < 6)
        {
            foreach (var line in Wrap(prefix + name, columns))
                yield return line;
            yield return total.PadLeft(columns);
            yield break;
        }

        var first = Wrap(name, firstWidth);
        yield return (prefix + first[0]).PadRight(columns - total.Length) + total;

        var rest = string.Join(' ', first.Skip(1));
        if (rest.Length == 0)
            yield break;

        var indent = new string(' ', prefix.Length);
        foreach (var line in Wrap(rest, columns - prefix.Length))
            yield return indent + line;
    }

    /// <summary>Texto à esquerda e valor alinhado à direita na mesma linha; se não couber, valor na linha de baixo.</summary>
    public static IEnumerable<string> LeftRight(string left, string right, int columns)
    {
        if (left.Length + 1 + right.Length <= columns)
        {
            yield return left + new string(' ', columns - left.Length - right.Length) + right;
            yield break;
        }

        foreach (var line in Wrap(left, columns))
            yield return line;

        yield return right.PadLeft(columns);
    }
}
