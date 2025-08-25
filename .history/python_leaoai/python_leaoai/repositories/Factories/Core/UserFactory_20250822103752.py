from sqlalchemy.orm import Session
from sqlalchemy.exc import SQLAlchemyError
from typing import List, Optional
from framework.data.models.core.user import User


class UserRepository:
    def __init__(self, db: Session):
        self.db = db

    # --------------------
    # CRUD
    # --------------------
    def get_obj(self, id: int) -> Optional[User]:
        return self.db.query(User).filter(User.id == id).first()

    def save_obj(self, obj: User) -> User:
        self.db.add(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def update_obj(self, obj: User) -> User:
        self.db.merge(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def get_all(self) -> List[User]:
        return self.db.query(User).all()

    def get_all_ativo(self) -> List[User]:
        return self.db.query(User).filter(User.ativo == True).all()

    def get_user_by_email(self, user_email: str) -> Optional[User]:
        return (
            self.db.query(User)
            .filter(User.ativo == True, User.email == user_email)
            .first()
        )

    def get_by_email(self, email: str) -> Optional[User]:
        return self.db.query(User).filter(User.email == email).first()

    def get_all_excluido(self) -> List[User]:
        return self.db.query(User).filter(User.excluido == True).all()

    def remove_obj(self, obj: User) -> bool:
        try:
            self.db.delete(obj)
            self.db.commit()
            return True
        except SQLAlchemyError:
            self.db.rollback()
            return False

    def delete_obj(self, id: int) -> bool:
        try:
            obj = self.get_obj(id)
            if not obj:
                return False
            obj.ativo = False
            obj.excluido = True
            self.db.merge(obj)
            self.db.commit()
            return True
        except SQLAlchemyError:
            self.db.rollback()
            return False

    def get(self, email: str, password: str) -> Optional[User]:
        return (
            self.db.query(User)
            .filter(User.email.ilike(email), User.password == password)
            .first()
        )
