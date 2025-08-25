using Newtonsoft.Json;
using System.Collections.Generic;

namespace Panel.Services.Streaming.PandaVideo.Domain
{
    public class PandaVideosResponse
    {
        public List<Video> Videos { get; set; }
        public int Pages { get; set; }
        public int Total { get; set; }
    }

    public class Video
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public Guid UserId { get; set; }
        public Guid FolderId { get; set; }
        public Guid LibraryId { get; set; }
        public Guid? LiveId { get; set; }
        public Guid VideoExternalId { get; set; }
        public DateTime? ConvertedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long StorageSize { get; set; }
        public int Length { get; set; }
        [JsonProperty("video_player")]
        public string VideoPlayer { get; set; }
        [JsonProperty("video_hls")]
        public string VideoHls { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Playable { get; set; }
        public bool Backup { get; set; }
        public string Preview { get; set; }
        public string Thumbnail { get; set; }
        public List<string> Playback { get; set; }
    }
}
