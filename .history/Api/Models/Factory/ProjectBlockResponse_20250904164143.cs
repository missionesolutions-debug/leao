namespace Api.Models.Factory
{
    public class ProjectBlockResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MinImagesAmount { get; set; }
        public int MaxImagesAmount { get; set; }
        public bool ObservationsEnabled { get; set; }
        public string Instructions { get; set; }
        public int? Position { get; set; }
        public string Code { get; set; }
        public bool BoxTypeActive { get; set; }
        public bool CardBoardTypeActive { get; set; }
        public bool ClosureTypeActive { get; set; }
        public bool VersionActive { get; set; }
        public string VersionInfo { get; set; }
        public bool CodigoBarraActive { get; set; }
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
        public List<ProjectMeasureItemResponse> MeasureItems { get; set; } = new();
        public List<ProjectImageResponse> Images { get; set; } = new();
        public List<ImagesGalleryResponse> ImagesGallery { get; set; } = new();
    }
}
