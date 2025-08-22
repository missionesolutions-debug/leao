from typing import List

# Supondo que você tenha classes equivalentes aos DTOs e modelos
class StudyHoursDto:
    def __init__(self, day_of_week: int, study_hours: int):
        self.day_of_week = day_of_week
        self.study_hours = study_hours

class StudyActivityDto:
    def __init__(self, id: int, activity_type: str, lesson_id: int, lesson_name: str, is_completed: bool):
        self.id = id
        self.activity_type = activity_type
        self.lesson_id = lesson_id
        self.lesson_name = lesson_name
        self.is_completed = is_completed

class StudyDayDto:
    def __init__(self, id: int, date, is_completed: bool, study_activities: List[StudyActivityDto]):
        self.id = id
        self.date = date
        self.is_completed = is_completed
        self.study_activities = study_activities

class JourneyDto:
    def __init__(self, usuario_id: int, course_id: int, study_hours_per_day: List[StudyHoursDto]):
        self.usuario_id = usuario_id
        self.course_id = course_id
        self.study_hours_per_day = study_hours_per_day

class JourneyMapping:

    @staticmethod
    def map_to_journey_dto(journey) -> JourneyDto:
        study_hours = [
            StudyHoursDto(jsh.day_of_week, jsh.study_hours)
            for jsh in getattr(journey, "journey_study_hours", [])
        ]
        return JourneyDto(
            usuario_id=getattr(journey, "usuario_id", None),
            course_id=getattr(journey, "course_id", None),
            study_hours_per_day=study_hours
        )

    @staticmethod
    def map_to_study_day_dto(study_day) -> StudyDayDto:
        activities = [
            StudyActivityDto(
                id=sa.id,
                activity_type=getattr(sa.activity_type, "name", None),
                lesson_id=getattr(sa, "lesson_id", None),
                lesson_name=getattr(sa.lesson, "title", None) if getattr(sa, "lesson", None) else None,
                is_completed=sa.is_completed
            )
            for sa in getattr(study_day, "study_activities", [])
        ]
        return StudyDayDto(
            id=getattr(study_day, "id", None),
            date=getattr(study_day, "date", None),
            is_completed=getattr(study_day, "is_completed", False),
            study_activities=activities
        )
