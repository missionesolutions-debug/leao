namespace Api.Models.Factory
{
    public class ProjectImageResponse
    {
        public int Id { get; set; }
        public required string UrlSource { get; set; }
        public int ProjectBlockId { get; set; }
        public DateTime DataCadastro { get; set; }
        public required string description { get; set; }
        public bool enableOnReport { get; set; }
    }
}
