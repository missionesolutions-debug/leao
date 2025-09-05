using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Framework.Domain.Dtos.Profile
{
    public class CreateAccountResponse
    {
        [Required]
        [JsonProperty("nome")]
        public string Nome { get; set; }

        [Required]
        [JsonProperty("sobrenome")]
        public string Sobrenome { get; set; }

        [Required]
        [JsonProperty("cpf")]
        public string CPF { get; set; }

        [Required]
        [EmailAddress]
        [JsonProperty("email")]
        public string Email { get; set; }

        [Required]
        [JsonProperty("phone")]
        public string Phone { get; set; }

        [Required]
        [JsonProperty("password")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "As Senhas não coincidem.")]
        [JsonProperty("confirmPassword")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "Genero é obrigatório.")]
        public string Genero { get; set; }

        [Required(ErrorMessage = "Logradouro é obrigatório.")]
        public string Logradouro { get; set; }

        [Required(ErrorMessage = "Cep é obrigatório.")]
        public string CEP { get; set; }
        [Required(ErrorMessage = "Cidade é obrigatório.")]
        public string Cidade { get; set; }
        [Required(ErrorMessage = "Estado é obrigatório.")]
        public string Estado { get; set; }
        [Required(ErrorMessage = "Bairro é obrigatório.")]
        public string Bairro { get; set; }
        public string Complemento { get; set; }
        [Required(ErrorMessage = "Número é obrigatório.")]
        public string Numero { get; set; }
    }
}
