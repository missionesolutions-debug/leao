using Api.Models.Auth;

namespace Api.Models.Factory
{
    public class ProjectPhaseResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public bool IsAproved { get; set; }
        public bool IsTaken { get; set; }
        public required List<ProjectGroupResponse> Groups { get; set; }
        public required UserResponse AssignedUser { get; set; }
    }
}
