from fastapi import Depends
from sqlalchemy.orm import Session

from app.repositories.user_repository import UserRepository
from app.schemas.user_schema import UserCreate
from app.services.user_service import UserService
from app.config.database import get_db

def register(user: UserCreate, db: Session = Depends(get_db)):
    user_repository = UserRepository(db)
    user_service = UserService(user_repository)
    if user_service.get_user_by_email(user.email, db):
        return {"error": "Email already registered"}
    return user_service.create_user(user, db)
class UserController:
    def __init__(self, user_service):
        self.user_service = user_service

    async def create_user(self, user_data, db: Session):
        """
        Handle user creation request.
        :param user_data: Data for the new user.
        :param db: Sessão do banco de dados.
        :return: Created user information.
        """
        return await self.user_service.create_user(user_data, db)

    async def get_user(self, user_id, db: Session):
        """
        Handle request to retrieve user information.
        :param user_id: ID of the user to retrieve.
        :param db: Sessão do banco de dados.
        :return: User information.
        """
        return await self.user_service.get_user(user_id, db)

    async def update_user(self, user_id, user_data, db: Session):
        """
        Handle request to update user information.
        :param user_id: ID of the user to update.
        :param user_data: Updated user data.
        :param db: Sessão do banco de dados.
        :return: Updated user information.
        """
        return await self.user_service.update_user(user_id, user_data, db)

    async def delete_user(self, user_id, db: Session):
        """
        Handle request to delete a user.
        :param user_id: ID of the user to delete.
        :param db: Sessão do banco de dados.
        :return: Confirmation of deletion.
        """
        return await self.user_service.delete_user(user_id, db)