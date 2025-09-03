class UserController:
    def __init__(self, user_service):
        self.user_service = user_service

    async def create_user(self, user_data):
        """
        Handle user creation request.
        :param user_data: Data for the new user.
        :return: Created user information.
        """
        return await self.user_service.create_user(user_data)

    async def get_user(self, user_id):
        """
        Handle request to retrieve user information.
        :param user_id: ID of the user to retrieve.
        :return: User information.
        """
        return await self.user_service.get_user(user_id)

    async def update_user(self, user_id, user_data):
        """
        Handle request to update user information.
        :param user_id: ID of the user to update.
        :param user_data: Updated user data.
        :return: Updated user information.
        """
        return await self.user_service.update_user(user_id, user_data)

    async def delete_user(self, user_id):
        """
        Handle request to delete a user.
        :param user_id: ID of the user to delete.
        :return: Confirmation of deletion.
        """
        return await self.user_service.delete_user(user_id)