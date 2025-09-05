namespace Api.Models.Factory
{
    public class ProjectBlockEditRequest
    {
        public string? Code { get; set; }
        public int? UsuarioId { get; set; }
        public string? BoxType { get; set; }
        public string? CardBoardType { get; set; }
        public string? ClosureType { get; set; }
        public string? Observation { get; set; }
        public bool? ObservationsEnabled { get; set; }
        public string? Version { get; set; }
        // Itens opcionais:
        public List<ProjectMeasureItemRequest>? MeasureItems { get; set; }
        //public List<IFormFile> Files { get; set; }
    }
}
