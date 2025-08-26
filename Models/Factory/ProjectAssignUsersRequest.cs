namespace Api.Models.Factory
{
    public class ProjectAssignUsersRequest
    {
        public int ProjectId { get; set; }
        public List<int> UserIds { get; set; }
    }
}
