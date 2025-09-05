namespace Api.Models.Factory
{
    public class ProjectMeasureItemResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Width { get; set; }
        public required string Height { get; set; }
        public required string Length { get; set; }
        public required string Weight { get; set; }
        public bool NameEditable { get; set; }
    }
}
