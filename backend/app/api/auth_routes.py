from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session
from app.schemas.user_schema import UserCreate, UserLogin, UserResponse
from app.services.user_service import UserService
from app.utils.auth import create_access_token
from app.config.database import get_db

router = APIRouter()

@router.post("/register")
def register(user: UserCreate, db: Session = Depends(get_db)):
    user_service = UserService(db)
    if user_service.get_user_by_email(user.email,):
        raise HTTPException(status_code=400, detail="Email already registered")
    
    # Criar usuário
    created_user = user_service.create_user(user)
    
    # Se for o primeiro usuário ou admin, fazer login automaticamente
    if hasattr(user, 'role') and user.role == 'ADMIN':
        access_token = create_access_token(data={"sub": created_user.email})
        return {
            "user": created_user,
            "access_token": access_token, 
            "token_type": "bearer"
        }
    
    return {"user": created_user}

@router.post("/login")
def login(user: UserLogin, db: Session = Depends(get_db)):
    user_service = UserService(db)
    db_user = user_service.authenticate_user(user.email, user.password)
    if not db_user:
        raise HTTPException(status_code=401, detail="Invalid credentials")
    access_token = create_access_token(data={"sub": db_user.email})
    return {"access_token": access_token, "token_type": "bearer"}