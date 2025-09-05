namespace Api.Models.Factory
{
    public class ProjectAssignUsersRequest
    {
        public int ProjectId { get; set; }
        public required List<int> UserIds { get; set; }
    }
}
