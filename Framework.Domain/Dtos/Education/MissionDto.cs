using System;
using System.Collections.Generic;

namespace Framework.Domain.Dtos.Education
{
    public class MissionDto
    {
        public int StudyDayId { get; set; }
        public DateTime Date { get; set; }
        public bool IsCompleted { get; set; }
        public decimal  CompletionPercentege { get; set; }
        public string StartDate { get; set; }
        public string ConclusionDate { get; set; }
        public int AdvancedMissionsQtd { get; set; } = 0;
        public int TotalDuration { get; set; }
        public int TotalCompletedDuration { get; set; }
        public int TotalLessonsWatched { get; set; }
        public string Status { get; set; } // Novo campo
        public string Resumo { get; set; }
        public string QtdQuestions { get; set; }
        public string QtdQuestionsOk { get; set; }
        public string QtdQuestionsNOk { get; set; }
        public string DayTitle { get; set; }
        public List<ActivityDto> Activities { get; set; }
    }

}

