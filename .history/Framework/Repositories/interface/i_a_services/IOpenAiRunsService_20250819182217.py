from abc import ABC, abstractmethod
from typing import List, Optional
from datetime import datetime
from sqlalchemy.orm import Session
from LeaoPy.Framework.Data.database import SessionLocal # Placeholder for DbContext equivalent
from LeaoPy.Framework.Data.models import YourModel # TODO: Import specific models used by this repository

class IOpenAiRunsService(ABC):
    @abstractmethod
    def run(self, messages): object # TODO: Specify correct type hint):
    pass

