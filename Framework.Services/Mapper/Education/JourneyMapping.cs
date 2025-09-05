using Framework.Data.Models.Education.Journeys;
using Framework.Data.Models.Education;
using Framework.Domain.Dtos.Education;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Services.Mapper.Education
{
    public class JourneyMapping
    {
        public JourneyDto MapToJourneyDto(Journey journey)
        {
            return new JourneyDto
            {
                UsuarioId = journey.UsuarioId,
                CourseId = journey.CourseId,
                StudyHoursPerDay = journey.JourneyStudyHours.Select(jsh => new StudyHoursDto
                {
                    DayOfWeek = jsh.DayOfWeek,
                    StudyHours = jsh.StudyHours
                }).ToList(),
                // Map other properties
            };
        }

        public StudyDayDto MapToStudyDayDto(StudyDay studyDay)
        {
            return new StudyDayDto
            {
                Id = studyDay.Id,
                Date = studyDay.Date,
                IsCompleted = studyDay.IsCompleted,
                StudyActivities = studyDay.StudyActivities.Select(sa => new StudyActivityDto
                {
                    Id = sa.Id,
                    ActivityType = sa.ActivityType.Name,
                    LessonId = sa.LessonId,
                    LessonName = sa.Lesson?.Title,
                    IsCompleted = sa.IsCompleted
                }).ToList()
            };
        }

    }
}
