using System;
using System.Collections.Generic;

namespace Framework.Domain.Dtos.Education
{
    public class ActivityDto
    {
        public int ActivityId { get; set; }
        public string ActivityType { get; set; }
        public int? LessonId { get; set; }
        public Guid? LessonGuid { get; set; } // Novo campo
        public string LessonTitle { get; set; } // Novo campo
        public string LessonVideoContent { get; set; } // Novo campo
        public DateTime? StartDate { get; set; } // Novo campo
        public DateTime? EndDate { get; set; } // Novo campo
        public int Duration { get; set; }
        public TimeSpan ExecutionTime { get; set; } // Novo campo
        public string Status { get; set; } // Novo campo
        public bool IsCompleted { get; set; }
        public bool IsStarted { get; set; } = false;
        public string Resume { get; set; }
        public LessonDetailPage DetailPage { get; set; } = new LessonDetailPage();
    }
}
