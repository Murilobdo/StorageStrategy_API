using System.Text;
using StorageStrategy.Printer;
using Xunit;

namespace StorageStrategy.Tests.Printer
{
    /// <summary>US2: papel legível em 58mm (contracts/receipt-layout.md).</summary>
    public class ReceiptRendererTest
    {
        private readonly ReceiptRenderer _renderer = new();
        private static readonly ReceiptOptions Opcoes = new();

        static ReceiptRendererTest()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        private static ReceiptDocument Exemplo(int copy = 1, decimal desconto = 5m, decimal acrescimo = 9.89m, string? cliente = "João da Silva") => new()
        {
            CompanyName = "BAR DO ZÉ LTDA",
            CommandCode = "A12",
            CommandName = "Mesa 4",
            ClientName = cliente,
            EmployeeName = "Maria",
            OpenedAt = new DateTime(2026, 11, 26, 20, 15, 0),
            Copy = copy,
            Discount = desconto,
            Increase = acrescimo,
            Items = new[]
            {
                new ReceiptItem("Porção de Calabresa Acebolada com Fritas", 1, 49.90m),
                new ReceiptItem("Cerveja Long Neck", 3, 12.00m),
                new ReceiptItem("Refrigerante Lata", 2, 6.50m),
            }
        };

        private string[] Linhas(ReceiptDocument doc, ReceiptOptions? opcoes = null) =>
            _renderer.RenderText(doc, opcoes ?? Opcoes).Split('\n');

        [Fact(DisplayName = "Layout igual ao contrato de 58mm")]
        public void Layout_igual_ao_contrato()
        {
            var esperado = string.Join('\n', new[]
            {
                "         BAR DO ZÉ LTDA",
                "      CONFERÊNCIA DE CONTA",
                "--------------------------------",
                "Comanda: #A12 - Mesa 4",
                "Cliente: João da Silva",
                "Atendente: Maria",
                "Aberta em: 26/11/2026 20:15",
                "--------------------------------",
                "1x Porção de Calabresa     49,90",
                "   Acebolada com Fritas",
                "3x Cerveja Long Neck       36,00",
                "2x Refrigerante Lata       13,00",
                "--------------------------------",
                "Subtotal                   98,90",
                "Desconto                   -5,00",
                "Acréscimo                  +9,89",
                "TOTAL                  R$ 103,79",
                "--------------------------------",
                "     Não é documento fiscal",
            });

            Assert.Equal(esperado, _renderer.RenderText(Exemplo(), Opcoes));
        }

        [Fact(DisplayName = "Nenhuma linha passa de 32 colunas, mesmo com textos longos")]
        public void Nenhuma_linha_passa_de_32_colunas()
        {
            var doc = Exemplo() with
            {
                CompanyName = "RESTAURANTE E CHOPERIA DO ZÉ DA ESQUINA LTDA ME",
                CommandName = "Mesa 12 varanda fundos perto da janela",
                Items = new[]
                {
                    new ReceiptItem("Supercalifragilisticexpialidocious-extraordinário", 1, 10m),
                    new ReceiptItem("Picanha", 1000, 12345.67m),
                }
            };

            Assert.All(Linhas(doc), l => Assert.True(l.Length <= 32, $"Linha com {l.Length} colunas: '{l}'"));
        }

        [Fact(DisplayName = "Nome longo quebra por palavra sem cortar texto")]
        public void Nome_longo_quebra_sem_cortar()
        {
            var linhas = Linhas(Exemplo());

            Assert.Contains("1x Porção de Calabresa     49,90", linhas);
            Assert.Contains("   Acebolada com Fritas", linhas);
        }

        [Fact(DisplayName = "Quantidade e total grandes continuam alinhados sem passar de 32 colunas")]
        public void Quantidade_gigante()
        {
            var doc = Exemplo() with { Items = new[] { new ReceiptItem("Picanha na chapa", 1000, 12345.67m) } };

            var linhas = Linhas(doc);

            Assert.Contains("1000x Picanha na".PadRight(19) + "12.345.670,00", linhas);
            Assert.Contains("      chapa", linhas);
            Assert.All(linhas, l => Assert.True(l.Length <= 32));
        }

        [Fact(DisplayName = "Palavra maior que a linha é quebrada no limite, sem perder letras")]
        public void Palavra_gigante_quebrada_no_limite()
        {
            var palavra = new string('X', 40);
            var doc = Exemplo() with { Items = new[] { new ReceiptItem(palavra, 1, 1m) } };

            var linhas = Linhas(doc);
            var textoDoItem = string.Concat(linhas
                .SkipWhile(l => !l.StartsWith("1x "))
                .TakeWhile(l => !l.StartsWith("---"))
                .Select(l => l.Replace("1x ", "").Replace("1,00", "").Trim()));

            Assert.Equal(palavra, textoDoItem);
        }

        [Fact(DisplayName = "Valores dos itens alinhados à direita, uma linha por item")]
        public void Valores_alinhados_a_direita()
        {
            var linhasDeValor = Linhas(Exemplo()).Where(l => System.Text.RegularExpressions.Regex.IsMatch(l, @"^\d+x ")).ToList();

            Assert.Equal(3, linhasDeValor.Count);
            Assert.All(linhasDeValor, l => Assert.Equal(32, l.Length));
        }

