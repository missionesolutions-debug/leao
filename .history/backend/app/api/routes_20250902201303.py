from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session
from from app.schemas.user_schema import UserResponse
from app.repositories.user_repository import UserRepository
from app.config.database import get_db
from app.models.user_model import User
from backend.app.schemas.user_schema import UserCreate
from backend.app.services.user_service import UserService

router = APIRouter()



# User routes
@router.get("/usuarios/")
def read_users(db: Session = Depends(get_db)):
    users = db.query(User).all()
    return users

@router.post("/register", response_model=UserResponse)
def register(user: UserCreate, db: Session = Depends(get_db)):
    user_repository = UserRepository(db)
    user_service = UserService(user_repository)
    if user_service.get_user_by_email(user.email):
        raise HTTPException(status_code=400, detail="Email already registered")
    return user_service.create_user(user)