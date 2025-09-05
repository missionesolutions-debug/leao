namespace Api.Models.Factory
{
    public class ProjectItemResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required List<ProjectPhaseResponse> Phases { get; set; }
    }
}
