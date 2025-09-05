using Data.Models.Pessoa;
using Framework.Domain.Dtos.Profile;

namespace Framework.Services.Mapper.Profile
{
    public static class AccountMapper
    {
        public static Usuario MappingUser(CreateAccountResponse model)
        {
            return new Usuario
            {
                Nome = model.Nome,
                Login = model.Email,
                Email = model.Email,
                Password = HashPassword(model.Password),
                RoleGate = "Student", // Defina o papel padrão
                Ativo = true,
                Excluido = false,
                Destaque = false,
                Phone = model.Phone,
                DataCriacao = DateTime.Now,
                Genero = model.Genero,
                Logradouro = model.Logradouro,
                Estado = model.Estado,
                Cidade = model.Cidade,
                Bairro = model.Bairro,
                Complemento = model.Complemento,
                Numero = model.Numero,
                DataNascimento= model.DataNascimento,
                CEP = model.CEP,
                Cpf = model.CPF,
            };
        }

        private static string HashPassword(string password)
        {
            return password;
            // Implemente o hashing de senha aqui
            //return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
