using System.Collections.Generic;

namespace Framework.Domain.Dtos.Education
{
    public class CourseItemShort
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CoverThumbUrl { get; set; }
        public string CoverImageUrl { get; set; }
        public string Duration { get; set; }
        public int TotalMinutes { get; set; }
        public int? NumberOfLessons { get; set; }
        public decimal? AverageRating { get; set; }
        public string Url { get; set; }
        public List<ModuleItemShort> Modules { get; set; } = new List<ModuleItemShort>();
        public List<LessonItemShort> Lessons { get; set; } = new List<LessonItemShort>();
    }

}
