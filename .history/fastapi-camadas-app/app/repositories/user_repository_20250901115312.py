# from app.config.settings import SessionLocal  # Descomente ao conectar ao banco

from app.models.user_model import User
from app.repositories.memory import users_db, next_id

class UserRepository:
    # Descomente e ajuste ao conectar ao banco
    # def __init__(self, db_session):
    #     self.db = db_session

    def create_user(self, name: str, email: str) -> User:
        global next_id
        user = User(id=next_id, name=name, email=email)
        users_db[next_id] = user
        next_id += 1
        return user

    def get_user(self, user_id: int):
        return users_db.get(user_id)

    def update_user(self, user_id: int, name: str, email: str):
        user = users_db.get(user_id)
        if user:
            user.name = name
            user.email = email
        return user

    def delete_user(self, user_id: int):
        return users_db.pop(user_id, None)