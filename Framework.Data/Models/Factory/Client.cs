using System;
using System.ComponentModel.DataAnnotations;

namespace Framework.Data.Models.Factory
{
	// CLIENTE
	public class Client
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Nome { get; set; }

		[Required]
		public string CNPJ { get; set; }

		public string Endereco { get; set; }
		public string Telefone { get; set; }
		public string Email { get; set; }
		public string CEP { get; set; }
		public int? PorcentagemCaixas { get; set; } = 0;

		public int? PorcentagemCaixasMontadas { get; set; } = 0;
        public bool Ativo { get; set; } = true;
		public bool Excluido { get; set; } = false;
		public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
	}
}
