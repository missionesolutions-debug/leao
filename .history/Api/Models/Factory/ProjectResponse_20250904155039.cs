using Api.Models.Auth;

namespace Api.Models.Factory
{
    public class ProjectResponse
    {
        public int Id { get; set; }
        public required string Guid { get; set; }
        public required string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public DateTime? DatePrevisioned { get; set; }
        public required string Observations { get; set; }
        public required string PdfReportTotal { get; set; }
        public required ClientResponse Client { get; set; }
        public required SupplierResponse Supplier { get; set; }
        public required ProjectStatusResponse ProjectStatus { get; set; }
        public required List<ProjectItemResponse> Items { get; set; }
        public required List<UserResponse> AssignedUsers { get; set; }
    }
}
