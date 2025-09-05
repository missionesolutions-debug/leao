using System.Collections.Generic;
using System;

namespace Framework.Domain.Dtos.Education
{
    public class ModuleItemFull
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CoverImageUrl { get; set; }
        public int? Duration { get; set; }
        public int? NumberOfLessons { get; set; }
        public int? Order { get; set; }
        public List<LessonItemFull> Lessons { get; set; } = new List<LessonItemFull>();
    }

}
