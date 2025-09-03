from sqlalchemy.orm import Session
from app.models.user_model import User
class UserRepository:
    def __init__(self, db):
        self.db = db

    def create_user(self, user_data):
        user = user_data.dict()
        self.db.users.insert_one(user)
        return user

    def get_user_by_id(self, user_id):
        return self.db.users.find_one({"_id": user_id})

    def get_user_by_email(self, email):
        return self.db.users.find_one({"email": email})

    def update_user(self, user_id, update_data):
        self.db.users.update_one({"_id": user_id}, {"$set": update_data})
        return self.get_user_by_id(user_id)

    def delete_user(self, user_id):
        self.db.users.delete_one({"_id": user_id})
        return {"message": "User deleted successfully"}