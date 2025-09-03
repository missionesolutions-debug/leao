from app.models.user_model import User


class UserService:
    def __init__(self, user_repository):
        self.user_repository = user_repository

    def create_user(self, user_data):
        # Logic to create a user
        user = self.user_repository.create(user_data)
        return user

    def get_user(self, user_id):
        # Logic to retrieve a user by ID
        user = self.user_repository.get_by_id(user_id)
        return user
    
    def get_user_by_email(self, email, db):
        return db.query(User).filter(User.email == email).first()

    def update_user(self, user_id, user_data):
        # Logic to update user information
        updated_user = self.user_repository.update(user_id, user_data)
        return updated_user

    def delete_user(self, user_id):
        # Logic to delete a user
        self.user_repository.delete(user_id)

    def validate_user(self, user_data):
        # Logic to validate user data
        # This could include checking for existing users, password strength, etc.
        pass