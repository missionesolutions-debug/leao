from fastapi import APIRouter
from src.controllers.user_controller import UserController

router = APIRouter()
user_controller = UserController()

@router.post("/users/")
async def create_user(user_data: dict):
    return await user_controller.create_user(user_data)

@router.get("/users/{user_id}")
async def get_user(user_id: int):
    return await user_controller.get_user(user_id)

@router.put("/users/{user_id}")
async def update_user(user_id: int, user_data: dict):
    return await user_controller.update_user(user_id, user_data)

@router.delete("/users/{user_id}")
async def delete_user(user_id: int):
    return await user_controller.delete_user(user_id)