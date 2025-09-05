using System.Collections.Generic;

namespace Framework.Domain.Dtos.Education
{
    public class CourseItemShortless
    {
        public List<CourseItemMin> courseItems { get; set; }
    }

    public class CourseItemMin
    {
        public string Title { get; set; }
        public string Id { get; set; }
    }
}
