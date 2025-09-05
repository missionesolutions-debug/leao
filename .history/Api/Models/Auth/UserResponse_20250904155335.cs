namespace Api.Models.Auth
{
    public class UserResponse
    {
        //Id = user.Id, Nome = user.Nome, Email = user.Email, Role = user.RoleGate

        public int Id { get; set; }
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public required string Role { get; set; }
        public required string Avatar { get; set; }
    }
}
