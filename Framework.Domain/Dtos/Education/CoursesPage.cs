using Framework.Domain.Site.Component;
using System.Collections.Generic;

namespace Framework.Domain.Dtos.Education
{
    public class CoursesPage
    {
        public Item Page { get; set; }
        public List<Item> Banners { get; set; } = new List<Item>();
        public List<Categories> Categories { get; set; } = new List<Categories>();
    }

    public class Categories
    {
        public Categories()
        {

        }

        public string Title { get; set; }
        public string Duration { get; set; }
        public int TotalMinutes { get; set; }
        public int? NumberOfLessons { get; set; }
        public List<CourseItemShort> Courses { get; set; } = new List<CourseItemShort>();
    }
}
