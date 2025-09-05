from sqlalchemy.orm import Session
from app.models.user_model import User
from passlib.context import CryptContext

pwd_context = CryptContext(schemes=["bcrypt"], deprecated="auto")

class UserRepository:
    def __init__(self, db: Session):
        self.db = db

    def create(self, user_data):
        hashed_password = pwd_context.hash(user_data.password)
        user = User(
            username=user_data.username,
            email=user_data.email,
            hashed_password=hashed_password,
            )
        self.db.add(user)
        self.db.commit()
        self.db.refresh(user)
        return user

    def get_by_id(self, user_id):
        return self.db.query(User).filter(User.id == user_id).first()

    def get_by_email(self, email):
        return self.db.query(User).filter(User.email == email).first()

    def update_user(self, user_id, update_data):
        self.db.query(User).filter(User.id == user_id).update(update_data)
        self.db.commit()
        return self.get_by_id(user_id)

    def delete_user(self, user_id):
        self.db.query(User).filter(User.id == user_id).delete()
        self.db.commit()
        return {"message": "User deleted successfully"}
    
    def get_all_users(self):
        return self.db.query(User).all()