from fastapi import APIRouter, Depends, HTTPException
from app.controllers.user_controller import router as user_router
from app.schemas.user_schema import UserCreate, UserUpdate, UserResponse
from app.utils.auth import get_current_user
from backend.app.controllers import user_controller
    
router = APIRouter()


@router.post("/users/", response_model=UserResponse)
async def create_user(user: UserCreate):
    return await user_controller.create_user(user)

@router.get("/users/{user_id}", response_model=UserResponse)
async def get_user(user_id: int, current_user: UserResponse = Depends(get_current_user)):
    user = await user_controller.get_user(user_id)
    if not user:
        raise HTTPException(status_code=404, detail="User not found")
    return user

@router.put("/users/{user_id}", response_model=UserResponse)
async def update_user(user_id: int, user: UserUpdate, current_user: UserResponse = Depends(get_current_user)):
    updated_user = await user_controller.update_user(user_id, user)
    if not updated_user:
        raise HTTPException(status_code=404, detail="User not found")
    return updated_user

@router.delete("/users/{user_id}", response_model=dict)
async def delete_user(user_id: int, current_user: UserResponse = Depends(get_current_user)):
    result = await user_controller.delete_user(user_id)
    if not result:
        raise HTTPException(status_code=404, detail="User not found")
    return {"detail": "User deleted successfully"}