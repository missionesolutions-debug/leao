using Data.Models.Core;
using System.Collections.Generic;

namespace Framework.Domain.Dtos.Education
{
    public class LessonItemShort
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? Order { get; set; }
        public int? Duration { get; set; }
        public int TotalMinutes { get; set; }
        public string Thumbnail { get; set; }
        public string Url { get; set; }   
        public string VideoContent { get; set; }
        public string Description { get; set; } 
        public bool IsFeatured { get; set; }

        public List<LessonFile> lessonFiles { get; set; } = new List<LessonFile>();
    }

    public class LessonFile {
    
        public string FileName { get; set; }
        public string FileSource { get; set; }
        public string FileImage { get; set; }
        public string FileDescription { get; set; }

    }

}
