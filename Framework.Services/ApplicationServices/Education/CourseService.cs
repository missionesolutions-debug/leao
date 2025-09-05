using Data;
using Data.Models.Core;
using Data.Models.Ecommerce;
using Framework.Data.Models.Education;
using Framework.Domain.Dtos.Education;
using Framework.Domain.Site.Component;
using Framework.Repositories.Factories.Education;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Services.ApplicationServices.Education
{
    public class CourseService
    {
        private readonly ApplicationDbContext _context;
        private readonly CourseRepository _courseRepository;

        public CourseService(ApplicationDbContext context, CourseRepository courseRepository)
        {
            _context = context;
            _courseRepository = courseRepository;
        }
        #region Portal
        public async Task<Course> SaveOrUpdate(Course obj)
        {
            try
            {
                if (obj.Id == 0)
                {
                    _courseRepository.SaveObj(obj);
                }
                else
                {
                    _courseRepository.UpdateObj(obj);
                }
            }
            catch (EntityException)
            {

            }

            return obj;
        }
        #endregion

        #region Application
        public CoursesPage GetCoursesPage(bool isAuthenticated)
        {
            var coursesPage = new CoursesPage();

            coursesPage.Page = GetPage();

            // Obter Banners
            coursesPage.Banners = GetBanners();

            List<CourseModule> courseModules = _context.CourseModules.ToList();
            List<Module> modules = _context.Modules.Where(b => b.IsActive == true).ToList();
            List<ModuleLesson> moduleLessons = _context.ModuleLessons.ToList();
            List<Lesson> lessons = _context.Lessons.Where(b => b.IsActive == true).ToList();

            var courseCategories = _context.CourseCategory
    .Include(cc => cc.Courses)
        .ThenInclude(course => course.CourseModules)
            .ThenInclude(courseModule => courseModule.Module)
                .ThenInclude(module => module.ModuleLessons)
                    .ThenInclude(moduleLesson => moduleLesson.Lesson)
    .Where(cc => cc.IsActive && !cc.IsDeleted)
    .ToList();

            foreach (var courseCategory in courseCategories)
            {
                var courses = courseCategory.Courses
                    .Where(course => course.IsActive && !course.IsDeleted)
                    .Select(course =>
                    {
                        var modules = course.CourseModules.Where(b=> b.DestaqueVitrine == true)
                            .Select(cm => cm.Module)
                            .Where(module => module.IsActive && !module.IsDeleted)
                            .OrderBy(module => module.Ordem)
                            .Select(module =>
                            {
                                var lessons = module.ModuleLessons
                                    .Where(ml => ml.Lesson.IsActive && !ml.Lesson.IsDeleted)
                                    .OrderBy(ml => ml.Order)
                                    .Select(ml => new LessonItemShort
                                    {
                                        Id = ml.Lesson.Id,
                                        Title = ml.Lesson.Title,
                                        Duration = ml.Lesson.Duration, // Assumindo em minutos
                                        Order = ml.Lesson.Ordem,
                                        Description = ml.Lesson.Description,
                                        IsFeatured = ml.Lesson.IsFeatured ?? false,
                                        Thumbnail = ml.Lesson.Thumbnail,
                                        Url = isAuthenticated ? ml.Lesson.Guid.ToString() : null,
                                        VideoContent = isAuthenticated ? ml.Lesson.VideoContent : null
                                    })
                                    .ToList();

                                var totalLessonDuration = lessons.Sum(lesson => lesson.Duration ?? 0);
                                var moduleDurationFormatted = FormatDuration(totalLessonDuration);
                                var numberOfLessons = lessons.Count();

                                return new ModuleItemShort
                                {
                                    Id = module.Id,
                                    Title = module.Title,
                                    Duration = moduleDurationFormatted,
                                    TotalMinutes = totalLessonDuration,
                                    Thumbnail = module.Thumbnail,
                                    Type = "Module",
                                    Order = module.Ordem,
                                    Url = module.Guid.ToString(),
                                    NumberOfLessons = numberOfLessons,
                                    Lessons = lessons.OrderBy(b=> b.Order).ToList()
                                };
                            })
                            .ToList();

                        var totalModuleDuration = modules.Sum(m => m.TotalMinutes);
                        var courseDurationFormatted = FormatDuration(totalModuleDuration);
                        var numberOfLessons = modules.Sum(m => m.NumberOfLessons ?? 0);

                        return new CourseItemShort
                        {
                            Id = course.Id,
                            Title = course.Title,
                            CoverImageUrl = course.CoverImageUrl,
                            Duration = courseDurationFormatted,
                            TotalMinutes = totalModuleDuration,
                            AverageRating = course.AverageRating,
                            Url = course.Guid.ToString(),
                            NumberOfLessons = numberOfLessons,
                            Modules = modules.OrderBy(b => b.Order).ToList()
                        };
                    })
                    .ToList();

                var totalCourseDuration = courses.Sum(c => c.TotalMinutes);
                var categoryDurationFormatted = FormatDuration(totalCourseDuration);
                var categoryNumberOfLessons = courses.Sum(c => c.NumberOfLessons ?? 0);

                var categoryDto = new Categories
                {
                    Title = courseCategory.Title,
                    Duration = categoryDurationFormatted,
                    TotalMinutes = totalCourseDuration,
                    NumberOfLessons = categoryNumberOfLessons,
                    Courses = courses
                };

                coursesPage.Categories.Add(categoryDto);
            }

            return coursesPage;


            // Obter Categorias de Cursos e Cursos
            //    var courseCategories = _context.CourseCategory
            //        .Include(cc => cc.Courses)
            //            .ThenInclude(course => course.Modules)
            //                .ThenInclude(module => module.ModuleLessons)
            //                    .ThenInclude(moduleLesson => moduleLesson.Lesson)
            //        .Where(cc => cc.IsActive && !cc.IsDeleted)
            //        .ToList();

            //    foreach (var courseCategory in courseCategories)
            //    {
            //        var courses = courseCategory.Courses
            //            .Where(course => course.IsActive && !course.IsDeleted)
            //            .Select(course =>
            //            {
            //                var modules = course.Modules
            //                    .Where(module => module.IsActive && !module.IsDeleted).OrderBy(b=> b.Ordem)
            //                    .Select(module =>
            //                    {
            //                        var lessons = module.ModuleLessons
            //                            .Where(ml => ml.Lesson.IsActive && !ml.Lesson.IsDeleted)
            //                            .OrderBy(ml => ml.Order)
            //                            .Select(ml => new LessonItemShort
            //                            {
            //                                Id = ml.Lesson.Id,
            //                                Title = ml.Lesson.Title,
            //                                Duration = ml.Lesson.Duration, // Assumindo em minutos
            //                                Order = ml.Lesson.Ordem,
            //                                Description = ml.Lesson.Description,
            //                                IsFeatured = ml.Lesson.IsFeatured ?? false,
            //                                Thumbnail = ml.Lesson.Thumbnail,
            //                                Url = isAuthenticated ? ml.Lesson.Guid.ToString() : null,
            //                                VideoContent = isAuthenticated ? ml.Lesson.VideoContent : null
            //                            })
            //                            .ToList();

            //                        lessons = lessons.OrderBy(b => b.Order).ToList();

            //                        var totalLessonDuration = lessons.Sum(lesson => lesson.Duration ?? 0);
            //                        var moduleDurationFormatted = FormatDuration(totalLessonDuration);
            //                        var numberOfLessons = lessons.Count();

            //                        return new ModuleItemShort
            //                        {
            //                            Id = module.Id,
            //                            Title = module.Title,
            //                            Duration = moduleDurationFormatted,
            //                            TotalMinutes = totalLessonDuration,
            //                            Thumbnail = module.Thumbnail,
            //                            Type = "Module",
            //                            Order = module.Order,
            //                            NumberOfLessons = numberOfLessons,
            //                            Lessons = lessons
            //                        };
            //                    })
            //                    .ToList();

            //                var totalModuleDuration = modules.Sum(m => m.TotalMinutes);
            //                var courseDurationFormatted = FormatDuration(totalModuleDuration);
            //                var numberOfLessons = modules.Sum(m => m.NumberOfLessons ?? 0);

            //                return new CourseItemShort
            //                {
            //                    Id = course.Id,
            //                    Title = course.Title,
            //                    CoverImageUrl = course.CoverImageUrl,
            //                    Duration = courseDurationFormatted,
            //                    TotalMinutes = totalModuleDuration,
            //                    AverageRating = course.AverageRating,
            //                    Url = course.Guid.ToString(),
            //                    NumberOfLessons = numberOfLessons,
            //                    Modules = modules
            //                };
            //            })
            //            .ToList();

            //        var totalCourseDuration = courses.Sum(c => c.TotalMinutes);
            //        var categoryDurationFormatted = FormatDuration(totalCourseDuration);
            //        var categoryNumberOfLessons = courses.Sum(c => c.NumberOfLessons ?? 0);

            //        var categoryDto = new Categories
            //        {
            //            Title = courseCategory.Title,
            //            Duration = categoryDurationFormatted,
            //            TotalMinutes = totalCourseDuration,
            //            NumberOfLessons = categoryNumberOfLessons,
            //            Courses = courses
            //        };

            //        coursesPage.Categories.Add(categoryDto);
            //    }


            //    return coursesPage;
            //}
        }

            private string FormatDuration(int totalMinutes)
        {
            int hours = totalMinutes / 60;
            int minutes = totalMinutes % 60;
            if (hours > 0)
            {
                return minutes > 0 ? $"{hours}h{minutes}min" : $"{hours}h";
            }
            else
            {
                return $"{minutes}min";
            }
        }

        private Item GetPage()
        {
            var page = _context.Pagina
               .Where(c => c.Ativo == true && c.Excluido != true && c.Id == 1)
               .OrderByDescending(c => c.Ordem)
               .Select(c => new Item
               {
                   Imagem = c.Imagem,
                   Titulo = c.Titulo,
                   PageTitle = c.PageTitle,
                   MetaDescription = c.MetaDescription,
                   Descricao = c.Descricao,
                   Link = c.Url
               })
               .FirstOrDefault();

            return page;
        }

        private List<Item> GetBanners()
        {

            var banners = _context.Banner
                .Where(c => c.Ativo == true && c.Excluido != true)
                .OrderByDescending(c => c.Ordem)
                .Select(c => new Item
                {
                    Imagem = c.Imagem,
                    Titulo = c.Titulo,
                    Descricao = c.Descricao,
                    Link = c.Url,
                    Chave = c.Chave
                })
                .ToList();

            return banners;
        }


        public CourseDetailPage GetCoursePage(string guid, bool isAuthenticated)
        {
            CourseDetailPage courseDetailPage = new CourseDetailPage();

            // Incluindo CourseModule na relação
            var courseCategories = _context.CourseCategory
                .Include(cc => cc.Courses)
                    .ThenInclude(course => course.CourseModules)
                        .ThenInclude(courseModule => courseModule.Module)
                            .ThenInclude(module => module.ModuleLessons)
                                .ThenInclude(moduleLesson => moduleLesson.Lesson)
                .Where(cc => cc.IsActive && !cc.IsDeleted && cc.Courses.Any(course => course.Guid.ToString() == guid))
                .ToList();

            foreach (var courseCategory in courseCategories)
            {
                courseDetailPage.Page = courseCategory.Courses
                    .Where(course => course.IsActive && !course.IsDeleted && course.Guid.ToString() == guid)
                    .Select(course =>
                    {
                        var modules = course.CourseModules
                            .Select(cm => cm.Module)
                            .Where(module => module.IsActive && !module.IsDeleted)
                            .OrderBy(module => module.Ordem)
                            .Select(module =>
                            {
                                var lessons = module.ModuleLessons
                                    .Where(ml => ml.Lesson.IsActive && !ml.Lesson.IsDeleted)
                                    .OrderBy(ml => ml.Order)
                                    .Select(ml => new LessonItemShort
                                    {
                                        Id = ml.Lesson.Id,
                                        Title = ml.Lesson.Title,
                                        Duration = ml.Lesson.Duration, // Certifique-se de que é int? e em minutos
                                        Order = ml.Lesson.Ordem,
                                        Description = ml.Lesson.Description,
                                        IsFeatured = ml.Lesson.IsFeatured ?? false,
                                        Thumbnail = ml.Lesson.Thumbnail,
                                        Url = isAuthenticated ? ml.Lesson.Guid.ToString() : null,
                                        VideoContent = isAuthenticated ? ml.Lesson.VideoContent : null
                                    })
                                    .ToList();

                                var totalLessonDuration = lessons.Sum(lesson => lesson.Duration ?? 0);
                                var moduleDurationFormatted = FormatDuration(totalLessonDuration);
                                var numberOfLessons = lessons.Count();

                                return new ModuleItemShort
                                {
                                    Id = module.Id,
                                    Title = module.Title,
                                    Duration = moduleDurationFormatted,
                                    TotalMinutes = totalLessonDuration,
                                    Thumbnail = module.Thumbnail,
                                    Order = module.Ordem,
                                    NumberOfLessons = numberOfLessons,
                                    Lessons = lessons.OrderBy(b=> b.Order).ToList()
                                };
                            })
                            .ToList();

                        var totalModuleDuration = modules.Sum(m => m.TotalMinutes);
                        var courseDurationFormatted = FormatDuration(totalModuleDuration);
                        var numberOfLessons = modules.Sum(m => m.NumberOfLessons);

                        return new CourseItemShort
                        {
                            Id = course.Id,
                            Title = course.Title,
                            CoverImageUrl = course.CoverImageUrl,
                            Duration = courseDurationFormatted,
                            TotalMinutes = totalModuleDuration,
                            AverageRating = course.AverageRating,
                            CoverThumbUrl = course.Imagem,
                            Url = course.Guid.ToString(),
                            NumberOfLessons = numberOfLessons,
                            Modules = modules
                        };
                    })
                    .FirstOrDefault();
            }

            return courseDetailPage;
        }

        //public CourseDetailPage GetCoursePage(string guid, bool isAuthenticated)
        //{
        //    CourseDetailPage courseDetailPage = new CourseDetailPage();

        //    var courseCategories = _context.CourseCategory
        //        .Include(cc => cc.Courses)
        //            .ThenInclude(course => course.Modules)
        //                .ThenInclude(module => module.ModuleLessons)
        //                    .ThenInclude(moduleLesson => moduleLesson.Lesson)
        //        .Where(cc => cc.IsActive && !cc.IsDeleted && cc.Courses.Where(dd => dd.Guid.ToString() == guid).Any())
        //        .ToList();

        //    foreach (var courseCategory in courseCategories)
        //    {
        //        courseDetailPage.Page = courseCategory.Courses
        //            .Where(course => course.IsActive && !course.IsDeleted && course.Guid.ToString() == guid)
        //            .Select(course =>
        //            {
        //                var modules = course.Modules
        //                    .Where(module => module.IsActive && !module.IsDeleted)
        //                    .Select(module =>
        //                    {
        //                        var lessons = module.ModuleLessons
        //                            .Where(ml => ml.Lesson.IsActive && !ml.Lesson.IsDeleted)
        //                            .OrderBy(ml => ml.Order)
        //                            .Select(ml => new LessonItemShort
        //                            {
        //                                Id = ml.Lesson.Id,
        //                                Title = ml.Lesson.Title,
        //                                Duration = ml.Lesson.Duration, // Certifique-se de que é int? e em minutos
        //                                Order = ml.Order,
        //                                Description = ml.Lesson.Description,
        //                                IsFeatured = ml.Lesson.IsFeatured ?? false,
        //                                Thumbnail = ml.Lesson.Thumbnail,
        //                                Url = isAuthenticated ? ml.Lesson.Guid.ToString() : null,
        //                                VideoContent = isAuthenticated ? ml.Lesson.VideoContent : null
        //                            })
        //                            .ToList();

        //                        var totalLessonDuration = lessons.Sum(lesson => lesson.Duration ?? 0);
        //                        var moduleDurationFormatted = FormatDuration(totalLessonDuration);
        //                        var numberOfLessons = lessons.Count();

        //                        return new ModuleItemShort
        //                        {
        //                            Id = module.Id,
        //                            Title = module.Title,
        //                            Duration = moduleDurationFormatted,
        //                            TotalMinutes = totalLessonDuration,
        //                            Order = module.Order,
        //                            NumberOfLessons = numberOfLessons,
        //                            Lessons = lessons
        //                        };
        //                    })
        //                    .ToList();

        //                var totalModuleDuration = modules.Sum(m => m.TotalMinutes);
        //                var courseDurationFormatted = FormatDuration(totalModuleDuration);
        //                var numberOfLessons = modules.Sum(m => m.NumberOfLessons);

        //                return new CourseItemShort
        //                {
        //                    Id = course.Id,
        //                    Title = course.Title,
        //                    CoverImageUrl = course.CoverImageUrl,
        //                    Duration = courseDurationFormatted,
        //                    TotalMinutes = totalModuleDuration,
        //                    AverageRating = course.AverageRating,
        //                    Url = course.Guid.ToString(),
        //                    NumberOfLessons = numberOfLessons,
        //                    Modules = modules
        //                };
        //            })
        //            .FirstOrDefault();
        //    }

        //    return courseDetailPage;
        //}

        public LessonDetailPage GetLessonPage(string guid, bool isAuthenticated)
        {
            LessonDetailPage lessonDetailPage = new LessonDetailPage();

            // Obter a lição e preencher o LessonItemShort com os dados básicos
            lessonDetailPage.Page = _context.Lessons
                .Where(b => b.Guid.ToString() == guid)
                .Select(b => new LessonItemShort
                {
                    Id = b.Id,
                    Title = b.Title,
                    Order = b.Order,
                    Description = b.Description,
                    IsFeatured = b.IsFeatured ?? false,
                    Thumbnail = b.Thumbnail,
                    Url = isAuthenticated ? b.Guid.ToString() : null,
                    VideoContent = isAuthenticated ? b.VideoContent : null
                })
                .FirstOrDefault();

            if (lessonDetailPage.Page != null)
            {
                // Obter arquivos associados à lição
                List<Arquivo> arquivos = _context.Arquivo
                    .Where(a => a.TableAction == "Lessons" && a.TableId == lessonDetailPage.Page.Id)
                    .ToList();

                // Mapear os arquivos para o tipo LessonFile e adicionar à lista lessonFiles
                lessonDetailPage.Page.lessonFiles = arquivos.Select(a => new LessonFile
                {
                    FileName = a.FileName,
                    FileSource = a.FileData,
                    FileImage = a.FileImage,
                    FileDescription = a.Description
                }).ToList();
            }

            return lessonDetailPage;
        }


        public CourseItemShortless GetCoursesShort()
        {
            return new CourseItemShortless()
            {
                courseItems = _context.Courses
                    .Where(b => b.IsActive == true)
                    .Select(b => new CourseItemMin
                    {
                        Title = b.Title,
                        Id = b.Id.ToString()
                    })
                    .ToList()
            };
        }

        public ModuleDetailPage GetModulePage(string guid, bool isAuthenticated)
        {
            ModuleDetailPage moduleDetailPage = new ModuleDetailPage();

            // Obter a lição e preencher o LessonItemShort com os dados básicos
            moduleDetailPage.Page = _context.Modules
                .Where(b => b.Guid.ToString() == guid).Include(b => b.ModuleLessons)
                .Select(b => new ModuleItemShort
                {
                    Id = b.Id,
                    Title = b.Title,
                    Thumbnail = b.Thumbnail,
                    Type = "Module",
                    Order = b.Order,
                })
                .FirstOrDefault();

            var lessons = _context.ModuleLessons
                                    .Where(ml => ml.Lesson.IsActive && !ml.Lesson.IsDeleted && ml.ModuleId == moduleDetailPage.Page.Id)
                                    .Select(ml => new LessonItemShort
                                    {
                                        Id = ml.Lesson.Id,
                                        Title = ml.Lesson.Title,
                                        Duration = ml.Lesson.Duration, // Assumindo em minutos
                                        Order = ml.Lesson.Ordem,
                                        Description = ml.Lesson.Description,
                                        IsFeatured = ml.Lesson.IsFeatured ?? false,
                                        Thumbnail = ml.Lesson.Thumbnail,
                                        Url = isAuthenticated ? ml.Lesson.Guid.ToString() : null,
                                        VideoContent = isAuthenticated ? ml.Lesson.VideoContent : null
                                    })
                                    .ToList();

            lessons = lessons.OrderBy(b => b.Order).ToList();

            var totalLessonDuration = lessons.Sum(lesson => lesson.Duration ?? 0);
            var moduleDurationFormatted = FormatDuration(totalLessonDuration);
            var numberOfLessons = lessons.Count();

            moduleDetailPage.Page.NumberOfLessons = numberOfLessons;
                    moduleDetailPage.Page.Lessons = lessons;
            moduleDetailPage.Page.Duration = moduleDurationFormatted;
                    moduleDetailPage.Page.TotalMinutes = totalLessonDuration;

            return moduleDetailPage;
        }

        #endregion
    }
}
