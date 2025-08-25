from datetime import datetime, timedelta
from typing import List, Dict, Optional, Tuple
import pytz
import random

# --- DTOs equivalentes ---
class ActivityDto:
    def __init__(self, activity_id=None, activity_type=None, lesson_id=None, lesson_guid=None,
                 lesson_title=None, lesson_video_content=None, start_date=None, end_date=None,
                 duration=0, execution_time=0, is_started=False, is_completed=False,
                 status=None, detail_page=None, resume=None):
        self.activity_id = activity_id
        self.activity_type = activity_type
        self.lesson_id = lesson_id
        self.lesson_guid = lesson_guid
        self.lesson_title = lesson_title
        self.lesson_video_content = lesson_video_content
        self.start_date = start_date
        self.end_date = end_date
        self.duration = duration
        self.execution_time = execution_time
        self.is_started = is_started
        self.is_completed = is_completed
        self.status = status
        self.detail_page = detail_page
        self.resume = resume


class StudyDayViewDto:
    def __init__(self, study_day_id=None, date=None, is_completed=False, resumo=None,
                 qtd_questions=None, qtd_questions_ok=None, qtd_questions_nok=None,
                 activities: List[ActivityDto] = None):
        self.study_day_id = study_day_id
        self.date = date
        self.is_completed = is_completed
        self.resumo = resumo
        self.qtd_questions = qtd_questions
        self.qtd_questions_ok = qtd_questions_ok
        self.qtd_questions_nok = qtd_questions_nok
        self.activities = activities or []


class UserMissionsDto:
    def __init__(self, current_mission=None, overdue_missions=None, completed_missions=None,
                 upcoming_missions=None):
        self.current_mission = current_mission
        self.overdue_missions = overdue_missions or []
        self.completed_missions = completed_missions or []
        self.upcoming_missions = upcoming_missions or []


# --- Serviço principal ---
class JourneyService:
    DAILY_ACTIVITIES_PERCENTAGE = 0.3

    def __init__(self, course_repo, course_module_repo, module_lesson_repo,
                 journey_repo, study_day_repo, study_activity_repo,
                 activity_type_repo, usuario_factory, arquivo_factory,
                 lesson_repo, notebook_repo, module_repo):
        self._course_repo = course_repo
        self._course_module_repo = course_module_repo
        self._module_lesson_repo = module_lesson_repo
        self._journey_repo = journey_repo
        self._study_day_repo = study_day_repo
        self._study_activity_repo = study_activity_repo
        self._activity_type_repo = activity_type_repo
        self._usuario_factory = usuario_factory
        self._arquivo_factory = arquivo_factory
        self._lesson_repo = lesson_repo
        self._notebook_repo = notebook_repo
        self._module_repo = module_repo

    # --- Timezone BR ---
    @staticmethod
    def get_brasilia_time() -> datetime:
        tz = pytz.timezone("America/Sao_Paulo")
        return datetime.now(tz)

    # --- Get StudyDay details ---
    def get_study_day_details(self, study_day_id: int) -> StudyDayViewDto:
        study_day = self._study_day_repo.get_obj(study_day_id)
        if not study_day:
            raise Exception("StudyDay não encontrado.")

        study_activities = self._study_activity_repo.get_by_study_day_id(study_day_id)
        activities_dto = []

        for sa in study_activities:
            activity_type_name = self._activity_type_repo.get_obj(sa.activity_type_id).name
            activity_dto = ActivityDto(
                activity_id=sa.id,
                activity_type=activity_type_name,
                lesson_id=sa.lesson_id,
                is_started=bool(sa.is_started),
                is_completed=sa.is_completed,
                duration=sa.duration
            )
            if activity_type_name == "Estudo":
                lesson = self._lesson_repo.get_obj(sa.lesson_id)
                arquivos = [a for a in self._arquivo_factory.get_all() if a.table_action == "Lessons" and a.table_id == sa.lesson_id]
                activity_dto.detail_page = self.get_lesson_page([lesson], arquivos)
                notebook = self._notebook_repo.get_by_lesson_and_study_activity(sa.lesson_id, sa.id)
                if notebook:
                    activity_dto.resume = notebook.content
            activities_dto.append(activity_dto)

        return StudyDayViewDto(
            study_day_id=study_day.id,
            date=study_day.date,
            is_completed=study_day.is_completed,
            resumo=study_day.resumo,
            qtd_questions=study_day.qtd_questions,
            qtd_questions_ok=study_day.qtd_questions_ok,
            qtd_questions_nok=study_day.qtd_questions_nok,
            activities=activities_dto
        )

    # --- Update StudyDay ---
    def update_study_day_info(self, study_day_id: int, resumo=None, qtd_questions=None,
                              qtd_questions_ok=None, qtd_questions_nok=None):
        study_day = self._study_day_repo.get_obj(study_day_id)
        if not study_day:
            raise Exception("StudyDay não encontrado.")

        if resumo:
            study_day.resumo = resumo
        if qtd_questions:
            study_day.qtd_questions = qtd_questions
        if qtd_questions_ok:
            study_day.qtd_questions_ok = qtd_questions_ok
        if qtd_questions_nok:
            study_day.qtd_questions_nok = qtd_questions_nok

        self._study_day_repo.update_obj(study_day)

    # --- Método auxiliar para obter lesson page ---
    @staticmethod
    def get_lesson_page(lessons: List, arquivos: List):
        # Retorna estrutura similar ao LessonDetailPage
        page = lessons[0] if lessons else None
        lesson_files = [{
            "file_name": a.file_name,
            "file_source": a.file_data,
            "file_image": a.file_image,
            "file_description": a.description
        } for a in arquivos]
        return {"page": page, "lesson_files": lesson_files}

    # --- Adicionar outros métodos de GenerateJourney, ScheduleRevisions etc. ---
    # A lógica seria convertida mantendo loops, dicionários e datetime do Python
    # Métodos como mark_activity_as_started, mark_activity_as_completed, update_journey_completion
    # também seriam traduzidos diretamente.

    # --- SafeAddDays ---
    @staticmethod
    def safe_add_days(date_time: datetime, days: int) -> datetime:
        return date_time + timedelta(days=days)
