from app.repositories.user_repository import UserRepository

class UserService:
    def __init__(self):
        self.repo = UserRepository()

    def create_user(self, name: str, email: str):
        return self.repo.create_user(name, email)

    def get_user(self, user_id: int):
        return self.repo.get_user(user_id)

    def update_user(self, user_id: int, name: str, email: str):
        return self.repo.update_user(user_id, name, email)

    def delete_user(self, user_id: int):
        return self.repo.delete_user(user_id)