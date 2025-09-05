using Framework.Data.Models.Education.Journeys;
using Framework.Data.Models.Education;
using Framework.Repositories.Interface.Education;
using Framework.Services.Interfaces.Education;
using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Repositories.Factories.Education;
using Framework.Factories.Pessoa;
using Data.Models.Pessoa;
using Framework.Domain.Dtos.Education;
using Framework.Domain.Site.Component;
using Microsoft.VisualBasic;
using Data.Models.Core;
using Framework.Factories.Core;

namespace Framework.Services.ApplicationServices.Education
{
	public class JourneyService : IJourneyService
	{
		private const double DailyActivitiesPercentage = 0.3;

		private readonly CourseRepository _courseRepository;
		private readonly CourseModuleRepository _courseModuleRepository;
		private readonly ModuleLessonRepository _moduleLessonRepository;
		private readonly JourneyRepository _journeyRepository;
		private readonly StudyDayRepository _studyDayRepository;
		private readonly StudyActivityRepository _studyActivityRepository;
		private readonly ActivityTypeRepository _activityTypeRepository;
		private readonly UsuarioFactory _usuarioFactory;
		private readonly ArquivoFactory _arquivoFactory;
		private readonly LessonRepository _lessonRepository;
		private readonly NotebookRepository _notebookRepository;
		private readonly ModuleRepository _moduleRepository;

		public JourneyService(
			CourseRepository courseRepository,
			CourseModuleRepository courseModuleRepository,
			ModuleLessonRepository moduleLessonRepository,
			JourneyRepository journeyRepository,
			StudyDayRepository studyDayRepository,
			StudyActivityRepository studyActivityRepository,
			ActivityTypeRepository activityTypeRepository,
			UsuarioFactory usuarioFactory,
			ArquivoFactory arquivoFactory,
			LessonRepository lessonRepository,
			NotebookRepository notebookRepository,
			ModuleRepository moduleRepository)
		{
			_courseRepository = courseRepository;
			_courseModuleRepository = courseModuleRepository;
			_moduleLessonRepository = moduleLessonRepository;
			_journeyRepository = journeyRepository;
			_studyDayRepository = studyDayRepository;
			_studyActivityRepository = studyActivityRepository;
			_activityTypeRepository = activityTypeRepository;
			_usuarioFactory = usuarioFactory;
			_arquivoFactory = arquivoFactory;
			_lessonRepository = lessonRepository;
			_notebookRepository = notebookRepository;
			_moduleRepository = moduleRepository;


		}

		public DateTime GetBrasiliaTime()
		{
			// Obtém o fuso horário de Brasília
			TimeZoneInfo brasiliaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");

			// Converte a hora UTC para o horário de Brasília
			return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, brasiliaTimeZone);
		}

		public StudyDayViewDto GetStudyDayDetails(int studyDayId)
		{
			var studyDay = _studyDayRepository.GetObj(studyDayId);
			if (studyDay == null)
			{
				throw new Exception("StudyDay não encontrado.");
			}

			var studyActivities = _studyActivityRepository.GetByStudyDayId(studyDayId).ToList();

			StudyDayViewDto model = new StudyDayViewDto
			{
				StudyDayId = studyDay.Id,
				Date = studyDay.Date,
				IsCompleted = studyDay.IsCompleted,

				Resumo = studyDay.Resumo,
				QtdQuestions = studyDay.QtdQuestions,
				QtdQuestionsOk = studyDay.QtdQuestionsOk,
				QtdQuestionsNOk = studyDay.QtdQuestionsNOk,
				Activities = studyActivities.Select(sa => new ActivityDto
				{
					ActivityId = sa.Id,
					ActivityType = _activityTypeRepository.GetObj(sa.ActivityTypeId)?.Name,
					LessonId = sa.LessonId,
					IsCompleted = sa.IsCompleted,
					IsStarted = sa.IsStarted.Value,
					Duration = sa.Duration
				}).ToList()
			};


			foreach (var item in model.Activities.Where(b => b.ActivityType == "Estudo"))
			{
				try
				{
					var arquivos = _arquivoFactory.GetAll()
							 .Where(a => a.TableAction == "Lessons" && a.TableId == item.LessonId)
							 .ToList();
					List<Lesson> lessonObj = new List<Lesson>();

					lessonObj.Add(_lessonRepository.GetObj(item.LessonId.Value));

					item.DetailPage = GetLessonPage(lessonObj, arquivos);

					var notebook = _notebookRepository.GetByLessonAndStudyActivity(item.LessonId.Value, item.ActivityId);

					if (notebook is not null)
						item.Resume = notebook.Content;
				}
				catch (Exception ex)
				{

				}
			}

			return model;

		}


		public void UpdateStudyDayInfo(int studyDayId, string resumo, string qtdQuestions, string qtdQuestionsOk, string qtdQuestionsNOk)
		{
			var studyDay = _studyDayRepository.GetObj(studyDayId);
			if (studyDay == null)
			{
				throw new Exception("StudyDay não encontrado.");
			}

			// Atualizar os campos
			if (!resumo.IsNullOrEmpty())
				studyDay.Resumo = resumo;
			if (!qtdQuestions.IsNullOrEmpty())
				studyDay.QtdQuestions = qtdQuestions;
			if (!qtdQuestionsOk.IsNullOrEmpty())
				studyDay.QtdQuestionsOk = qtdQuestionsOk;
			if (!qtdQuestionsNOk.IsNullOrEmpty())
				studyDay.QtdQuestionsNOk = qtdQuestionsNOk;

			_studyDayRepository.UpdateObj(studyDay);
		}

