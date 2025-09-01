from typing import List, Optional
from app.schemas.user_schema import UserSchema
from app.repositories.user_repository import UserRepository

class UserService:
    def __init__(self, user_repository: UserRepository):
        self.user_repository = user_repository

    def register_user(self, user_data: UserSchema) -> UserSchema:
        return self.user_repository.add_user(user_data)

    def fetch_user(self, user_id: int) -> Optional[UserSchema]:
        return self.user_repository.find_user(user_id)

    def modify_user(self, user_id: int, user_data: UserSchema) -> Optional[UserSchema]:
        return self.user_repository.update_user(user_id, user_data)

    def remove_user(self, user_id: int) -> bool:
        return self.user_repository.delete_user(user_id)