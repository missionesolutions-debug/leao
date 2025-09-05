using Api.Models.Auth;

namespace Api.Models.Factory
{
    public class ProjectPhaseResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAproved { get; set; }
        public bool IsTaken { get; set; }
        public List<ProjectGroupResponse> Groups { get; set; }
        public UserResponse AssignedUser { get; set; }
    }
}