		public UserMissionsDto GetUserMissions(string emailAddress)
		{
			List<Module> modules = _moduleRepository.GetAll().ToList();

			Usuario usuario = _usuarioFactory.GetByEmail(emailAddress);

			if (usuario is null)
				return new UserMissionsDto();

			int userId = usuario.Id;

			// Recupera a jornada atual do usuário
			var journey = _journeyRepository.GetByUserId(userId);
			if (journey == null)
			{
				throw new Exception($"Journey not found for user ID {userId}.");
			}

			// Data atual sem a parte de tempo
			DateTime today = DateTime.Today;

			// Lista para armazenar as missões (StudyDays)
			MissionDto currentMission = null;
			var overdueMissions = new List<MissionDto>();
			var completedMissions = new List<MissionDto>();
			var upcomingMissions = new List<MissionDto>();

			List<StudyDay> studydays = _studyDayRepository.GetByJourneyId(journey.Id).ToList();
			List<StudyActivity> studyActivities = new List<StudyActivity>();

			foreach (var item in studydays)
			{
				studyActivities.AddRange(_studyActivityRepository.GetByStudyDayId(item.Id).ToList());
			}

			// Ordena os StudyDays por data
			var studyDays = journey.StudyDays.OrderBy(sd => sd.Date).ToList();

			// Variável para controlar se precisamos atualizar o banco de dados
			bool databaseUpdated = false;

			foreach (var studyDay in studyDays)
			{
				// Determinar o status da missão
				string missionStatus;
				if (studyDay.IsCompleted)
				{
					missionStatus = "Concluído";
				}
				else if (studyDay.Date < today)
				{
					missionStatus = "Atrasado";
				}
				else if (studyDay.Date == today)
				{
					missionStatus = "Em andamento";
				}
				else
				{
					missionStatus = "Iniciar";
				}

				var missionDto = new MissionDto
				{
					StudyDayId = studyDay.Id,
					Date = studyDay.Date,
					IsCompleted = studyDay.IsCompleted,
					QtdQuestions = studyDay.QtdQuestions,
					QtdQuestionsNOk = studyDay.QtdQuestionsNOk,
					QtdQuestionsOk = studyDay.QtdQuestionsOk,
					Resumo = studyDay.Resumo,
					Status = missionStatus,
					Activities = new List<ActivityDto>(),
					DayTitle = ""
				};

				if (studyDay.StudyActivities is not null)
					foreach (var sa in studyDay.StudyActivities)
					{
						// Recuperar informações da lição, se houver
						Lesson lesson = null;
						if (sa.LessonId.HasValue)
						{
							lesson = _moduleLessonRepository.GetLessonById(sa.LessonId.Value);

							if (lesson != null && sa.ActivityTypeId == 2)
							{
								int moduleId = _moduleLessonRepository.GetModuleIdFromLesson(sa.LessonId.Value);

								if (moduleId > 0)
								{
									Module module = modules.Where(b => b.Id == moduleId).FirstOrDefault();

									if (module != null)
									{
										if (!missionDto.DayTitle.Contains(module.Title))
										{
											if (missionDto.DayTitle.Length > 0)
												missionDto.DayTitle = missionDto.DayTitle + " / " + module.Title;
											else
												missionDto.DayTitle = missionDto.DayTitle + module.Title;
										}
									}
								}
							}

						}







						// Determinar o status da atividade
						string activityStatus;
						if (sa.IsCompleted)
						{
							activityStatus = "Concluído";
						}
						else if (studyDay.Date < today)
						{
							activityStatus = "Atrasado";
						}
						else if (studyDay.Date == today)
						{
							activityStatus = "Em andamento";
						}
						else
						{
							activityStatus = "Iniciar";
						}

						// Tempo de execução (pode ser calculado ou armazenado em outro campo)
						TimeSpan executionTime = TimeSpan.Zero; // Substitua por cálculo real, se disponível

						List<Arquivo> arquivos = new List<Arquivo>();




						var activityDto = new ActivityDto
						{
							ActivityId = sa.Id,
							ActivityType = _activityTypeRepository.GetObj(sa.ActivityTypeId).Name,
							LessonId = sa.LessonId,
							LessonGuid = lesson?.Guid,
							LessonTitle = lesson?.Title,
							LessonVideoContent = lesson?.VideoContent,
							StartDate = sa.StartDate is not null ? sa.StartDate.Value : null,
							IsStarted = sa.IsStarted is not null && sa.IsStarted is true ? true : false,
							EndDate = sa.EndDate is not null ? sa.EndDate.Value : null,
							Duration = sa.Duration,
							ExecutionTime = executionTime,
							IsCompleted = sa.IsCompleted,
							Status = activityStatus,

						};

						if (lesson != null && lesson.Id > 0 && sa.ActivityTypeId == 2)
						{ // Obter arquivos associados à lição
							arquivos = _arquivoFactory.GetAll()
								.Where(a => a.TableAction == "Lessons" && a.TableId == lesson.Id)
								.ToList();
							List<Lesson> lessonObj = new List<Lesson>();

							lessonObj.Add(lesson);

							activityDto.DetailPage = GetLessonPage(lessonObj, arquivos);
						}
						if (lesson != null && lesson.Id > 0 && sa.ActivityTypeId == 1)
						{ // Obter arquivos associados à lição
							arquivos = _arquivoFactory.GetAll()
								.Where(a => a.TableAction == "Lessons" && a.TableId == lesson.Id)
								.ToList();
							List<Lesson> lessonObj = new List<Lesson>();

							lessonObj.Add(lesson);

							activityDto.DetailPage = GetLessonPage(lessonObj, arquivos);
						}

						missionDto.Activities.Add(activityDto);
					}

				if (studyDay.IsCompleted)
				{
					// Missão já concluída
					completedMissions.Add(missionDto);
				}
				else if (studyDay.Date < today)
				{
					missionDto.QtdQuestions = "";
					missionDto.QtdQuestionsNOk = "";
					missionDto.QtdQuestionsOk = "";
					missionDto.Resumo = "";

					// Missão atrasada
					overdueMissions.Add(missionDto);
				}
				else if (studyDay.Date == today)
				{
					// Missão atual (do dia)
					currentMission = missionDto;
				}
				else
				{
					missionDto.QtdQuestions = "";
					missionDto.QtdQuestionsNOk = "";
					missionDto.QtdQuestionsOk = "";
					missionDto.Resumo = "";

					// Missões futuras
					upcomingMissions.Add(missionDto);
				}
			}

			// Se não houver missão atual (por exemplo, se a missão de hoje já foi concluída ou não existe), precisamos definir a próxima missão disponível como atual
			if (currentMission == null)
			{
				// Procurar a próxima missão não concluída
				StudyDay nextStudyDay = null;

				if (overdueMissions.Any())
				{
					// Se houver missões atrasadas, pegar a primeira
					nextStudyDay = journey.StudyDays
						.Where(sd => !sd.IsCompleted && sd.Date < today)
						.OrderBy(sd => sd.Date)
						.FirstOrDefault();
				}
				else if (upcomingMissions.Any())
				{
					// Se não houver atrasadas, pegar a próxima missão futura
					nextStudyDay = journey.StudyDays
						.Where(sd => !sd.IsCompleted && sd.Date > today)
						.OrderBy(sd => sd.Date)
						.FirstOrDefault();
				}

				if (nextStudyDay != null)
				{
					// Atualizar a data da missão para hoje
					nextStudyDay.Date = today;

					// Atualizar o banco de dados
					_studyDayRepository.UpdateObj(nextStudyDay);
					databaseUpdated = true;

					// Reprocessar as missões após a atualização
					return GetUserMissions(emailAddress);
				}
				else
				{
					// Não há mais missões disponíveis
					currentMission = null;
				}
			}

			// Se o banco de dados foi atualizado, podemos atualizar a porcentagem de conclusão da jornada
			if (databaseUpdated)
			{
				UpdateJourneyCompletion(journey.Id);
			}

			currentMission.CompletionPercentege = journey.CompletionPercentage;
			currentMission.StartDate = journey.CreatedAt.ToShortDateString();
			currentMission.ConclusionDate = upcomingMissions != null && upcomingMissions.Count > 0 ? upcomingMissions.LastOrDefault().Date.ToShortDateString() : GetBrasiliaTime().ToShortDateString();


			var userMissionsDto = new UserMissionsDto
			{
				CurrentMission = currentMission,
				OverdueMissions = overdueMissions,
				CompletedMissions = completedMissions,
				UpcomingMissions = upcomingMissions
			};


			try
			{
				int currentDurationTotal = currentMission.Activities
			   .Where(b => b.ActivityType == "Estudo")
			   .Sum(b => b.Duration);

				int overdueDurationTotal = overdueMissions
				.SelectMany(mission => mission.Activities)
				.Where(activity => activity.ActivityType == "Estudo")
				.Sum(activity => activity.Duration);

				int completedDurationTotal = completedMissions
				.SelectMany(mission => mission.Activities)
				.Where(activity => activity.ActivityType == "Estudo")
				.Sum(activity => activity.Duration);

				int upcomingDurationTotal = upcomingMissions
				.SelectMany(mission => mission.Activities)
				.Where(activity => activity.ActivityType == "Estudo")
				.Sum(activity => activity.Duration);

				userMissionsDto.CurrentMission.TotalLessonsWatched = completedMissions
				.SelectMany(mission => mission.Activities)
				.Where(activity => activity.ActivityType == "Estudo").Count();

				userMissionsDto.CurrentMission.TotalCompletedDuration = completedMissions
				.SelectMany(mission => mission.Activities)
				.Sum(activity => activity.Duration);


				userMissionsDto.CurrentMission.TotalDuration = currentDurationTotal + overdueDurationTotal + completedDurationTotal + upcomingDurationTotal;
			}
			catch (Exception ex)
			{

			}

			return userMissionsDto;
		}


