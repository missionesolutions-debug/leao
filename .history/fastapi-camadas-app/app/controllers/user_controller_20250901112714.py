from fastapi import APIRouter, HTTPException
from app.schemas.user_schema import UserSchema
from app.services.user_service import UserService

router = APIRouter()
user_service = UserService()

@router.post("/users/", response_model=UserSchema)
async def create_user(user: UserSchema):
    return await user_service.register_user(user)

@router.get("/users/{user_id}", response_model=UserSchema)
async def get_user(user_id: int):
    user = await user_service.fetch_user(user_id)
    if user is None:
        raise HTTPException(status_code=404, detail="User not found")
    return user

@router.put("/users/{user_id}", response_model=UserSchema)
async def update_user(user_id: int, user: UserSchema):
    updated_user = await user_service.modify_user(user_id, user)
    if updated_user is None:
        raise HTTPException(status_code=404, detail="User not found")
    return updated_user

@router.delete("/users/{user_id}")
async def delete_user(user_id: int):
    success = await user_service.remove_user(user_id)
    if not success:
        raise HTTPException(status_code=404, detail="User not found")
    return {"detail": "User deleted successfully"}