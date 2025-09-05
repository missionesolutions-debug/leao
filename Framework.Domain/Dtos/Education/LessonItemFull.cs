using System;

namespace Framework.Domain.Dtos.Education
{
    public class LessonItemFull
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Title { get; set; }
        public string Thumbnail { get; set; }

        public string ContentUrl { get; set; }
        public int? Duration { get; set; }
        public int? Order { get; set; }
    }

}
