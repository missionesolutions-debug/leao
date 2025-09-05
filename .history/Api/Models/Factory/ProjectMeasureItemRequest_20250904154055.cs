namespace Api.Models.Factory
{
    public class ProjectMeasureItemRequest
    {
        public int Id { get; set; }
        public int ProjectBlockId { get; set; }
        public required string Name { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string Length { get; set; }
        public string Weight { get; set; }
        public bool NameEditable { get; set; }
    }
}
