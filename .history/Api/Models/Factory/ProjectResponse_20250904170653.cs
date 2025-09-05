using Api.Models.Auth;

namespace Api.Models.Factory
{
    public class ProjectResponse
    {
        public int Id { get; set; }
        public string Guid { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public DateTime? DatePrevisioned { get; set; }
        public string Observations { get; set; } = string.Empty;
        public string PdfReportTotal { get; set; } = string.Empty;
        public ClientResponse Client { get; set; } = new ClientResponse {
            Nome = string.Empty,
            CNPJ = string.Empty
        };
        public SupplierResponse Supplier { get; set; } = new SupplierResponse {
            Nome = string.Empty,
            CNPJ = string.Empty
        };
        public ProjectStatusResponse ProjectStatus { get; set; } = new ProjectStatusResponse {
            Nome = string.Empty
        };
        public List<ProjectItemResponse> Items { get; set; } = new();
        public List<UserResponse> AssignedUsers { get; set; } = new();
        public object AssignedUser { get; set; } = new();
    }
}
