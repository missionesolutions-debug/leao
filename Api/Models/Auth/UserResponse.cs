namespace Api.Models.Auth
{
    public class UserResponse
    {
        //Id = user.Id, Nome = user.Nome, Email = user.Email, Role = user.RoleGate

            public int Id {  get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Role {  get; set; }
        public string Avatar { get; set; }
    }
}
