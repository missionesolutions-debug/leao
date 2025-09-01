from app.services.user_service import UserService
from app.schemas.user_schema import UserCreate, UserRead

class UserController:
    def __init__(self):
        self.service = UserService()

    async def create_user(self, user_data: UserCreate):
        user = self.service.create_user(user_data.name, user_data.email)
        return UserRead(id=user.id, name=user.name, email=user.email)

    async def get_user(self, user_id: int):
        user = self.service.get_user(user_id)
        if user:
            return UserRead(id=user.id, name=user.name, email=user.email)
        return None

    async def update_user(self, user_id: int, user_data: UserCreate):
        user = self.service.update_user(user_id, user_data.name, user_data.email)
        if user:
            return UserRead(id=user.id, name=user.name, email=user.email)
        return None

    async def delete_user(self, user_id: int):
        user = self.service.delete_user(user_id)
        if user:
            return {"message": "Usuário deletado com sucesso"}
        return {"message": "Usuário não encontrado"}