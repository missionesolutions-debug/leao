namespace Api.Models.Factory
{
    public class ProjectItemResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<ProjectPhaseResponse> Phases { get; set; }
    }
}
