from typing import List, Optional
from datetime import datetime
from pydantic import BaseModel # Assuming services might use Pydantic models
from abc import ABC, abstractmethod # For interfaces

class IJourneyService(ABC):
    @abstractmethod
    def get_user_missions(self, email_address): object # TODO: Specify correct type hint):
    pass

    @abstractmethod
    def generate_journey(self, email_address): object # TODO: Specify correct type hint, course_id: object # TODO: Specify correct type hint, dictionary<_day_of_week: object # TODO: Specify correct type hint, study_hours_per_day: object # TODO: Specify correct type hint, reset_basic_modules: object # TODO: Specify correct type hint, reset_advanced_modules: object # TODO: Specify correct type hint):
    pass

    @abstractmethod
    def mark_activity_as_completed(self, activity_id): object # TODO: Specify correct type hint):
    pass

    @abstractmethod
    def confirm_current_mission_completion(self, study_day_id): object # TODO: Specify correct type hint):
    pass

    @abstractmethod
    def update_journey_completion(self, journey_id): object # TODO: Specify correct type hint):
    pass

    @abstractmethod
    def get_activity_type_id(self, activity_type_name): object # TODO: Specify correct type hint):
    pass

    @abstractmethod
    def mark_activity_as_started(self, activity_id): object # TODO: Specify correct type hint):
    pass

    @abstractmethod
    def get_study_activity(self, activity_id): object # TODO: Specify correct type hint):
    pass

    @abstractmethod
    def update_study_day_info(self, study_day_id): object # TODO: Specify correct type hint, resumo: object # TODO: Specify correct type hint, qtd_questions: object # TODO: Specify correct type hint, qtd_questions_ok: object # TODO: Specify correct type hint, qtd_questions_n_ok: object # TODO: Specify correct type hint):
    pass

    @abstractmethod
    def update_notebook(self, activity_id): object # TODO: Specify correct type hint, resume: object # TODO: Specify correct type hint):
    pass

    @abstractmethod
    def detail_notebook(self, activity_id): object # TODO: Specify correct type hint):
        pass

    @abstractmethod
    def get_study_day_details(self, study_day_id): object # TODO: Specify correct type hint):
        pass

