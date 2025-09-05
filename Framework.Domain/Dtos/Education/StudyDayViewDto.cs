using System;
using System.Collections.Generic;

namespace Framework.Domain.Dtos.Education
{
    public class StudyDayViewDto
    {
        public int StudyDayId { get; set; }
        public DateTime Date { get; set; }
        public bool IsCompleted { get; set; }
        public string Resumo { get; set; }
        public string QtdQuestions { get; set; }
        public string QtdQuestionsOk { get; set; }
        public string QtdQuestionsNOk { get; set; }
        public List<ActivityDto> Activities { get; set; } = new List<ActivityDto>();
    }

}
