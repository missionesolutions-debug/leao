from fastapi import APIRouter, HTTPException
from app.controllers.user_controller import UserController
from app.schemas.user_schema import UserCreate, UserRead

router = APIRouter()
user_controller = UserController()

@router.post("/users/", response_model=UserRead)
async def create_user(user_data: UserCreate):
    return await user_controller.create_user(user_data)

@router.get("/users/{user_id}", response_model=UserRead)
async def get_user(user_id: int):
    user = await user_controller.get_user(user_id)
    if not user:
        raise HTTPException(status_code=404, detail="Usuário não encontrado")
    return user

@router.put("/users/{user_id}", response_model=UserRead)
async def update_user(user_id: int, user_data: UserCreate):
    user = await user_controller.update_user(user_id, user_data)
    if not user:
        raise HTTPException(status_code=404, detail="Usuário não encontrado")
    return user

@router.delete("/users/{user_id}")
async def delete_user(user_id: int):
    return await user_controller.delete_user(user_id)