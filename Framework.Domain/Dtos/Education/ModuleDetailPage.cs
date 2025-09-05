using System.Collections.Generic;

namespace Framework.Domain.Dtos.Education
{
    public class ModuleDetailPage
    {
        public ModuleItemShort Page { get; set; }
        public List<LessonItemShort> Lessons { get; set; }
    }
}
