using System.Collections.Generic;

namespace Panel.Services.Streaming.PandaVideo.Domain
{
    public class PandaFolderResponse
    {
        public List<PandaFolder> Folders { get; set; }
    }

    public class PandaFolder
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public string ParentFolderId { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int VideosCount { get; set; }
    }
}
