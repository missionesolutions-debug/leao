from typing import List
from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session
from app.repositories.user_repository import UserRepository
from app.schemas.user_schema import UserCreate, UserUpdate, UserResponse
from app.utils.auth import get_current_user
from app.config.database import get_db
from app.models.user_model import User

router = APIRouter()

@router.post("/", response_model=UserResponse)
async def create_user(user: UserCreate, db: Session = Depends(get_db)):
    user_repository = UserRepository(db)
    created_user = user_repository.create(user)
    return created_user

@router.get("/{user_id}", response_model=UserResponse)
async def get_user(user_id: int, db: Session = Depends(get_db), current_user: UserResponse = Depends(get_current_user)):
    user_repository = UserRepository(db)
    user = user_repository.get_by_id(user_id)
    if not user:
        raise HTTPException(status_code=404, detail="User not found")
    return user

@router.get("/", response_model=List[UserResponse])
async def get_all_users(db: Session = Depends(get_db), current_user: UserResponse = Depends(get_current_user)):
    user_repository = UserRepository(db)
    users = user_repository.get_all()
    return users

@router.put("/{user_id}", response_model=UserResponse)
async def update_user(user_id: int, user: UserUpdate, db: Session = Depends(get_db), current_user: UserResponse = Depends(get_current_user)):
    user_repository = UserRepository(db)
    update_data = user.dict(exclude_unset=True)
    db_user = user_repository.get_by_id(user_id)
    if not db_user:
        raise HTTPException(status_code=404, detail="User not found")
    for key, value in update_data.items():
        setattr(db_user, key, value)
    db.commit()
    db.refresh(db_user)
    return db_user

@router.delete("/{user_id}", response_model=dict)
async def delete_user(user_id: int, db: Session = Depends(get_db), current_user: UserResponse = Depends(get_current_user)):
    user_repository = UserRepository(db)
    db_user = user_repository.get_by_id(user_id)
    if not db_user:
        raise HTTPException(status_code=404, detail="User not found")
    db.delete(db_user)
    db.commit()
    return {"detail": "User deleted successfully"}

@router.get("/count")
async def count_users(db: Session = Depends(get_db)):
    """Contar total de usuários (público para verificar primeiro usuário)"""
    count = db.query(User).count()
    return {"count": count}