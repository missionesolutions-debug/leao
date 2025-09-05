using Api.Models.Auth;

namespace Api.Models.Factory
{
    public class ProjectResponse
    {
        public int Id { get; set; }
        public string Guid { get; set; }
        public required string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public DateTime? DatePrevisioned { get; set; }
        public string Observations { get; set; }
        public string PdfReportTotal { get; set; }
        public ClientResponse Client { get; set; }
        public SupplierResponse Supplier { get; set; }
        public ProjectStatusResponse ProjectStatus { get; set; }
        public List<ProjectItemResponse> Items { get; set; }
        public List<UserResponse> AssignedUsers { get; set; }
    }
}
