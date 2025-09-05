namespace Framework.Domain.Dtos.Profile
{
    public class LoginResult
    {
        public DataResult Data { get; set; } = new DataResult();
    }

    public class DataResult()
    {
        public string Token { get; set; }

    }
}
