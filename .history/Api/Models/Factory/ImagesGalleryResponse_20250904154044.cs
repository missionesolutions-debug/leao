namespace Api.Models.Factory
{
	public class ImagesGalleryResponse
	{
		public int Id { get; set; }
		public required string UrlSource { get; set; }
		public required string Title { get; set; }
		public required string Description { get; set; }
	}
}
