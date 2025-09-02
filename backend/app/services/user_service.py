class UserService:
    def __init__(self, user_repository):
        self.user_repository = user_repository

    def create_user(self, user_data):
        return self.user_repository.create(user_data)

    def get_user(self, user_id):
        user = self.user_repository.get_user_by_id(user_id)
        return user
    
    def get_user_by_email(self, email):
        return self.user_repository.get_by_email(email)

    def update_user(self, user_id, user_data):
        updated_user = self.user_repository.update_user(user_id, user_data)
        return updated_user

    def delete_user(self, user_id):
        return self.user_repository.delete_user(user_id)

    def validate_user(self, user_data):
        pass