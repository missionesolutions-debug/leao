using Api.Models.Auth;

namespace Api.Models.Factory
{
    public class ProjectPhaseResponse
    {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsAproved { get; set; }
    public bool IsTaken { get; set; }
    public List<ProjectGroupResponse> Groups { get; set; } = new();
    public UserResponse AssignedUser { get; set; } = new();
    }
}
