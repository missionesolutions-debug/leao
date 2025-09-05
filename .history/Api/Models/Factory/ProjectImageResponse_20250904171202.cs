namespace Api.Models.Factory
{
    public class ProjectImageResponse
    {
        public int Id { get; set; }
        public string UrlSource { get; set; } = string.Empty;
        public int ProjectBlockId { get; set; }
        public DateTime DataCadastro { get; set; }
        public string Description { get; set; } = string.Empty;
    public bool EnableOnReport { get; set; } = false;
    }
}
