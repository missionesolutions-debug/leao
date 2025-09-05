namespace Api.Models.Factory
{
    public class ProjectGroupResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required List<ProjectBlockResponse> Blocks { get; set; }
    }
}
