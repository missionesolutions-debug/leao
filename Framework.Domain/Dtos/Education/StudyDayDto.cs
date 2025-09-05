using System;
using System.Collections.Generic;

namespace Framework.Domain.Dtos.Education
{
    public class StudyDayDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public bool IsCompleted { get; set; }
        public List<StudyActivityDto> StudyActivities { get; set; }
    }

    public class StudyActivityDto
    {
        public int Id { get; set; }
        public string ActivityType { get; set; }
        public int? LessonId { get; set; }
        public string LessonName { get; set; }
        public bool IsCompleted { get; set; }
    }

}
