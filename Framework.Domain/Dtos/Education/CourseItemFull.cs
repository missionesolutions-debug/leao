using System;
using System.Collections.Generic;

namespace Framework.Domain.Dtos.Education
{
    public class CourseItemFull
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CoverImageUrl { get; set; }
        public string Url { get; set; }
        public int? Duration { get; set; }
        public int? NumberOfModules { get; set; }
        public int? NumberOfLessons { get; set; }
        public int? NumberOfReviews { get; set; }
        public decimal? AverageRating { get; set; }
        public List<ModuleItemFull> Modules { get; set; } = new List<ModuleItemFull>();
    }

}
