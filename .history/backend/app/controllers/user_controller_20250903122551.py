from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session
from app.repositories.user_repository import UserRepository
from app.services.user_service import UserService
from app.schemas.user_schema import UserCreate, UserUpdate, UserResponse
from app.config.database import get_db
from app.utils.auth import get_current_user

router = APIRouter()

@router.post("/usuarios", response_model=UserResponse)
def create_user(user: UserCreate, db: Session = Depends(get_db)):
    user_repository = UserRepository(db)
    user_service = UserService(user_repository)
    db_user = user_service.get_user_by_email(user.email)
    if db_user:
        raise HTTPException(status_code=400, detail="Email já cadastrado")
    return user_service.create_user(user)

@router.get("/usuarios/{user_id}", response_model=UserResponse)
def get_user(user_id: int, db: Session = Depends(get_db), current_user=Depends(get_current_user)):
    user_repository = UserRepository(db)
    user_service = UserService(user_repository)
    user = user_service.get_user(user_id)
    if not user:
        raise HTTPException(status_code=404, detail="Usuário não encontrado")
    return user

@router.put("/usuarios/{user_id}", response_model=UserResponse)
def update_user(user_id: int, user_update: UserUpdate, db: Session = Depends(get_db), current_user=Depends(get_current_user)):
    user_repository = UserRepository(db)
    user_service = UserService(user_repository)
    user = user_service.update_user(user_id, user_update)
    if not user:
        raise HTTPException(status_code=404, detail="Usuário não encontrado")
    return user

@router.delete("/usuarios/{user_id}")
def delete_user(user_id: int, db: Session = Depends(get_db), current_user=Depends(get_current_user)):
    user_repository = UserRepository(db)
    user_service = UserService(user_repository)
    result = user_service.delete_user(user_id)
    if not result:
        raise HTTPException(status_code=404, detail="Usuário não encontrado")
    return {"detail": "Usuário deletado com sucesso"}