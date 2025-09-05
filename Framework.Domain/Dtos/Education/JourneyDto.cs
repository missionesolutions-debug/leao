using System;
using System.Collections.Generic;

namespace Framework.Domain.Dtos.Education
{
    public class JourneyDto
    {
        public int UsuarioId { get; set; }
        public int CourseId { get; set; }
        public List<StudyHoursDto> StudyHoursPerDay { get; set; }
        // Other properties
    }

    public class StudyHoursDto
    {
        public DayOfWeek DayOfWeek { get; set; }
        public int StudyHours { get; set; }
    }

}
