using Newtonsoft.Json;

namespace StorageStrategy.Domain.Commands.NFE
{
    public record class ItemNFE
    {
        [JsonProperty("numero_item")]
        public int NumeroItem { get; set; }

        [JsonProperty("codigo_produto")]
        public int CodigoProduto { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; } = string.Empty;

        [JsonProperty("cfop")]
        public int CPOF { get; set; }

        [JsonProperty("unidade_comercial")]
        public string UnidadeComercial { get; set; } = string.Empty;

        [JsonProperty("quantidade_comercial")]
        public int QuantidadeComercial { get; set; }

        [JsonProperty("valor_unitario_comercial")]
        public decimal ValorUnitarioComercial { get; set; }

        [JsonProperty("valor_unitario_tributavel")]
        public decimal ValorUnitarioTributavel { get; set; }

        [JsonProperty("unidade_tributavel")]
        public string UnidadeTributavel { get; set; } = string.Empty;

        [JsonProperty("codigo_ncm")]
        public int CodigoNCM { get; set; }

        [JsonProperty("quantidade_tributavel")]
        public int QuantidadeTributavel { get; set; }

        [JsonProperty("valor_bruto")]
        public decimal ValorBruto { get; set; }

        [JsonProperty("icms_situacao_tributaria")]
        public int IcmsSituacaoTributaria { get; set; }

        [JsonProperty("icms_origem")]
        public int IcmsOrigem { get; set; }

        [JsonProperty("pis_situacao_tributaria")]
        public string PisSituacaoTributaria { get; set; } = string.Empty;

        [JsonProperty("cofins_situacao_tributaria")]
        public string MyProperty { get; set; } = string.Empty;
    }
}
