from abc import ABC, abstractmethod
from typing import List, Dict, Tuple
from dataclasses import dataclass
import datetime

# ------------------------
# Modelos equivalentes (simplificados, você pode expandir conforme a estrutura real do seu projeto C#)
# ------------------------

@dataclass
class Lesson:
    id: int
    title: str

@dataclass
class Module:
    id: int
    name: str

@dataclass
class CourseModule:
    module: Module
    lessons: List[Lesson]

@dataclass
class UserMissionsDto:
    user_id: int
    missions: List[str]

@dataclass
class ActivityCompleteDto:
    activity_id: int
    completed: bool

@dataclass
class StudyActivityDto:
    activity_id: int
    description: str

@dataclass
class NotebookContentDto:
    activity_id: int
    content: str

@dataclass
class StudyDayViewDto:
    study_day_id: int
    details: str

# ------------------------
# Interface IJourneyService
# ------------------------

class IJourneyService(ABC):

    @abstractmethod
    def get_user_missions(self, email_address: str) -> UserMissionsDto:
        """Retorna as missões de um usuário."""
        pass

    @abstractmethod
    def generate_journey(
        self,
        email_address: str,
        course_id: int,
        study_hours_per_day: Dict[datetime.date.weekday, int],
        reset_basic_modules: bool,
        reset_advanced_modules: bool
    ) -> None:
        """Gera uma nova jornada para o usuário com base nas preferências fornecidas."""
        pass

    @abstractmethod
    def mark_activity_as_completed(self, activity_id: int) -> ActivityCompleteDto:
        """Marca uma atividade específica como concluída."""
        pass

    @abstractmethod
    def confirm_current_mission_completion(self, study_day_id: int) -> bool:
        """Confirma a conclusão da missão atual."""
        pass

    @abstractmethod
    def update_journey_completion(self, journey_id: int) -> None:
        """Atualiza o progresso da jornada."""
        pass

    @abstractmethod
    def get_activity_type_id(self, activity_type_name: str) -> int:
        """Recupera o ID de um tipo de atividade com base no nome."""
        pass

    @abstractmethod
    def get_ordered_lessons(self, modules: List[CourseModule]) -> List[Tuple[Lesson, Module]]:
        """Obtém as lições ordenadas para os módulos fornecidos."""
        pass

    @abstractmethod
    def get_remaining_lessons(self, modules: List[CourseModule], user_id: int) -> List[Tuple[Lesson, Module]]:
        """Obtém as lições restantes para os módulos, excluindo as concluídas."""
        pass

    @abstractmethod
    def mark_activity_as_started(self, activity_id: int) -> None:
        """Marca uma atividade como iniciada."""
        pass

    @abstractmethod
    def get_study_activity(self, activity_id: int) -> StudyActivityDto:
        """Obtém os detalhes de uma atividade de estudo."""
        pass

    @abstractmethod
    def update_study_day_info(
        self,
        study_day_id: int,
        resumo: str,
        qtd_questions: str,
        qtd_questions_ok: str,
        qtd_questions_nok: str
    ) -> None:
        """Atualiza informações do dia de estudo."""
        pass

    @abstractmethod
    def update_notebook(self, activity_id: int, resume: str) -> None:
        """Atualiza o caderno de anotações da atividade."""
        pass

    @abstractmethod
    def detail_notebook(self, activity_id: int) -> NotebookContentDto:
        """Recupera o conteúdo detalhado do caderno."""
        pass

    @abstractmethod
    def get_study_day_details(self, study_day_id: int) -> StudyDayViewDto:
        """Recupera os detalhes de um dia de estudo."""
        pass
