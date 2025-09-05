using System.Collections.Generic;

namespace Framework.Domain.Dtos.Education
{
    public class ModuleItemShort
    {
        public string Url { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Thumbnail { get; set; }
        public int? Order { get; set; }
        public string Duration { get; set; }
        public string Type { get; set; }
        public int TotalMinutes { get; set; }
        public int? NumberOfLessons { get; set; }
        public List<LessonItemShort> Lessons { get; set; }
    }

}
