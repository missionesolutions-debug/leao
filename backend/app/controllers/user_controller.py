from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session
from app.services.user_service import UserService
from app.repositories.user_repository import UserRepository
from app.schemas.user_schema import UserCreate, UserUpdate, UserResponse
from app.config.database import get_db

router = APIRouter()

@router.get("/usuarios/", response_model=list[UserResponse])
def list_users(db: Session = Depends(get_db)):
    user_service = UserService(db)
    return user_service.get_all_users()

@router.post("/usuarios/", response_model=UserResponse)
def create_user(user: UserCreate, db: Session = Depends(get_db)):
    user_service = UserService(db)
    return user_service.create_user(user)

@router.get("/usuarios/{user_id}", response_model=UserResponse)
def get_user(user_id: int, db: Session = Depends(get_db)):
    user_service = UserService(db)
    user = user_service.get_user_by_id(user_id)
    return user

@router.put("/usuarios/{user_id}", response_model=UserResponse)
def update_user(user_id: int, user_update: UserUpdate, db: Session = Depends(get_db)):
    user_service = UserService(db)
    user = user_service.update_user(user_id, user_update)
    return user

@router.delete("/usuarios/{user_id}")
def delete_user(user_id: int, db: Session = Depends(get_db)):
    user_service = UserService(db)
    result = user_service.delete_user(user_id)
    return {"detail": "Usuário deletado com sucesso"}