		private LessonDetailPage GetLessonPage(List<Lesson> lesson, List<Arquivo> arquivos)
		{
			LessonDetailPage lessonDetailPage = new LessonDetailPage();

			// Obter a lição e preencher o LessonItemShort com os dados básicos
			lessonDetailPage.Page = lesson
				.Select(b => new LessonItemShort
				{
					Id = b.Id,
					Title = b.Title,
					Order = b.Order,
					Description = b.Description,
					IsFeatured = b.IsFeatured ?? false,
					Thumbnail = b.Thumbnail,
					Url = b.Guid.ToString(),
					VideoContent = b.VideoContent
				})
				.FirstOrDefault();

			if (lessonDetailPage.Page != null)
			{
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


		// Método principal de geração da jornada
		public void GenerateJourney(string emailAddress, int courseId, Dictionary<DayOfWeek, int> studyHoursPerDay, bool resetBasicModules, bool resetAdvancedModules)
		{
			Usuario usuario = _usuarioFactory.GetByEmail(emailAddress);

			if (usuario is null)
				return;

			int userId = usuario.Id;

			var course = _courseRepository.GetObj(courseId);
			var courseModules = _courseModuleRepository.GetByCourseId(courseId)
				.Where(cm => cm.Module.IsActive && !cm.Module.IsDeleted)
				.ToList();

			// Separar módulos básicos e avançados
			var basicModules = courseModules.Where(cm => cm.Module.ModuleType.Name == "Básico").ToList();
			var advancedModules = courseModules.Where(cm => cm.Module.ModuleType.Name == "Avançado").ToList();

			// Obter lições para distribuição
			var basicLessons = resetBasicModules ? GetOrderedLessons(basicModules) : GetRemainingLessons(basicModules, userId);
			var advancedLessons = resetAdvancedModules ? GetOrderedLessons(advancedModules) : GetRemainingLessons(advancedModules, userId);

			int totalLessons = basicLessons.Count + advancedLessons.Count;

			// Determinar quando desbloquear os módulos avançados (após 50% das lições básicas)
			int basicLessonsUntilAdvancedUnlock = basicLessons.Count / 2;

			// Gerar os dias de estudo com base nos pesos e regras fornecidas
			var studyDays = GenerateStudyDaysBasedOnWeights(DateTime.Today, studyHoursPerDay, basicLessons, advancedLessons, basicLessonsUntilAdvancedUnlock);

			// Agendar revisões após todas as aulas terem sido agendadas
			ScheduleRevisions(studyDays, studyHoursPerDay);

			var journey = new Journey
			{
				UsuarioId = userId,
				CourseId = courseId,
				StartDate = DateTime.Today,
				JourneyStudyHours = studyHoursPerDay.Select(sh => new JourneyStudyHours
				{
					DayOfWeek = sh.Key,
					StudyHours = sh.Value
				}).ToList(),
				StudyDays = studyDays.Values.OrderBy(sd => sd.Date).ToList(),
				TotalLessons = totalLessons,
				CompletionPercentage = 0.0M
			};

			_journeyRepository.SaveObj(journey);

			usuario.JourneyId = journey.Id;

			_usuarioFactory.UpdateObj(usuario);

		}

		public DateTime SafeAddDays(DateTime dateTime, int days)
		{
			//if (days > 0 && dateTime > SafeAddDays(DateTime.MaxValue, -days))
			//{
			//	return DateTime.MaxValue;
			//}
			//else if (days < 0 && dateTime < SafeAddDays(DateTime.MinValue, -days))
			//{
			//	return DateTime.MinValue;
			//}
			return dateTime.AddDays(days);
		}


		// Método para agendar revisões após as aulas
		private void ScheduleRevisions(Dictionary<DateTime, StudyDay> studyDays, Dictionary<DayOfWeek, int> studyMinutesPerDay)
		{
			// Obter todas as atividades de estudo (aulas)
			var studyActivities = studyDays.Values
				.SelectMany(sd => sd.StudyActivities)
				.Where(sa => sa.ActivityTypeId == GetActivityTypeId("Estudo"))
				.ToList();

			// Determinar a última data necessária para as revisões
			DateTime lastRevisionDate = DateTime.MinValue;

			foreach (var activity in studyActivities)
			{
				var lessonDate = activity.StudyDay.Date;
				var revisionOffsets = new[] { 3, 7, 15 };

				foreach (var offset in revisionOffsets)
				{
					var desiredRevisionDate = SafeAddDays(lessonDate, offset);
					if (desiredRevisionDate > lastRevisionDate)
					{
						lastRevisionDate = desiredRevisionDate;
					}
				}
			}

			// Estender o cronograma até a última data de revisão necessária
			var currentDate = studyDays.Keys.Max().AddDays(1);
			while (currentDate <= lastRevisionDate)
			{
				if (studyMinutesPerDay.ContainsKey(currentDate.DayOfWeek))
				{
					if (!studyDays.ContainsKey(currentDate))
					{
						studyDays[currentDate] = new StudyDay
						{
							Date = currentDate,
							IsCompleted = false,
							StudyActivities = new List<StudyActivity>()
						};
					}
				}
				currentDate = SafeAddDays(currentDate, 1);
			}

			// Agendar as revisões
			foreach (var activity in studyActivities)
			{
				var lessonDate = activity.StudyDay.Date;
				var lesson = activity.LessonId;

				var revisionOffsets = new[] { 3, 7, 15 };

				foreach (var offset in revisionOffsets)
				{
					var desiredRevisionDate = SafeAddDays(lessonDate, offset);

					// Ajustar a data de revisão para o próximo dia de estudo disponível
					var revisionDate = GetNextStudyDate(desiredRevisionDate, studyMinutesPerDay);

					// Garantir que a data de revisão seja posterior à data da aula
					if (revisionDate <= lessonDate)
					{
						revisionDate = GetNextStudyDate(SafeAddDays(lessonDate, 1), studyMinutesPerDay);
						while (revisionDate <= lessonDate)
						{
							revisionDate = GetNextStudyDate(SafeAddDays(revisionDate, 1), studyMinutesPerDay);
						}
					}

					// Verificar se há horas de estudo neste dia
					if (!studyMinutesPerDay.TryGetValue(revisionDate.DayOfWeek, out int totalStudyMinutes))
					{
						continue; // Não é possível agendar a revisão neste dia
					}

					// Certificar-se de que o StudyDay existe
					if (!studyDays.TryGetValue(revisionDate, out var revisionDay))
					{
						revisionDay = new StudyDay
						{
							Date = revisionDate,
							IsCompleted = false,
							StudyActivities = new List<StudyActivity>()
						};
						studyDays[revisionDate] = revisionDay;
					}

					// Verificar se já existe uma revisão para esta lição neste dia
					bool revisionExists = revisionDay.StudyActivities.Any(sa => sa.ActivityTypeId == GetActivityTypeId("Revisão") && sa.LessonId == lesson);
					if (revisionExists)
					{
						continue; // Revisão já agendada, não adicionar novamente
					}

					// Calcular a duração disponível para revisões
					int dailyActivitiesTime = (int)(totalStudyMinutes * DailyActivitiesPercentage);
					int existingRevisionsCount = revisionDay.StudyActivities.Count(sa => sa.ActivityTypeId == GetActivityTypeId("Revisão"));
					int revisionDuration = dailyActivitiesTime / 3; // Dividir tempo entre até 3 revisões

					// Ajustar a duração se houver mais revisões no dia
					if (existingRevisionsCount > 0)
					{
						revisionDuration = dailyActivitiesTime / (existingRevisionsCount + 1);
					}

					// Adicionar a atividade de revisão
					revisionDay.StudyActivities.Add(new StudyActivity
					{
						ActivityTypeId = GetActivityTypeId("Revisão"),
						LessonId = lesson,
						IsCompleted = false,
						Duration = revisionDuration,
						StudyDay = revisionDay
					});
				}
			}
		}


		private DateTime GetNextStudyDate(DateTime startDate, Dictionary<DayOfWeek, int> studyMinutesPerDay)
		{
			var daysOfWeek = studyMinutesPerDay.Keys.ToList();
			DateTime date = startDate;

			while (!daysOfWeek.Contains(date.DayOfWeek))
			{
				date = date.AddDays(1);
			}

			return date;
		}


		private Dictionary<DateTime, StudyDay> GenerateStudyDaysBasedOnWeights(
				DateTime startDate,
				Dictionary<DayOfWeek, int> studyMinutesPerDay,
				List<(Lesson Lesson, Module Module)> basicLessons,
				List<(Lesson Lesson, Module Module)> advancedLessons,
				int basicLessonsUntilAdvancedUnlock)
		{
			var studyDays = new Dictionary<DateTime, StudyDay>();
			var currentDate = GetNextStudyDate(startDate, studyMinutesPerDay);

			// Inicializar filas de lições para módulos básicos e avançados
			var basicLessonsByModule = basicLessons
				.GroupBy(lm => lm.Module.Id)
				.ToDictionary(g => g.Key, g => new Queue<(Lesson Lesson, Module Module)>(g));

			var advancedLessonsByModule = advancedLessons
				.GroupBy(lm => lm.Module.Id)
				.ToDictionary(g => g.Key, g => new Queue<(Lesson Lesson, Module Module)>(g));

			// Obter módulos ordenados por peso
			var basicModulesByWeight = basicLessonsByModule.Keys
				.Select(moduleId => basicLessons.First(lm => lm.Module.Id == moduleId).Module)
				.OrderByDescending(m => m.Weight)
				.ToList();

			var advancedModulesByWeight = advancedLessonsByModule.Keys
				.Select(moduleId => advancedLessons.First(lm => lm.Module.Id == moduleId).Module)
				.OrderByDescending(m => m.Weight)
				.ToList();

			var random = new Random();

			int basicLessonsScheduled = 0;
			int totalBasicLessons = basicLessons.Count;
			bool advancedModulesUnlocked = false;

			// Agendamento das lições
			while (basicLessonsByModule.Any(bl => bl.Value.Count > 0) || advancedLessonsByModule.Any(al => al.Value.Count > 0))
			{
				if (!studyMinutesPerDay.TryGetValue(currentDate.DayOfWeek, out int totalStudyMinutes))
				{
					currentDate = GetNextStudyDate(currentDate.AddDays(1), studyMinutesPerDay);
					continue;
				}

				var studyDay = new StudyDay
				{
					Date = currentDate,
					IsCompleted = false,
					StudyActivities = new List<StudyActivity>()
				};

				int dailyActivitiesTime = (int)Math.Round(totalStudyMinutes * DailyActivitiesPercentage);
				int lessonsTime = totalStudyMinutes - dailyActivitiesTime;
				int usedLessonMinutes = 0;

				// Verificar se os módulos avançados devem ser desbloqueados
				if (!advancedModulesUnlocked && basicLessonsScheduled >= basicLessonsUntilAdvancedUnlock)
				{
					advancedModulesUnlocked = true;
				}

				// Selecionar módulos para o dia
				var modulesForTheDay = new List<Module>();

				// Selecionar módulos básicos se ainda houver lições
				if (basicLessonsByModule.Any(bl => bl.Value.Count > 0))
				{
					var selectedBasicModules = SelectModulesForTheDay(basicModulesByWeight, basicLessonsByModule, 2, random);
					modulesForTheDay.AddRange(selectedBasicModules);
				}

				// Selecionar módulos avançados se desbloqueados
				if (advancedModulesUnlocked && advancedLessonsByModule.Any(al => al.Value.Count > 0))
				{
					var selectedAdvancedModules = SelectModulesForTheDay(advancedModulesByWeight, advancedLessonsByModule, 2, random);
					modulesForTheDay.AddRange(selectedAdvancedModules);
				}

				// Garantir que haja pelo menos dois módulos
				modulesForTheDay = modulesForTheDay.Distinct().Take(2).ToList();

				foreach (var module in modulesForTheDay)
				{
					var lessonsByModuleDict = basicLessonsByModule.ContainsKey(module.Id) ? basicLessonsByModule : advancedLessonsByModule;
					var lessonQueue = lessonsByModuleDict[module.Id];

					if (lessonQueue == null || lessonQueue.Count == 0)
						continue;

					// Definir número de aulas a serem agendadas deste módulo
					int numberOfLessons = (module.Weight >= 10) ? 2 : 1; // Pelo menos duas aulas para módulo de peso maior
					int lessonsScheduledFromModule = 0;

					while (lessonQueue.Count > 0 && usedLessonMinutes < lessonsTime && lessonsScheduledFromModule < numberOfLessons)
					{
						var (lesson, _) = lessonQueue.Peek();
						int lessonDuration = lesson.Duration ?? 0;

						if (usedLessonMinutes + lessonDuration <= lessonsTime)
						{
							// Adicionar atividade de estudo
							AddStudyActivity(studyDay, lesson, "Estudo", lessonDuration, currentDate);
							usedLessonMinutes += lessonDuration;
							lessonQueue.Dequeue();
							lessonsScheduledFromModule++;

							// Incrementar contador de lições básicas agendadas
							if (basicLessonsByModule.ContainsKey(module.Id))
							{
								basicLessonsScheduled++;
							}
						}
						else
						{
							break; // Não há tempo suficiente para esta lição
						}
					}
				}

				if (usedLessonMinutes > 0)
				{
					// Atividades diárias (Resumo e Questões)
					ScheduleDailyActivities(studyDay, dailyActivitiesTime);

					studyDays[currentDate] = studyDay;
				}

				// Avançar para o próximo dia de estudo
				currentDate = GetNextStudyDate(currentDate.AddDays(1), studyMinutesPerDay);
			}

			return studyDays;
		}



		private void ScheduleRevisionsForLesson(
	   Lesson lesson,
	   DateTime lessonDate,
	   Dictionary<DateTime, StudyDay> studyDays,
	   Dictionary<DayOfWeek, int> studyMinutesPerDay,
	   HashSet<DateTime> scheduledRevisionDates)
		{
			var revisionOffsets = new[] { 3, 7, 15 };

			foreach (var offset in revisionOffsets)
			{
				var initialRevisionDate = SafeAddDays(lessonDate, offset);
				var revisionDate = initialRevisionDate;

				// Ajustar a data de revisão para o próximo dia de estudo disponível
				revisionDate = GetNextStudyDate(revisionDate, studyMinutesPerDay);

				// Garantir que a data de revisão seja posterior à data da aula
				if (revisionDate <= lessonDate)
				{
					// Avançar para o próximo dia de estudo após a data da aula
					revisionDate = GetNextStudyDate(SafeAddDays(lessonDate, 1), studyMinutesPerDay);

					// Caso ainda seja anterior ou igual à data da aula, continuar avançando
					while (revisionDate <= lessonDate)
					{
						revisionDate = GetNextStudyDate(SafeAddDays(revisionDate, 1), studyMinutesPerDay);
					}
				}

				// Adicionar a data de revisão ao conjunto
				scheduledRevisionDates.Add(revisionDate);

				// Verificar se há horas de estudo neste dia
				if (!studyMinutesPerDay.TryGetValue(revisionDate.DayOfWeek, out int totalStudyMinutes))
				{
					continue; // Não é possível agendar a revisão neste dia
				}

				// Certificar-se de que o StudyDay existe
				if (!studyDays.TryGetValue(revisionDate, out var revisionDay))
				{
					revisionDay = new StudyDay
					{
						Date = revisionDate,
						IsCompleted = false,
						StudyActivities = new List<StudyActivity>()
					};
					studyDays[revisionDate] = revisionDay;
				}

				// Verificar se já existe uma revisão para esta lição neste dia
				bool revisionExists = revisionDay.StudyActivities.Any(sa => sa.ActivityTypeId == GetActivityTypeId("Revisão") && sa.LessonId == lesson.Id);
				if (revisionExists)
				{
					continue; // Revisão já agendada, não adicionar novamente
				}

				// Calcular a duração disponível para revisões
				int dailyActivitiesTime = (int)(totalStudyMinutes * DailyActivitiesPercentage);
				int existingRevisionsCount = revisionDay.StudyActivities.Count(sa => sa.ActivityTypeId == GetActivityTypeId("Revisão"));
				int revisionDuration = dailyActivitiesTime / 3; // Dividir tempo entre até 3 revisões

				// Ajustar a duração se houver mais revisões no dia
				if (existingRevisionsCount > 0)
				{
					revisionDuration = dailyActivitiesTime / (existingRevisionsCount + 1);
				}

				// Adicionar a atividade de revisão
				revisionDay.StudyActivities.Add(new StudyActivity
				{
					ActivityTypeId = GetActivityTypeId("Revisão"),
					LessonId = lesson.Id,
					IsCompleted = false,
					Duration = revisionDuration,
					StudyDay = revisionDay
				});
			}
		}

		private List<Module> SelectModulesForTheDay(List<Module> modulesByWeight, Dictionary<int, Queue<(Lesson Lesson, Module Module)>> lessonsByModule, int numberOfModules, Random random)
		{
			// Filtrar módulos com lições disponíveis
			var availableModules = modulesByWeight
				.Where(m => lessonsByModule.ContainsKey(m.Id) && lessonsByModule[m.Id].Count > 0)
				.ToList();

			// Selecionar módulos aleatoriamente com base no peso
			var selectedModules = new List<Module>();
			int totalWeight = availableModules.Sum(m => m.Weight);

			while (selectedModules.Count < numberOfModules && availableModules.Count > 0)
			{
				int randValue = random.Next(0, totalWeight);
				int cumulativeWeight = 0;
				Module selectedModule = null;

				foreach (var module in availableModules)
				{
					cumulativeWeight += module.Weight;
					if (randValue < cumulativeWeight)
					{
						selectedModule = module;
						break;
					}
				}

				if (selectedModule != null)
				{
					selectedModules.Add(selectedModule);
					totalWeight -= selectedModule.Weight;
					availableModules.Remove(selectedModule);
				}
				else
				{
					break;
				}
			}

			return selectedModules;
		}

		private void ScheduleRevisionsForLesson(Lesson lesson, DateTime lessonDate, Dictionary<DateTime, StudyDay> studyDays, Dictionary<DayOfWeek, int> studyMinutesPerDay)
		{
			var revisionOffsets = new[] { 3, 7, 15 };

			foreach (var offset in revisionOffsets)
			{
				var revisionDate = SafeAddDays(lessonDate, offset);

				if (!studyMinutesPerDay.TryGetValue(revisionDate.DayOfWeek, out int totalStudyMinutes))
				{
					// Se o dia não é um dia de estudo, continue
					continue;
				}

				// Verificar se o StudyDay já existe ou criar um novo
				if (!studyDays.TryGetValue(revisionDate, out var revisionDay))
				{
					revisionDay = new StudyDay
					{
						Date = revisionDate,
						IsCompleted = false,
						StudyActivities = new List<StudyActivity>()
					};
					studyDays[revisionDate] = revisionDay;
				}

				// Verificar se já existe uma revisão para esta lição neste dia
				bool revisionExists = revisionDay.StudyActivities.Any(sa => sa.ActivityTypeId == GetActivityTypeId("Revisão") && sa.LessonId == lesson.Id);
				if (revisionExists)
				{
					continue; // Revisão já agendada, não adicionar novamente
				}

				// Calcular a duração disponível para revisões
				int dailyActivitiesTime = (int)(totalStudyMinutes * DailyActivitiesPercentage);
				int existingRevisionsCount = revisionDay.StudyActivities.Count(sa => sa.ActivityTypeId == GetActivityTypeId("Revisão"));
				int revisionDuration = dailyActivitiesTime / 3; // Dividir tempo entre até 3 revisões

				// Ajustar a duração se houver mais revisões no dia
				if (existingRevisionsCount > 0)
				{
					revisionDuration = dailyActivitiesTime / (existingRevisionsCount + 1);
				}

				// Adicionar a atividade de revisão
				revisionDay.StudyActivities.Add(new StudyActivity
				{
					ActivityTypeId = GetActivityTypeId("Revisão"),
					LessonId = lesson.Id,
					IsCompleted = false,
					Duration = revisionDuration,
					StudyDay = revisionDay
				});
			}
		}

		public StudyActivityDto GetStudyActivity(int activityId)
		{
			var activity = _studyActivityRepository.GetObj(activityId);
			if (activity != null)
			{

				string activityType = "";
				switch (activity.ActivityTypeId)
				{
					case 1: activityType = "Revisão"; break;
					case 2: activityType = "Estudo"; break;
					case 3: activityType = "Resumo"; break;
					case 4: activityType = "Questões"; break;
				}


				return new StudyActivityDto()
				{
					IsCompleted = activity.IsCompleted,
					ActivityType = activityType,
					LessonId = activity.LessonId,
					Id = activityId
				};
			}
			else
			{
				throw new Exception($"StudyActivity with ID {activityId} not found.");
			}
		}


		public NotebookContentDto DetailNotebook(int activityId)
		{
			var activity = _studyActivityRepository.GetObj(activityId);

			if (activity == null)
			{
				throw new Exception($"StudyActivity with ID {activityId} not found.");
			}

			Notebook notebook = _notebookRepository.GetByLessonAndStudyActivity(activity.LessonId, activity.Id);

			if (notebook is null || notebook.Id == 0)
			{
				return new NotebookContentDto() { Content = "" };
			}
			else
			{
				return new NotebookContentDto() { Content = notebook.Content };
			}
		}

		public void UpdateNotebook(int activityId, string Resume)
		{
			var activity = _studyActivityRepository.GetObj(activityId);

			if (activity == null)
			{
				throw new Exception($"StudyActivity with ID {activityId} not found.");
			}

			Notebook notebook = _notebookRepository.GetByLessonAndStudyActivity(activity.LessonId, activity.Id);

			if (notebook is null)
			{
				_notebookRepository.SaveObj(new Notebook()
				{
					LessonId = activity.LessonId.Value,
					StudyActivityId = activityId,
					Content = Resume,
					Date = GetBrasiliaTime()
				});
			}
			else
			{
				notebook.Content = Resume;
				notebook.Date = GetBrasiliaTime();

				_notebookRepository.UpdateObj(notebook);
			}
		}

		public void MarkActivityAsStarted(int activityId)
		{
			var activity = _studyActivityRepository.GetObj(activityId);
			if (activity != null)
			{
				activity.IsStarted = true;
				activity.StartDate = GetBrasiliaTime();

				_studyActivityRepository.UpdateObj(activity);
			}
			else
			{
				throw new Exception($"StudyActivity with ID {activityId} not found.");
			}
		}

		public ActivityCompleteDto MarkActivityAsCompleted(int activityId)
		{
			var activity = _studyActivityRepository.GetObj(activityId);
			if (activity != null)
			{
				activity.IsCompleted = true;
				activity.EndDate = GetBrasiliaTime();

				_studyActivityRepository.UpdateObj(activity);

				return new ActivityCompleteDto() { IsConcluded = ConfirmCurrentMissionCompletion(activity.StudyDayId) };
			}
			else
			{
				throw new Exception($"StudyActivity with ID {activityId} not found.");
			}
		}

		public bool ConfirmCurrentMissionCompletion(int studyDayId)
		{
			var studyDay = _studyDayRepository.GetObj(studyDayId);
			if (studyDay == null)
			{
				throw new Exception($"StudyDay with ID {studyDayId} not found.");
			}

			//List<StudyActivity> studyActivities = _studyActivityRepository.GetByStudyDayId(studyDayId).ToList();

			bool allActivitiesCompleted = studyDay.StudyActivities.All(activity => activity.IsCompleted);

			if (allActivitiesCompleted)
			{
				studyDay.IsCompleted = true;
				_studyDayRepository.UpdateObj(studyDay);

				UpdateJourneyCompletion(studyDay.JourneyId);
				return true;
			}

			return false;
		}

		public void UpdateJourneyCompletion(int journeyId)
		{
			var journey = _journeyRepository.GetObj(journeyId);
			if (journey == null)
			{
				throw new Exception($"Journey with ID {journeyId} not found.");
			}

			int completedLessons = journey.StudyDays
				.SelectMany(sd => sd.StudyActivities)
				.Count(activity => activity.IsCompleted && activity.LessonId.HasValue);

			journey.CompletionPercentage = journey.TotalLessons > 0
				? Math.Round((decimal)completedLessons / journey.TotalLessons * 100, 2)
				: 0;

			_journeyRepository.UpdateObj(journey);
		}

		public List<(Lesson Lesson, Module Module)> GetOrderedLessons(List<CourseModule> modules)
		{
			var lessons = new List<(Lesson Lesson, Module Module)>();

			foreach (var module in modules)
			{
				var moduleLessons = _moduleLessonRepository
					.GetByModuleId(module.Module.Id)
					.OrderBy(ml => ml.Order)
					.Select(ml => (ml.Lesson, module.Module))
					.ToList();

				lessons.AddRange(moduleLessons);
			}

			return lessons;
		}

		public List<(Lesson Lesson, Module Module)> GetRemainingLessons(List<CourseModule> modules, int userId)
		{
			var remainingLessons = new List<(Lesson Lesson, Module Module)>();

			var completedLessonIds = _studyActivityRepository
				.GetCompletedLessonsForUser(userId)
				.Select(sa => sa.LessonId)
				.ToHashSet();

			foreach (var module in modules)
			{
				var moduleLessons = _moduleLessonRepository
					.GetByModuleId(module.Module.Id)
					.Where(ml => !completedLessonIds.Contains(ml.Lesson.Id))
					.OrderBy(ml => ml.Order)
					.Select(ml => (ml.Lesson, module.Module))
					.ToList();

				remainingLessons.AddRange(moduleLessons);
			}

			return remainingLessons;
		}

		private void AddStudyActivity(StudyDay studyDay, Lesson lesson, string activityType, int duration, DateTime date)
		{
			studyDay.StudyActivities.Add(new StudyActivity
			{
				ActivityTypeId = GetActivityTypeId(activityType),
				LessonId = lesson.Id,
				IsCompleted = false,
				Duration = duration,
				StudyDay = studyDay
			});
		}

		public int GetActivityTypeId(string activityTypeName)
		{
			var activityType = _activityTypeRepository.GetByName(activityTypeName);
			if (activityType == null)
			{
				throw new Exception($"ActivityType '{activityTypeName}' not found.");
			}
			return activityType.Id;
		}

		private void ScheduleDailyActivities(StudyDay studyDay, int dailyActivitiesTime)
		{
			int activityCount = 2;
			int activityDuration = dailyActivitiesTime / activityCount;

			studyDay.StudyActivities.Add(new StudyActivity
			{
				ActivityTypeId = GetActivityTypeId("Questões"),
				IsCompleted = false,
				Duration = activityDuration,
				StudyDay = studyDay
			});

			studyDay.StudyActivities.Add(new StudyActivity
			{
				ActivityTypeId = GetActivityTypeId("Resumo"),
				IsCompleted = false,
				Duration = activityDuration,
				StudyDay = studyDay
			});
		}
	}
}
