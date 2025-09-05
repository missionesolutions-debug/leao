using Microsoft.AspNetCore.Mvc;

namespace Api.Models.Factory
{
    public class InsertProjectImageRequest
	{
        [FromForm]
        public int ProjectImageId { get; set; } = 0;

		[FromForm]
        public int ProjectId { get; set; }

        [FromForm]
        public int ProjectBlockId { get; set; }

        [FromForm]
        public IFormFile? File { get; set; }

        [FromForm]
        public string? Description { get; set; }

        [FromForm]
        public bool? EnableOnReport { get; set; }
    }

}
