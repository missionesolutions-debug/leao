from app.repositories.user_repository import UserRepository
from app.schemas.user_schema import UserCreate, UserUpdate, UserResponse
from app.models.user_model import User
from sqlalchemy.orm import Session
from fastapi import HTTPException, status

class UserService:
    def __init__(self, db: Session):
        self.db = db
        self.user_repository = UserRepository(db)

    def create_user(self, user_create: UserCreate) -> UserResponse:
        existing_user = self.user_repository.get_by_email(user_create.email)
        if existing_user:
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="Email already registered"
            )
        user = self.user_repository.create(user_create)
        return UserResponse.from_orm(user)

    def get_user(self, user_id: int) -> UserResponse:
        user = self.user_repository.get_user(user_id)
        if not user:
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND,
                detail="User not found"
            )
        return UserResponse.from_orm(user)
    
    def get_user_by_email(self, email):
        return self.user_repository.get_by_email(email)

    def update_user(self, user_id: int, user_update: UserUpdate) -> UserResponse:
        user = self.user_repository.get_user(user_id)
        if not user:
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND,
                detail="User not found"
            )
        updated_user = self.user_repository.update_user(user_id, user_update)
        return UserResponse.from_orm(updated_user)

    def delete_user(self, user_id: int) -> None:
        user = self.user_repository.get_user(user_id)
        if not user:
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND,
                detail="User not found"
            )
        self.user_repository.delete_user(user_id)