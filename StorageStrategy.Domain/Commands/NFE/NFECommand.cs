using Newtonsoft.Json;

namespace StorageStrategy.Domain.Commands.NFE
{
    public record class NFECommand : CommandBase
    {
        [JsonProperty("natureza_operacao")]
        public string NaturezaOperacao { get; set; } = string.Empty;

        [JsonProperty("data_emissao")]
        public string DataEmissao { get; set; } = string.Empty;

        [JsonProperty("data_entrada_saida")]
        public string DataEntradaSaida { get; set; } = string.Empty;

        [JsonProperty("tipo_documento")]
        public int TipoDocumento { get; set; }

        [JsonProperty("finalidade_emissao")]
        public string FinalidadeEmissao { get; set; } = string.Empty;

        [JsonProperty("cnpj_emitente")]
        public string CnpjEmitente { get; set; } = string.Empty;

        [JsonProperty("cpf_emitente")]
        public string CpfEmitente { get; set; } = string.Empty;

        [JsonProperty("nome_emitente")]
        public string NomeEmitente { get; set; } = string.Empty;

        [JsonProperty("nome_fantasia_emitente")]
        public string NomeFantasiaEmitente { get; set; } = string.Empty;

        [JsonProperty("numero_emitente")]
        public int NumeroEmitente { get; set; }

        [JsonProperty("bairro_emitente")]
        public string BairroEmitente { get; set; } = string.Empty;

        [JsonProperty("municipio_emitente")]
        public string MunicipioEmitente { get; set; } = string.Empty;

        [JsonProperty("uf_emitente")]
        public string UfEmitente { get; set; } = string.Empty;

        [JsonProperty("cep_emitente")]
        public string CepEmitente { get; set; } = string.Empty;

        [JsonProperty("inscricao_estadual_emitente")]
        public string InscricaoEstadualEmitente { get; set; } = string.Empty;

        [JsonProperty("nome_destinatario")]
        public string NomeDestinatario { get; set; } = string.Empty;

        [JsonProperty("cpf_destinatario")]
        public string CpfDestinatario { get; set; } = string.Empty;

        [JsonProperty("municipio_emitente")]
        public string inscricao_estadual_destinatario { get; set; } = null;

        [JsonProperty("telefone_destinatario")]
        public string TelefoneDestinatario { get; set; } = string.Empty;

        [JsonProperty("logradouro_destinatario")]
        public string LogradouroDestinatario { get; set; } = string.Empty;

        [JsonProperty("numero_destinatario")]
        public int NumeroDestinatario { get; set; }

        [JsonProperty("bairro_destinatario")]
        public string BairroDestinatario { get; set; } = string.Empty;

        [JsonProperty("municipio_destinatario")]
        public string MunicipioDestinatario { get; set; } = string.Empty;

        [JsonProperty("uf_destinatario")]
        public string UfDestinatario { get; set; } = string.Empty;

        [JsonProperty("pais_destinatario")]
        public string PaisDestinatario { get; set; } = string.Empty;

        [JsonProperty("cep_destinatario")]
        public int CepDestinatario { get; set; }

        [JsonProperty("valor_frete")]
        public int ValorFrete { get; set; }

        [JsonProperty("valor_seguro")]
        public int ValorSeguro { get; set; }

        [JsonProperty("valor_total")]
        public int ValorTotal { get; set; }

        [JsonProperty("valor_produtos")]
        public int ValorProdutos { get; set; }

        [JsonProperty("modalidade_frete")]
        public int MensalidadeFrete { get; set; }

        public List<ItemNFE> MyProperty { get; set; } = new();

    }
}
