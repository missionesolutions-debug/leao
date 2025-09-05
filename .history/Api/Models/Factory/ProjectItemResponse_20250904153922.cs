namespace Api.Models.Factory
{
    public class ProjectItemResponse
    {
        public int Id { get; set; }
        public requiredstring Name { get; set; }
        public List<ProjectPhaseResponse> Phases { get; set; }
    }
}
