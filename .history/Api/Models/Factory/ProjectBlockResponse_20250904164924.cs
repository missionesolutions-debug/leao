namespace Api.Models.Factory
{
    public class ProjectBlockResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int MinImagesAmount { get; set; }
        public int MaxImagesAmount { get; set; }
        public bool ObservationsEnabled { get; set; }
        public string Instructions { get; set; } = string.Empty;
        public int? Position { get; set; }
        public string Code { get; set; } = string.Empty;
        public bool BoxTypeActive { get; set; }
        public bool CardBoardTypeActive { get; set; }
        public bool ClosureTypeActive { get; set; }
        public bool VersionActive { get; set; }
        public string VersionInfo { get; set; } = string.Empty;
        public bool CodigoBarraActive { get; set; }
        public string CodigoBarraInfo { get; set; } = string.Empty;
        public bool BlockForConclude { get; set; }
        public string ActionText { get; set; } = string.Empty;
        public string ImagesLabel { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public string Observation { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string BoxType { get; set; } = string.Empty;
        public string CardBoardType { get; set; } = string.Empty;
        public string ClosureType { get; set; } = string.Empty;

        // Novos campos para incluir os itens de medida e as imagens
        public List<ProjectMeasureItemResponse> MeasureItems { get; set; } = new();
        public List<ProjectImageResponse> Images { get; set; } = new();
        public List<ImagesGalleryResponse> ImagesGallery { get; set; } = new();
    }
}
