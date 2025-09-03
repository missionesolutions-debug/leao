from app.models.user_model import User


class UserService:
    def __init__(self, user_repository):
        self.user_repository = user_repository

    def create_user(self, user_data, db):
        return self.user_repository.create(user_data, db)

    def get_user(self, user_id, db):
        # Logic to retrieve a user by ID
        user = self.user_repository.get_by_id(user_id, db)
        return user
    
    def get_user_by_email(self, email):
        return self.user_repository.get_by_email(email)

    def update_user(self, user_id, user_data, db):
        # Logic to update user information
        updated_user = self.user_repository.update(user_id, user_data, db)
        return updated_user

    def delete_user(self, user_id, db):
        # Logic to delete a user
        self.user_repository.delete(user_id, db)

    def validate_user(self, user_data):
        # Logic to validate user data
        # This could include checking for existing users, password strength, etc.
        pass