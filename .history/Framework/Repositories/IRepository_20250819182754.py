from abc import ABC, abstractmethod
from typing import List, Optional
from datetime import datetime
from sqlalchemy.orm import Session
from LeaoPy.Framework.Data.database import SessionLocal # Placeholder for DbContext equivalent
from LeaoPy.Framework.Data.models import YourModel # TODO: Import specific models used by this repository

class IRepository(ABC):
    @abstractmethod
    def get_obj(self, id): object # TODO: Specify correct type hint):
    pass

    @abstractmethod
    def save_obj(self, obj): object # TODO: Specify correct type hint):
    pass

    @abstractmethod
    def update_obj(self, obj): object # TODO: Specify correct type hint):
    pass

    @abstractmethod
    def remove_obj(self, obj): object # TODO: Specify correct type hint):
    pass

    @abstractmethod
    def delete_obj(self, id): object # TODO: Specify correct type hint):
    pass

    @abstractmethod
    def get_by_url(self, url): object # TODO: Specify correct type hint):
    pass

    @abstractmethod
    def get_by_id(self, id): object # TODO: Specify correct type hint):
    pass

    @abstractmethod
    def add(self, entity): object # TODO: Specify correct type hint):
    pass

    @abstractmethod
    def update(self, entity): object # TODO: Specify correct type hint):
    pass

    @abstractmethod
    def delete(self, entity): object # TODO: Specify correct type hint):
    pass

