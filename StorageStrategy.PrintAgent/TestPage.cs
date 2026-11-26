using StorageStrategy.Printer;
using StorageStrategy.PrintAgent.Printing;

namespace StorageStrategy.PrintAgent;

/// <summary>
/// <c>--test-page</c>: imprime localmente (sem API) a mesma conferência nas 3 codificações, para descobrir
/// qual o firmware da impressora entende (research R6). A escolhida vai na tela de configuração da impressora.
/// </summary>
public static class TestPage
{
    public static int Run(string printerName, IRawPrinter printer)
    {
        if (string.IsNullOrWhiteSpace(printerName))
        {
            Console.Error.WriteLine("Configure PrintAgent:PrinterName no appsettings.json antes da página de teste.");
            return 2;
        }

        var problem = printer.GetProblem(printerName);
        if (problem is not null)
        {
            Console.Error.WriteLine($"Impressora não está pronta: {problem}");
            return 3;
        }

        var renderer = new ReceiptRenderer();
        var options = new[]
        {
            (ReceiptEncoding.Cp860, "1 - Português (CP860)"),
            (ReceiptEncoding.Cp850, "2 - Multilíngue (CP850)"),
            (ReceiptEncoding.AsciiSemAcentos, "3 - Sem acentos"),
        };

        foreach (var (encoding, title) in options)
        {
            var document = new ReceiptDocument
            {
                CompanyName = "PÁGINA DE TESTE",
                Title = title,
                CommandCode = "TESTE",
                CommandName = "Ação, Coração, Pão",
                EmployeeName = "Conceição",
                OpenedAt = DateTime.Now,
                Items = new[]
                {
                    new ReceiptItem("Porção de Calabresa à Moda da Casa", 1, 49.90m),
                    new ReceiptItem("Açaí com Maçã e Limão", 2, 12.50m),
                },
                Footer = "Acentos certos? Use esta opção."
            };

            printer.Print(printerName, renderer.RenderEscPos(document, new ReceiptOptions { Encoding = encoding }), $"Teste {title}");
        }

        Console.WriteLine("Página de teste enviada. Veja qual bloco (1, 2 ou 3) saiu com os acentos corretos.");
        return 0;
    }
}
