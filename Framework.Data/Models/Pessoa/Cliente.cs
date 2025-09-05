using System;

namespace Data.Models.Pessoa
{
    public class Cliente : Base
    {
		public string Nome { get; set; }
		public string RazaoSocial { get; set; }
		public string CNPJ { get; set; }
		public string CEP { get; set; }
		public string Logradouro { get; set; }
		public string Numero { get; set; }
		public string Complemento { get; set; }
		public string Bairro { get; set; }
		public string Cidade { get; set; }
		public string Estado { get; set; }
		public string Pais { get; set; }
		public string TelefoneEmpresa { get; set; }
		public string WhatsappEmpresa { get; set; }
		public string Logo { get; set; }
		public string Email { get; set; }
		public string Facebook { get; set; }
		public string Instagram { get; set; }
		public string Linkedin { get; set; }
		public string Website { get; set; }
        public string Imagem { get; set; }
        public string ImagemAlt { get; set; }
        public string NomeResponsavel { get; set; }
		public string CPFResponsavel { get; set; }
		public string WhatsappResponsavel { get; set; }
		public string EmailResponsavel { get; set; }
        public Nullable<int> PaginaId { get; set; }
        public string Chave { get; set; }
    }
}
