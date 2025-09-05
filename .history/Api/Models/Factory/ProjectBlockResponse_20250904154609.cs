namespace Api.Models.Factory
{
    public class ProjectBlockResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int MinImagesAmount { get; set; }
        public int MaxImagesAmount { get; set; }
        public bool ObservationsEnabled { get; set; }
        public required string Instructions { get; set; }
        public int? Position { get; set; }
        public required string Code { get; set; }
        public bool BoxTypeActive { get; set; }
        public bool CardBoardTypeActive { get; set; }
        public bool ClosureTypeActive { get; set; }
        public bool VersionActive { get; set; }
        public required string VersionInfo { get; set; }
        public required bool CodigoBarraActive { get; set; }
        public string CodigoBarraInfo { get; set; }
        public bool BlockForConclude { get; set; }
        public string ActionText { get; set; }
        public string ImagesLabel { get; set; }
        public bool IsCompleted { get; set; }
        public string Observation { get; set; }
        public string Version { get; set; }
        public string BoxType { get; set; }
        public string CardBoardType { get; set; }
        public string ClosureType { get; set; }

        // Novos campos para incluir os itens de medida e as imagens
        public List<ProjectMeasureItemResponse> MeasureItems { get; set; }
        public List<ProjectImageResponse> Images { get; set; }
        public List<ImagesGalleryResponse> ImagesGallery { get; set; }
    }
}
