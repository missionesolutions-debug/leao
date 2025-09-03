from fastapi import APIRouter, Depends, HTTPException
from app.services.user_service import UserService
from app.schemas.user_schema import UserCreate, UserUpdate, UserResponse
from app.config.database import get_db

router = APIRouter()

@router.post("/users/", response_model=UserResponse)
def create_user(user: UserCreate, db=Depends(get_db)):
    return UserService(db).create_user(user)

@router.get("/users/{user_id}", response_model=UserResponse)
def get_user(user_id: int, db=Depends(get_db)):
    user = UserService(db).get_user(user_id)
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