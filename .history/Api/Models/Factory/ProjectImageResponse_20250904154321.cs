namespace Api.Models.Factory
{
    public class ProjectImageResponse
    {
        public int Id { get; set; }
        public string UrlSource { get; set; }
        public int ProjectBlockId { get; set; }
        public DateTime DataCadastro { get; set; }
        public string Description { get; set; }
        public bool EnableOnReport { get; set; }
    }
}
