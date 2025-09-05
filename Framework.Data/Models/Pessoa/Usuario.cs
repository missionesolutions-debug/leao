using System;

namespace Data.Models.Pessoa
{
	public class Usuario : Base
    {
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RoleGate { get; set; }
        public string Avatar { get; set; }
        public string Imagem { get; set; }
        public string PasswordToken { get; set; } 
        public DateTime? PasswordTokenExpiry { get; set; }
        public int? SubscriptionId { get; set; } 
        public int? JourneyId { get; set; }
        public string Guid { get; set; }
        public string Token { get; set; }

        public DateTime DataNascimento { get; set; }

        public string Genero { get; set; }
        public string Logradouro { get; set; }

        public string CEP { get; set; }

        public string Cidade { get; set; }

        public string Estado { get; set; }

        public string Bairro { get; set; }

        public string Complemento { get; set; }

        public string Numero { get; set; }

        public string Phone { get; set; }

        public string Cpf { get; set; }


   
    }
}
