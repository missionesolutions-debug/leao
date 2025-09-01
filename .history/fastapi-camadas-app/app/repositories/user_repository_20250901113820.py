from sqlalchemy.orm import Session
from app.models.user_model import User
from app.schemas.user_schema import UserSchema
from app.services.user_service import UserService

from app.config.settings import get_db  # Supondo que exista uma função para obter o db

db = get_db()  # Ajuste conforme sua implementação
user_repository = UserRepository(db)
user_service = UserService(user_repository)

class UserRepository:
    def __init__(self, db: Session):
        self.db = db

    def add_user(self, user: UserSchema) -> User:
        db_user = User(**user.dict())
        self.db.add(db_user)
        self.db.commit()
        self.db.refresh(db_user)
        return db_user

    def find_user(self, user_id: int) -> User:
        return self.db.query(User).filter(User.id == user_id).first()

    def update_user(self, user_id: int, user: UserSchema) -> User:
        db_user = self.find_user(user_id)
        if db_user:
            for key, value in user.dict().items():
                setattr(db_user, key, value)
            self.db.commit()
            self.db.refresh(db_user)
        return db_user

    def delete_user(self, user_id: int) -> None:
        db_user = self.find_user(user_id)
        if db_user:
            self.db.delete(db_user)
            self.db.commit()