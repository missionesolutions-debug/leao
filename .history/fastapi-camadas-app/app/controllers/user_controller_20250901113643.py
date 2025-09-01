from fastapi import APIRouter, HTTPException
from app.schemas.user_schema import UserSchema
from app.repositories.user_repository import UserRepository
from app.services.user_service import UserService

router = APIRouter()

# Supondo que UserRepository precise de um db, ajuste conforme sua implementação
user_repository = UserRepository(db=None)  # Troque 'None' pela instância real do banco
user_service = UserService(user_repository)

@router.post("/users/", response_model=UserSchema)
async def create_user(user: UserSchema):
    return user_service.register_user(user)

@router.get("/users/{user_id}", response_model=UserSchema)
async def get_user(user_id: int):
    user = user_service.fetch_user(user_id)
    if user is None:
        raise HTTPException(status_code=404, detail="User not found")
    return user

@router.put("/users/{user_id}", response_model=UserSchema)
async def update_user(user_id: int, user: UserSchema):
    updated_user = user_service.modify_user(user_id, user)
    if updated_user is None:
        raise HTTPException(status_code=404, detail="User not found")
    return updated_user

@router.delete("/users/{user_id}")
async def delete_user(user_id: int):
    success = user_service.remove_user(user_id)
    if not success:
        raise HTTPException(status_code=404, detail="User not found")
    return {"detail": "User deleted successfully"}