namespace Framework.Domain.Dtos.Profile
{
    public class UpdateAccountResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public UserDto UpdatedUser { get; set; }
    }
}
