namespace Api.Models.Factory
{
    public class ClientResponse
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public required string CNPJ { get; set; }
    }
}
