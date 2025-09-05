namespace Api.Models.Factory
{
    public class ProjectGroupResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProjectBlockResponse> Blocks { get; set; }
    }
}
