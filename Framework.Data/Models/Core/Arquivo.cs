namespace Data.Models.Core
{
    public class Arquivo : Base
    {
		public string FileName { get; set; }
		public string FileType { get; set; }
		public string FileSize { get; set; }
		public string FileData { get; set; }
		public string FileTags { get; set; }
		public string FileImage { get; set; }
		public string Guid { get; set; }
		public string Slug { get; set; }
		public string PlaceReceived { get; set; }

		public int TableId { get; set; }
		public string TableAction { get; set; }
		public string Description { get; set; }
	}
}