        [Fact(DisplayName = "Sem desconto, sem acréscimo e sem cliente as linhas são omitidas")]
        public void Linhas_opcionais_omitidas()
        {
            var texto = _renderer.RenderText(Exemplo(desconto: 0, acrescimo: 0, cliente: null), Opcoes);

            Assert.DoesNotContain("Desconto", texto);
            Assert.DoesNotContain("Acréscimo", texto);
            Assert.DoesNotContain("Cliente:", texto);
            Assert.Contains("TOTAL                   R$ 98,90", texto);
        }

        [Fact(DisplayName = "Primeira via não mostra VIA; segunda via mostra 2ª VIA")]
        public void Segunda_via()
        {
            Assert.DoesNotContain("VIA", _renderer.RenderText(Exemplo(copy: 1), Opcoes));
            Assert.Contains("2ª VIA", _renderer.RenderText(Exemplo(copy: 2), Opcoes));
            Assert.Contains("3ª VIA", _renderer.RenderText(Exemplo(copy: 3), Opcoes));
        }

        [Fact(DisplayName = "Moeda em pt-BR com milhar e vírgula decimal")]
        public void Moeda_pt_br()
        {
            var doc = Exemplo(desconto: 0, acrescimo: 0) with { Items = new[] { new ReceiptItem("Rodada", 2, 1234.5m) } };

            Assert.Contains("TOTAL                R$ 2.469,00", Linhas(doc));
        }

        [Fact(DisplayName = "ESC/POS começa com ESC @ e seleciona CP860")]
        public void Escpos_inicializa_e_seleciona_cp860()
        {
            var bytes = _renderer.RenderEscPos(Exemplo(), Opcoes);

            Assert.Equal(new byte[] { 0x1B, 0x40, 0x1B, 0x74, 3 }, bytes.Take(5).ToArray());
        }

        [Fact(DisplayName = "Acentos codificados em CP860")]
        public void Acentos_em_cp860()
        {
            var bytes = _renderer.RenderEscPos(Exemplo(), Opcoes);
            var cp860 = Encoding.GetEncoding(860);

            Assert.True(Contem(bytes, cp860.GetBytes("Porção")));
            Assert.True(Contem(bytes, cp860.GetBytes("CONFERÊNCIA")));
            Assert.False(Contem(bytes, Encoding.UTF8.GetBytes("ç")));
        }

        [Fact(DisplayName = "CP850 seleciona a página 2")]
        public void Cp850()
        {
            var bytes = _renderer.RenderEscPos(Exemplo(), Opcoes with { Encoding = ReceiptEncoding.Cp850 });

            Assert.Equal(new byte[] { 0x1B, 0x74, 2 }, bytes.Skip(2).Take(3).ToArray());
            Assert.True(Contem(bytes, Encoding.GetEncoding(850).GetBytes("Porção")));
        }

        [Fact(DisplayName = "Modo sem acentos gera só ASCII")]
        public void Sem_acentos_so_ascii()
        {
            var opcoes = Opcoes with { Encoding = ReceiptEncoding.AsciiSemAcentos };

            var texto = _renderer.RenderText(Exemplo(copy: 2), opcoes);
            var bytes = _renderer.RenderEscPos(Exemplo(copy: 2), opcoes);

            Assert.Contains("1x Porcao de Calabresa", texto);
            Assert.Contains("2a VIA", texto);
            Assert.All(bytes.Skip(5), b => Assert.True(b < 0x80));
        }

        [Fact(DisplayName = "Termina com avanço de 4 linhas e sem comando de corte")]
        public void Termina_com_avanco_sem_corte()
        {
            var bytes = _renderer.RenderEscPos(Exemplo(), Opcoes);

            Assert.Equal(new byte[] { 0x1B, 0x64, 4 }, bytes[^3..]);
            Assert.False(Contem(bytes, new byte[] { 0x1D, 0x56 }));
        }

        [Fact(DisplayName = "TOTAL sai em negrito e altura dupla")]
        public void Total_em_destaque()
        {
            var bytes = _renderer.RenderEscPos(Exemplo(), Opcoes);
            var destaque = new byte[] { 0x1B, 0x45, 1, 0x1D, 0x21, 0x01 };

            Assert.True(Contem(bytes, destaque.Concat(Encoding.ASCII.GetBytes("TOTAL")).ToArray()));
        }

        [Fact(DisplayName = "Comanda com 50 itens sai completa")]
        public void Cinquenta_itens()
        {
            var itens = Enumerable.Range(1, 50).Select(i => new ReceiptItem($"Produto {i}", 1, 1m)).ToArray();
            var doc = Exemplo(desconto: 0, acrescimo: 0) with { Items = itens };

            var linhas = Linhas(doc);

            Assert.Contains(linhas, l => l.StartsWith("1x Produto 1 "));
            Assert.Contains(linhas, l => l.StartsWith("1x Produto 50 "));
            Assert.Contains("TOTAL                   R$ 50,00", linhas);
        }

        private static bool Contem(byte[] origem, byte[] trecho)
        {
            for (var i = 0; i <= origem.Length - trecho.Length; i++)
            {
                if (origem.AsSpan(i, trecho.Length).SequenceEqual(trecho))
                    return true;
            }
            return false;
        }
    }
}
