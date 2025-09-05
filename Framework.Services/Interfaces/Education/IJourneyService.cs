using Framework.Data.Models.Education.Journeys;
using Framework.Data.Models.Education;
using System.Collections.Generic;
using Framework.Domain.Dtos.Education;

namespace Framework.Services.Interfaces.Education
{
    public interface IJourneyService
    {
        public UserMissionsDto GetUserMissions(string emailAddress);
        /// <summary>
        /// Gera uma nova jornada para o usuário com base nas preferências fornecidas.
        /// </summary>
        void GenerateJourney(string emailAddress, int courseId, Dictionary<DayOfWeek, int> studyHoursPerDay, bool resetBasicModules, bool resetAdvancedModules);

        /// <summary>
        /// Marca uma atividade específica como concluída.
        /// </summary>
        ActivityCompleteDto MarkActivityAsCompleted(int activityId);

        /// <summary>
        /// Confirma a conclusão da missão atual, verificando se todas as atividades foram completadas.
        /// </summary>
        bool ConfirmCurrentMissionCompletion(int studyDayId);

        /// <summary>
        /// Atualiza o progresso da jornada com base nas lições concluídas.
        /// </summary>
        void UpdateJourneyCompletion(int journeyId);

        /// <summary>
        /// Recupera o ID de um tipo de atividade com base no nome.
        /// </summary>
        int GetActivityTypeId(string activityTypeName);

        /// <summary>
        /// Obtém as lições ordenadas para os módulos fornecidos.
        /// </summary>
        List<(Lesson Lesson, Module Module)> GetOrderedLessons(List<CourseModule> modules);

        /// <summary>
        /// Obtém as lições restantes para os módulos, excluindo as lições concluídas.
        /// </summary>
        List<(Lesson Lesson, Module Module)> GetRemainingLessons(List<CourseModule> modules, int userId);
        void MarkActivityAsStarted(int activityId);
        StudyActivityDto GetStudyActivity(int activityId);

        void UpdateStudyDayInfo(int studyDayId, string resumo, string qtdQuestions, string qtdQuestionsOk, string qtdQuestionsNOk);
        void UpdateNotebook(int activityId, string resume);
        NotebookContentDto DetailNotebook(int activityId);

        StudyDayViewDto GetStudyDayDetails(int studyDayId);
    }


}
