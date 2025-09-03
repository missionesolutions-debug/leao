from fastapi import APIRouter, Depends
from sqlalchemy.orm import Session
from app.repositories.user_repository import UserRepository
from app.config.database import get_db
from app.models.user_model import User

router = APIRouter()



# User routes
@router.get("/usuarios/")
def read_users(db: Session = Depends(get_db)):
    users = db.query(User).all()
    return users