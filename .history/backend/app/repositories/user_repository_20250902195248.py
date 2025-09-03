from sqlalchemy.orm import Session
from app.models.user_model import User

class UserRepository:
    def __init__(self, db):
        self.db = db

    def create(self, user_data):
        user = User(**user_data.dict())
        self.db.add(user)
        self.db.commit()
        self.db.refresh(user)
        return user

    def get_user_by_id(self, user_id):
        return self.db.query(User).filter(User.id == user_id).first()

    def get_by_email(self, email):
        return self.db.query(User).filter(User.email == email).first()

    def update_user(self, user_id, update_data):
        self.db.query(User).filter(User.id == user_id).update(update_data)
        self.db.commit()
        return self.get_user_by_id(user_id)

    def delete_user(self, user_id):
        self.db.query(User).filter(User.id == user_id).delete()
        self.db.commit()
        return {"message": "User deleted successfully"}