from pydantic import BaseModel
from typing import Optional

class UserBase(BaseModel):
    Id: int
    Nome: str
    Email: str
    Password: str
    RoleGate: str
    Avatar: Optional[str] = None

class UserCreate(UserBase):
    Password: str

class User(UserBase):
    Id: int

    class Config:
        from_attributes = True
